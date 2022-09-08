using AutoMapper;
using FluentValidation;
using MediatR;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Interfaces;

namespace OfficeManager.Application.Feature.Designations.Queries
{
    public record GetAllDesignations : IRequest<Response<List<DesignationDTO>>>, ICacheable
    {

        public bool BypassCache => false;

        public string CacheKey => CacheKeys.Designations;
    }

    public class GetAllDesignationsHandler : IRequestHandler<GetAllDesignations, Response<List<DesignationDTO>>>
    {
        private readonly IApplicationDbContext Context;
        private readonly IMapper Mapper;
        public GetAllDesignationsHandler(IApplicationDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<Response<List<DesignationDTO>>> Handle(GetAllDesignations request, CancellationToken cancellationToken)
        {
            Response<List<DesignationDTO>> response = new Response<List<DesignationDTO>>();
            try
            {
                response.Data = await Context.Designation
                    .ProjectToListAsync<DesignationDTO>(Mapper.ConfigurationProvider);
                response.Message = response.Data.Count > 0 ? Messages.DataFound : Messages.NoDataFound;
                response.StatusCode = StausCodes.Accepted;
                response.IsSuccess = true;
                return response;
            }
            catch (ValidationException exception)
            {
                response.Errors = exception.Errors.Select(err => err.ErrorMessage).ToList();
                response.Message = "";
                response.StatusCode = StausCodes.BadRequest;
                response.IsSuccess = false;
                return response;
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
                response.Message = Messages.IssueWithData;
                response.StatusCode = StausCodes.InternalServerError;
                response.IsSuccess = false;
                return response;
            }
        }
    }
}
