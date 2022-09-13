using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;

namespace OfficeManager.Application.Feature.UserRoles.Queries
{
    public record SearchUserRoles : IRequest<Response<PaginatedList<RolesDTO>>>
    {
        public string Search { get; init; } = string.Empty;
        public int Page_No { get; init; } = 1;
        public int Page_Size { get; init; } = 10;
        public string SortingColumn { get; set; } = "Name";
        public string SortingDirection { get; set; } = "ASC";
    }

    public class SearchUserRolesHandler : IRequestHandler<SearchUserRoles, Response<PaginatedList<RolesDTO>>>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SearchUserRolesHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<PaginatedList<RolesDTO>>> Handle(SearchUserRoles request, CancellationToken cancellationToken)
        {
            Response<PaginatedList<RolesDTO>> response = new Response<PaginatedList<RolesDTO>>();
            try
            {
                var roles = _context.Roles.AsQueryable().OrderBy(request.SortingColumn, (request.SortingDirection.ToLower() == "desc" ? false : true));
                if (string.IsNullOrEmpty(request.Search))
                {
                    response._Data = await roles
                        .ProjectTo<RolesDTO>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync<RolesDTO>(request.Page_No, request.Page_Size);
                }
                else
                {
                    response._Data = await roles
                        .AsNoTracking()
                        .Where(d => d.Name.Contains(request.Search))
                        .ProjectTo<RolesDTO>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync<RolesDTO>(request.Page_No, request.Page_Size);
                }
                response.Message = response.Data.Items.Count > 0 ? Messages.DataFound : Messages.NoDataFound;
                response.StatusCode = StausCodes.Accepted;
                response.IsSuccess = true;
                return response;
            }
            catch (ValidationException exception)
            {
                response.Errors = exception.Errors.Select(err => err.ErrorMessage).ToList();
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
