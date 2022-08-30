using AutoMapper;
using FluentValidation;
using MediatR;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.Common.Models;

namespace OfficeManager.Application.Designations.Queries.SearchDesignations
{
    public record SearchDesignationsQuery(string search):IRequest<Response<List<DesignationDTO>>>;

    public class SearchDesignationsQueryHandler : IRequestHandler<SearchDesignationsQuery, Response<List<DesignationDTO>>>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;
        public SearchDesignationsQueryHandler(IApplicationDbContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Response<List<DesignationDTO>>> Handle(SearchDesignationsQuery request, CancellationToken cancellationToken)
        {
            Response<List<DesignationDTO>> response = new Response<List<DesignationDTO>>();
            try
            {
                if(!string.IsNullOrEmpty(request.search))
                {
                    response.Data = await context.Designation.Where(d => d.Name.Contains(request.search))
                        .ProjectToListAsync<DesignationDTO>(mapper.ConfigurationProvider);
                }
                else
                {
                    response.Data = await context.Designation
                        .ProjectToListAsync<DesignationDTO>(mapper.ConfigurationProvider);
                }
                response.Message = response.Data.Count > 0 ? "Data found!" : "No Data found!";
                response.StatusCode = "200";
                response.IsSuccess = true;
                return response;
            }
            catch(ValidationException exception)
            {
                response.Errors = exception.Errors.Select(err => err.ErrorMessage).ToList();
                response.Message = "";
                response.StatusCode = "400";
                response.IsSuccess = false;
                return response;
                //return Result.Failure(exception.Errors.Select(err => err.ErrorMessage).ToList(), "");
            }
            catch(Exception ex)
            {
                response.Errors.Add(ex.Message);
                response.Message = "There is some issue with the data!";
                response.StatusCode = "500";
                response.IsSuccess = false;
                return response;
                //return Result.Failure(Array.Empty<string>(),"Issue found in data.");
            }
        }
    }
}
