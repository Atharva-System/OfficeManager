using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using FluentValidation;

namespace OfficeManager.Application.ApplicationRoles.Queries
{
    public record GetUserRoles : IRequest<Response<List<RolesDTO>>>;

    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRoles, Response<List<RolesDTO>>>
    {
        private readonly IApplicationDbContext Context;
        private readonly IMapper Mapper;

        public GetUserRolesQueryHandler(IApplicationDbContext context,IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<Response<List<RolesDTO>>> Handle(GetUserRoles request, CancellationToken cancellationToken)
        {
            Response<List<RolesDTO>> response = new Response<List<RolesDTO>>();
            try
            {
                response._Data = new List<RolesDTO>();
                response._Data = await Context.Roles.ProjectTo<RolesDTO>(Mapper.ConfigurationProvider).ToListAsync();
                response.Message = response.Data.Count > 0 ? Messages.DataFound : Messages.NoDataFound;
                response.StatusCode = StausCodes.Accepted;
                response.IsSuccess = true;
                return response;
            }
            catch (ValidationException exception)
            {
                response.Errors = exception.Errors.Select(err => err.ErrorMessage)
                    .ToList();
                response.Message = "";
                response.StatusCode = StausCodes.BadRequest;
                response.IsSuccess = false;
                return response;
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
                response.Message = Messages.IssueWithData;
                response.StatusCode = StausCodes.InternalServerError;
                response.IsSuccess = false;
                return response;
            }
            
        }
    }
}
