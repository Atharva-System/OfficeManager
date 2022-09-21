using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.Application.Feature.UserRoles.Queries
{
    public record SearchUserRoles : IRequest<IResponse>
    {
        public string Search { get; init; } = string.Empty;
        public int Page_No { get; init; } = 1;
        public int Page_Size { get; init; } = 10;
        public string SortingColumn { get; set; } = "Name";
        public string SortingDirection { get; set; } = "ASC";
    }

    public class SearchUserRolesHandler : IRequestHandler<SearchUserRoles, IResponse>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SearchUserRolesHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IResponse> Handle(SearchUserRoles request, CancellationToken cancellationToken)
        {
            var roles = _context.Roles.AsQueryable().OrderBy(request.SortingColumn, (request.SortingDirection.ToLower() == "desc" ? false : true));
            if (string.IsNullOrEmpty(request.Search))
            {
                return new DataResponse<PaginatedList<RolesDTO>>(await roles
                    .ProjectTo<RolesDTO>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync<RolesDTO>(request.Page_No, request.Page_Size),
                    StatusCodes.Accepted,
                    Messages.DataFound);
            }
            return new DataResponse<PaginatedList<RolesDTO>>(await roles
                .AsNoTracking()
                .Where(d => d.Name.Contains(request.Search))
                .ProjectTo<RolesDTO>(_mapper.ConfigurationProvider)
                .PaginatedListAsync<RolesDTO>(request.Page_No, request.Page_Size),
                StatusCodes.Accepted,
                Messages.DataFound);
        }
    }
}
