using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Skills.Queries.GetAllSkillRates
{
    public record GetAllSkillRatesQuery : IRequest<Response<List<SkillRate>>>;

    public class GetAllSkillRatesQueryHandler : IRequestHandler<GetAllSkillRatesQuery,Response<List<SkillRate>>>
    {
        private readonly IApplicationDbContext context;

        public GetAllSkillRatesQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Response<List<SkillRate>>> Handle(GetAllSkillRatesQuery request, CancellationToken cancellationToken)
        {
            Response<List<SkillRate>> response = new Response<List<SkillRate>>();
            response.Data = new List<SkillRate>();

            response.Data = context.SkillRate.Where(sk => sk.IsActive == true).ToList();
            if (response.Data.Count == 0)
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
