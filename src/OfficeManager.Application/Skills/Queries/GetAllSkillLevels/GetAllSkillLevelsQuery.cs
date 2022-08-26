using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Skills.Queries.GetAllSkillLevels
{
    public record GetAllSkillLevelsQuery : IRequest<Response<List<SkillLevel>>>;

    public class GetAllSkillLevelQueryHandler : IRequestHandler<GetAllSkillLevelsQuery, Response<List<SkillLevel>>>
    {
        private readonly IApplicationDbContext _context;
        public GetAllSkillLevelQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<SkillLevel>>> Handle(GetAllSkillLevelsQuery request, CancellationToken cancellationToken)
        {
            Response<List<SkillLevel>> response = new Response<List<SkillLevel>>();
            response._Data = new List<SkillLevel>();

            response._Data = _context.SkillLevel.Where(sk => sk.IsActive == true).ToList();
            if(response._Data.Count == 0)
            {
                response._Errors.Add("No records found");
                response._StatusCode = "404";
                response._IsSuccess = false;
            }
            else
            {
                response._Message = "All records found";
                response._StatusCode = "200";
                response._IsSuccess = true;
            }
            return response;
        }
    }
}
