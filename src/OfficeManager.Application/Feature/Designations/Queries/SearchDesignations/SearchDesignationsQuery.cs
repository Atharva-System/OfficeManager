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
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public SearchDesignationsQueryHandler(IApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<List<DesignationDTO>>> Handle(SearchDesignationsQuery request, CancellationToken cancellationToken)
        {
            Response<List<DesignationDTO>> response = new Response<List<DesignationDTO>>();
            try
            {
                if(!string.IsNullOrEmpty(request.search))
                {
                    response.Data = await _context.Designation.Where(d => d.Name.Contains(request.search))
                        .ProjectToListAsync<DesignationDTO>(_mapper.ConfigurationProvider);
                }
                else
                {
                    response.Data = await _context.Designation
                        .ProjectToListAsync<DesignationDTO>(_mapper.ConfigurationProvider);
                }
                response._Message = response.Data.Count > 0 ? "Data found!" : "No Data found!";
                response._StatusCode = "200";
                response._IsSuccess = true;
                return response;
            }
            catch(ValidationException exception)
            {
                response._Errors = exception.Errors.Select(err => err.ErrorMessage).ToList();
                response._Message = "";
                response._StatusCode = "400";
                response._IsSuccess = false;
                return response;
                //return Result.Failure(exception.Errors.Select(err => err.ErrorMessage).ToList(), "");
            }
            catch(Exception ex)
            {
                response._Errors.Add(ex.Message);
                response._Message = "There is some issue with the data!";
                response._StatusCode = "500";
                response._IsSuccess = false;
                return response;
                //return Result.Failure(Array.Empty<string>(),"Issue found in data.");
            }
        }
    }
}
