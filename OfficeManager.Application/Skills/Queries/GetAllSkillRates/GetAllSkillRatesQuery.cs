using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Skills.Queries.GetAllSkillRates
{
    public record GetAllSkillRatesQuery : IRequest<Response<List<SkillRate>>>;

    public class GetAllSkillRatesQueryHandler : IRequestHandler<GetAllSkillRatesQuery,Response<List<SkillRate>>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllSkillRatesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<SkillRate>>> Handle(GetAllSkillRatesQuery request, CancellationToken cancellationToken)
        {
            Response<List<SkillRate>> response = new Response<List<SkillRate>>();
            response._Data = new List<SkillRate>();

            response._Data = _context.SkillRate.Where(sk => sk.IsActive == true).ToList();
            if (response._Data.Count == 0)
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
