﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Exceptions;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.Users.Commands
{
    public record LoginUser : IRequest<IResponse>
    {
        public string EmployeeNo { get; init; }
        public string Password { get; init; } = string.Empty;
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUser, IResponse>
    {
        private readonly IApplicationDbContext Context;
        private readonly IMapper Mapper;
        private readonly ICurrentUserServices CurrentUserService;
        private readonly ITokenService TokenService;
        public LoginUserCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserServices currentUserService , ITokenService tokenService)
        {
            Context = context;
            Mapper = mapper;
            CurrentUserService = currentUserService;
            TokenService = tokenService;
        }

        public async Task<IResponse> Handle(LoginUser request, CancellationToken cancellationToken)
        {
            var user = await Context.Users.Include(x => x.Employee)
            .FirstOrDefaultAsync(a => a.Employee.EmployeeNo == Convert.ToInt32(request.EmployeeNo));

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return new ErrorResponse(StatusCodes.BadRequest,Messages.InvalidCredentials);
            }

            var userRoles = await Context.UserRoleMapping.Include(x => x.Roles).Where(d => d.UserId == user.Id)
                                .ProjectTo<UserRoleDTO>(Mapper.ConfigurationProvider).ToListAsync();

            LoggedInUserDTO loggedInUser = Mapper.Map<UserMaster, LoggedInUserDTO>(user);
            loggedInUser.Roles = userRoles;
            var tokendto = TokenService.CreateToken(loggedInUser);

            if (tokendto == null && tokendto.AccessToken == null)
            {
                return new ErrorResponse(StatusCodes.InternalServerError,Messages.IssueWithData);
            }

            RefreshToken refreshToken = new RefreshToken
            {
                UserId = loggedInUser.UserId,
                Code = tokendto.RefreshToken,
                Expiration = tokendto.RefreshTokenExpiration
            };

            Context.BeginTransaction();

            Context.RefreshToken.Add(refreshToken);
            await Context.SaveChangesAsync(cancellationToken);

            Context.CommitTransaction();

            CurrentUserService.loggedInUser = loggedInUser;

            return new DataResponse<TokenDTO>(tokendto, StatusCodes.Accepted,Messages.Success);
        }
    }
}
