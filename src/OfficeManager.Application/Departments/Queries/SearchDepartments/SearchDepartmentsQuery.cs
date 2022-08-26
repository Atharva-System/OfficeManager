using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;

namespace OfficeManager.Application.Departments.Queries.SearchDepartments
{
    public record SearchDepartmentsQuery(string search) : IRequest<Response<List<DepartmentDto>>>;

    public class SearchDepartmentQueryHandler : IRequestHandler<SearchDepartmentsQuery, Response<List<DepartmentDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public SearchDepartmentQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<List<DepartmentDto>>> Handle(SearchDepartmentsQuery request,CancellationToken cancellationToken)
        {
            Response<List<DepartmentDto>> response = new Response<List<DepartmentDto>>();
            try
            {

                List<DepartmentDto> departments = new List<DepartmentDto>();
                if (!string.IsNullOrEmpty(request.search))
                {
                    departments = await _context.DepartMent
                        .AsNoTracking()
                        .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)
                        .Where(x => x.Name.Contains(request.search)).ToListAsync(cancellationToken);
                }
                else
                {
                    departments = await _context.DepartMent
                        .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)
                        .ToListAsync();
                }
                response.Data = departments;
                response._StatusCode = "200";
                response._IsSuccess = true;
                response._Message = departments.Count > 0 ? "Data found!" : "No Data found!";
                
                //return Result.Success("Department found",departments);
                return response;
            }
            catch (Exception ex)
            {
                //return Result.Failure(Array.Empty<string>(),"Data has some issue please check");
                response.Data = null;
                response._Message = "There is some issue with the data!";
                response._Errors.Add(ex.Message);
                response._IsSuccess = false;
                response._StatusCode = "500";
                return response;
            }

            
        }
    }
}
