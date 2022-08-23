using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeManager.Application.Skills.Queries.SearchSkillsQuery
{
    public record SearchSkillsQuery: IRequest<Response<PaginatedList<SkillDto>>>
    {
        public string Search { get; init; }
        public int PageNo { get; init; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class SearchSkillQueryHandler : IRequestHandler<SearchSkillsQuery,Response<PaginatedList<SkillDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public SearchSkillQueryHandler(IApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<PaginatedList<SkillDto>>> Handle(SearchSkillsQuery request, CancellationToken cancellationToken)
        {
            Response<PaginatedList<SkillDto>> response = new Response<PaginatedList<SkillDto>>();
            response._Data = new PaginatedList<SkillDto>(new List<SkillDto>(),0,request.PageNo,request.PageSize);
            if (string.IsNullOrEmpty(request.Search))
                response._Data = await _context.Skill
                    .OrderBy(x => x.Name)
                    .ProjectTo<SkillDto>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.PageNo, request.PageSize);
            else
                response._Data = await _context.Skill
                    .Where(x => x.Name.Contains(request.Search))
                    .OrderBy(x => x.Name)
                    .ProjectTo<SkillDto>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.PageNo, request.PageSize);
            if(response._Data.TotalCount > 0)
            {
                response._Message = "Records found!";
                response._IsSuccess = true;
                response._StatusCode = "200";
            }
            else
            {
                response._Message = "No Records found!";
                response._IsSuccess = true;
                response._StatusCode = "200";
            }
            return response;
        }
    }
}
