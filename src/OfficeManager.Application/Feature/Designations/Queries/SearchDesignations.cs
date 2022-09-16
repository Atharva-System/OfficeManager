using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;

namespace OfficeManager.Application.Feature.Designations.Queries
{
    public record SearchDesignations : IRequest<Response<PaginatedList<DesignationDTO>>>
    {
        public string Search { get; set; } = string.Empty;
        public int Page_No { get; set; } = 1;
        public int Page_Size { get; set; } = 10;
        public string SortingColumn { get; set; } = "Name";
        public string SortingDirection { get; set; } = "ASC";
    }
    public class SearchDesginatoinsHandler : IRequestHandler<SearchDesignations, Response<PaginatedList<DesignationDTO>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SearchDesginatoinsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<PaginatedList<DesignationDTO>>> Handle(SearchDesignations request, CancellationToken cancellationToken)
        {
            Response<PaginatedList<DesignationDTO>> response = new Response<PaginatedList<DesignationDTO>>();

            try
            {
                var designations = _context.Designation.AsQueryable().OrderBy(request.SortingColumn, (request.SortingDirection.ToLower() == "desc" ? false : true));
                if (string.IsNullOrEmpty(request.Search))
                {
                    response._Data = await designations
                        .ProjectTo<DesignationDTO>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync<DesignationDTO>(request.Page_No, request.Page_Size);
                }
                else
                {
                    response._Data = await designations
                        .AsNoTracking()
                        .Where(d => d.Name.Contains(request.Search))
                        .ProjectTo<DesignationDTO>(_mapper.ConfigurationProvider)
                        .PaginatedListAsync<DesignationDTO>(request.Page_No, request.Page_Size);
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
