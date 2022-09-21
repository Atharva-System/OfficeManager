using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.Skills.Queries
{
    public record GetAllSkillRates : IRequest<IResponse>;

    public class GetAllSkillRatesQueryHandler : IRequestHandler<GetAllSkillRates, IResponse>
    {
        private readonly IApplicationDbContext Context;

        public GetAllSkillRatesQueryHandler(IApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<IResponse> Handle(GetAllSkillRates request, CancellationToken cancellationToken)
        {
            return new DataResponse<List<SkillRate>>(await Context.SkillRate.Where(sk => sk.IsActive == true).ToListAsync(cancellationToken), StatusCodes.Accepted, Messages.DataFound);
        }
    }
}
