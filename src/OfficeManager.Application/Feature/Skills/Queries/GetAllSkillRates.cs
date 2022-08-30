using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.Skills.Queries
{
    public record GetAllSkillRates : IRequest<Response<List<SkillRate>>>;

    public class GetAllSkillRatesQueryHandler : IRequestHandler<GetAllSkillRates, Response<List<SkillRate>>>
    {
        private readonly IApplicationDbContext context;

        public GetAllSkillRatesQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Response<List<SkillRate>>> Handle(GetAllSkillRates request, CancellationToken cancellationToken)
        {
            Response<List<SkillRate>> response = new()
            {
                Data = new List<SkillRate>()
            };

            response.Data =await context.SkillRate.Where(sk => sk.IsActive == true).ToListAsync();

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
