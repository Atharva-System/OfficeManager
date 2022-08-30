using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Skills.Queries.GetAllSkillLevels
{
    public record GetAllSkillLevelsQuery : IRequest<Response<List<SkillLevel>>>;

    public class GetAllSkillLevelQueryHandler : IRequestHandler<GetAllSkillLevelsQuery, Response<List<SkillLevel>>>
    {
        private readonly IApplicationDbContext context;
        public GetAllSkillLevelQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Response<List<SkillLevel>>> Handle(GetAllSkillLevelsQuery request, CancellationToken cancellationToken)
        {
            Response<List<SkillLevel>> response = new Response<List<SkillLevel>>();
            response.Data = new List<SkillLevel>();

            response.Data = context.SkillLevel.Where(sk => sk.IsActive == true).ToList();
            if(response.Data.Count == 0)
            {
                response.Errors.Add("No records found");
                response.StatusCode = "404";
                response.IsSuccess = false;
            }
            else
            {
                response.Message = "All records found";
                response.StatusCode = "200";
                response.IsSuccess = true;
            }
            return response;
        }
    }
}
