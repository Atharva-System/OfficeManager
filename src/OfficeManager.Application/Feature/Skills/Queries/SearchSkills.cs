using AutoMapper;
using FluentValidation;
using MediatR;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.Common.Models;
using AutoMapper.QueryableExtensions;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.Application.Feature.Skills.Queries
{
    public record SearchSkills : IRequest<IResponse>
    {
        public string Search { get; init; } = string.Empty;
        public int Page_No { get; init; } = 1;
        public int Page_Size { get; set; } = 10;
        public string SortingColumn { get; set; } = "Name";
        public string SortingDirection { get; set; } = "ASC";
    }

    public class SearchSkillQueryHandler : IRequestHandler<SearchSkills, IResponse>
    {
        private readonly IApplicationDbContext Context;
        private readonly IMapper Mapper;
        public SearchSkillQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<IResponse> Handle(SearchSkills request, CancellationToken cancellationToken)
        {
            var skills = Context.Skill.AsQueryable().OrderBy(request.SortingColumn, (request.SortingDirection.ToLower() == "desc" ? false : true));
            PaginatedList<SkillDTO> data = new PaginatedList<SkillDTO>(new List<SkillDTO>(), 0, request.Page_No, request.Page_Size);

            if (string.IsNullOrEmpty(request.Search))
                data = await skills
                    .ProjectTo<SkillDTO>(Mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.Page_No, request.Page_Size);
            else
                data = await skills
                    .Where(x => x.Name.Contains(request.Search))
                    .ProjectTo<SkillDTO>(Mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.Page_No, request.Page_Size);
            if(data.Items.Count == 0)
            {
                return new ErrorResponse(StatusCodes.BadRequest,Messages.NoDataFound);
            }
            return new DataResponse<PaginatedList<SkillDTO>>(data, StatusCodes.Accepted, Messages.DataFound);
        }
    }
}
