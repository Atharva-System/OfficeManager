using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.Skills.Queries
{
    public record GetAllSkillLevels : IRequest<IResponse>;

    public class GetAllSkillLevelQueryHandler : IRequestHandler<GetAllSkillLevels, IResponse>
    {
        private readonly IApplicationDbContext Context;
        public GetAllSkillLevelQueryHandler(IApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<IResponse> Handle(GetAllSkillLevels request, CancellationToken cancellationToken)
        {
            return new DataResponse<List<SkillLevel>>(await Context.SkillLevel.Where(sk => sk.IsActive == true).ToListAsync(), StatusCodes.Accepted, Messages.DataFound);
        }
    }
}
