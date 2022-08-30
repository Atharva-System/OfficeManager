using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.Common.Models;

namespace OfficeManager.Application.Feature.Skills.Queries
{
    public record SearchSkills : IRequest<Response<PaginatedList<SkillDTO>>>
    {
        public string Search { get; init; } = string.Empty;
        public int PageNo { get; init; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class SearchSkillQueryHandler : IRequestHandler<SearchSkills, Response<PaginatedList<SkillDTO>>>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;
        public SearchSkillQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Response<PaginatedList<SkillDTO>>> Handle(SearchSkills request, CancellationToken cancellationToken)
        {
            Response<PaginatedList<SkillDTO>> response = new Response<PaginatedList<SkillDTO>>();
            response.Data = new PaginatedList<SkillDTO>(new List<SkillDTO>(), 0, request.PageNo, request.PageSize);
            if (string.IsNullOrEmpty(request.Search))
                response.Data = await context.Skill
                    .OrderBy(x => x.Name)
                    .ProjectTo<SkillDTO>(mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.PageNo, request.PageSize);
            else
                response.Data = await context.Skill
                    .Where(x => x.Name.Contains(request.Search))
                    .OrderBy(x => x.Name)
                    .ProjectTo<SkillDTO>(mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.PageNo, request.PageSize);

            if (response.Data.TotalCount > 0)
            {
                response.Message = Messages.DataFound;
                response.IsSuccess = true;
                response.StatusCode = StausCodes.Accepted;
            }
            else
            {
                response.Message = Messages.NoDataFound;
                response.IsSuccess = true;
                response.StatusCode = StausCodes.NotFound;
            }
            return response;
        }
    }
}
