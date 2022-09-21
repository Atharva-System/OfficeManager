using AutoMapper;
using MediatR;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.Interfaces;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.Application.Feature.Designations.Queries
{
    public record GetAllDesignations : IRequest<IResponse>, ICacheable
    {

        public bool BypassCache => false;

        public string CacheKey => CacheKeys.Designations;
    }

    public class GetAllDesignationsHandler : IRequestHandler<GetAllDesignations, IResponse>
    {
        private readonly IApplicationDbContext Context;
        private readonly IMapper Mapper;
        public GetAllDesignationsHandler(IApplicationDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<IResponse> Handle(GetAllDesignations request, CancellationToken cancellationToken)
        {
            DataResponse<List<DesignationDTO>> response = new DataResponse<List<DesignationDTO>>(new List<DesignationDTO>(), StatusCodes.Accepted);
            return new DataResponse<List<DesignationDTO>>(
                await Context.Designation
                    .ProjectToListAsync<DesignationDTO>(Mapper.ConfigurationProvider),
                StatusCodes.Accepted,
                Messages.DataFound
                );
        }
    }
}
