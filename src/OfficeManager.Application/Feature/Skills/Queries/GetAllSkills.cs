using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Interfaces;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.Application.Feature.Skills.Queries
{
    public record GetAllSkills : IRequest<IResponse>, ICacheable
    {
        public bool BypassCache => false;

        public string CacheKey => CacheKeys.Departments;
    }

    public class GetAllSkillsHandler : IRequestHandler<GetAllSkills, IResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllSkillsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IResponse> Handle(GetAllSkills request, CancellationToken cancellationToken)
        {
            
             return new DataResponse<List<SkillDTO>>(await _context.Skill
                 .AsNoTracking()
                 .ProjectTo<SkillDTO>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken)
                 , StatusCodes.Accepted
                 , Messages.DataFound);
        }
    }
}
