using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.Skills.Queries
{
    public record GetAllSkillLevels : IRequest<Response<List<SkillLevel>>>;

    public class GetAllSkillLevelQueryHandler : IRequestHandler<GetAllSkillLevels, Response<List<SkillLevel>>>
    {
        private readonly IApplicationDbContext context;
        public GetAllSkillLevelQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Response<List<SkillLevel>>> Handle(GetAllSkillLevels request, CancellationToken cancellationToken)
        {
            Response<List<SkillLevel>> response = new()
            {
                Data = new List<SkillLevel>()
            };

            response.Data = await context.SkillLevel.Where(sk => sk.IsActive == true).ToListAsync();
            
            if (response.Data.Count == 0)
            {
                response.Errors.Add(Messages.NoDataFound);
                response.StatusCode = StausCodes.NotFound;
                response.IsSuccess = false;
            }
            else
            {
                response.Message = Messages.DataFound;
                response.StatusCode = StausCodes.Accepted;
                response.IsSuccess = true;
            }
            return response;
        }
    }
}
