using AutoMapper;
using FluentValidation;
using MediatR;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.Common.Models;

namespace OfficeManager.Application.Feature.Designations.Queries
{
    public record SearchDesignations(string search) : IRequest<Response<List<DesignationDTO>>>;

    public class SearchDesignationsQueryHandler : IRequestHandler<SearchDesignations, Response<List<DesignationDTO>>>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;
        public SearchDesignationsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Response<List<DesignationDTO>>> Handle(SearchDesignations request, CancellationToken cancellationToken)
        {
            Response<List<DesignationDTO>> response = new Response<List<DesignationDTO>>();
            try
            {
                if (!string.IsNullOrEmpty(request.search))
                {
                    response.Data = await context.Designation.Where(d => d.Name.Contains(request.search))
                        .ProjectToListAsync<DesignationDTO>(mapper.ConfigurationProvider);
                }
                else
                {
                    response.Data = await context.Designation
                        .ProjectToListAsync<DesignationDTO>(mapper.ConfigurationProvider);
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
