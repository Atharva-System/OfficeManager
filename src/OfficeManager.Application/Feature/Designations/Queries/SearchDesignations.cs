using AutoMapper;
using FluentValidation;
using MediatR;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Interfaces;

namespace OfficeManager.Application.Feature.Designations.Queries
{
    public record SearchDesignations : IRequest<Response<List<DesignationDTO>>>, ICacheable
    {
        public string Search { get; set; } = string.Empty;

        public bool BypassCache => false;

        public string CacheKey => CacheKeys.Designations;
    }

    public class SearchDesignationsQueryHandler : IRequestHandler<SearchDesignations, Response<List<DesignationDTO>>>
    {
        private readonly IApplicationDbContext Context;
        private readonly IMapper Mapper;
        public SearchDesignationsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<Response<List<DesignationDTO>>> Handle(SearchDesignations request, CancellationToken cancellationToken)
        {
            Response<List<DesignationDTO>> response = new Response<List<DesignationDTO>>();
            try
            {
                if (!string.IsNullOrEmpty(request.Search))
                {
                    response.Data = await Context.Designation.Where(d => d.Name.Contains(request.Search))
                        .ProjectToListAsync<DesignationDTO>(Mapper.ConfigurationProvider);
                }
                else
                {
                    response.Data = await Context.Designation
                        .ProjectToListAsync<DesignationDTO>(Mapper.ConfigurationProvider);
                }
                response.Message = response.Data.Count > 0 ? Messages.DataFound : Messages.NoDataFound;
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
