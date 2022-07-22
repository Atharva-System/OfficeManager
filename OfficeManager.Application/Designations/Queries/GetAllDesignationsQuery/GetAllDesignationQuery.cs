using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Application.Designations.Queries.GetAllDesignationsQuery
{
    public record GetAllDesignationQuery(string? search) : IRequest<List<DesignationDto>>;

    public class GetAllDesignationQueryHandler : IRequestHandler<GetAllDesignationQuery,List<DesignationDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllDesignationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DesignationDto>> Handle(GetAllDesignationQuery request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.search))
            {
                return await _context.DesignationMasters
                    .OrderBy(d => d.Name)
                    .ProjectTo<DesignationDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            }

            return await _context.DesignationMasters
                .Where(d => d.Name.Contains(request.search))
                .OrderBy(d => d.Name)
                .ProjectTo<DesignationDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
