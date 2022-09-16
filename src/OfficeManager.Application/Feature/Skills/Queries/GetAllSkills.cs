using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Interfaces;

namespace OfficeManager.Application.Feature.Skills.Queries
{
    public record GetAllSkills : IRequest<Response<List<SkillDTO>>>, ICacheable
    {
        public bool BypassCache => false;

        public string CacheKey => CacheKeys.Departments;
    }

    public class GetAllSkillsHandler : IRequestHandler<GetAllSkills, Response<List<SkillDTO>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllSkillsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<List<SkillDTO>>> Handle(GetAllSkills request, CancellationToken cancellationToken)
        {
            Response<List<SkillDTO>> response = new Response<List<SkillDTO>>();
            try
            {
                response._Data = new List<SkillDTO>();
                response._Data = await _context.Skill
                    .AsNoTracking()
                    .ProjectTo<SkillDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
                response.StatusCode = StausCodes.Accepted;
                response.IsSuccess = true;
                response.Message = response._Data.Count > 0 ? Messages.DataFound : Messages.NoDataFound;

                return response;
            }
            catch (Exception ex)
            {
                response.Data = new List<SkillDTO>();
                response.Message = Messages.IssueWithData;
                response.Errors.Add(ex.Message);
                response.IsSuccess = false;
                response.StatusCode = StausCodes.InternalServerError;
                return response;
            }
        }
    }
}
