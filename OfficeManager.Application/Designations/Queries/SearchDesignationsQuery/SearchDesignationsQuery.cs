﻿using AutoMapper;
using FluentValidation;
using MediatR;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.Common.Models;

namespace OfficeManager.Application.Designations.Queries.SearchDesignationsQuery
{
    public record SearchDesignationsQuery(string search):IRequest<Response<List<DesignationDto>>>;

    public class SearchDesignationsQueryHandler : IRequestHandler<SearchDesignationsQuery, Response<List<DesignationDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public SearchDesignationsQueryHandler(IApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<List<DesignationDto>>> Handle(SearchDesignationsQuery request, CancellationToken cancellationToken)
        {
            Response<List<DesignationDto>> response = new Response<List<DesignationDto>>();
            try
            {
                if(!string.IsNullOrEmpty(request.search))
                {
                    response.Data = await _context.Designation.Where(d => d.Name.Contains(request.search))
                        .ProjectToListAsync<DesignationDto>(_mapper.ConfigurationProvider);
                    
                    //return Result.Success("Designations found",await _context.Designation.Where(d => d.Name.Contains(request.search))
                    //    .ProjectToListAsync<DesignationDto>(_mapper.ConfigurationProvider));
                }
                else
                {
                    response.Data = await _context.Designation
                        .ProjectToListAsync<DesignationDto>(_mapper.ConfigurationProvider);
                }
                response._Message = "Designations found";
                response._StatusCode = "200";
                response._IsSuccess = true;
                return response;
                //return Result.Success("Designations found", await _context.Designation
                //        .ProjectToListAsync<DesignationDto>(_mapper.ConfigurationProvider));
            }
            catch(ValidationException exception)
            {
                response._Errors = exception.Errors.Select(err => err.ErrorMessage).ToList();
                response._Message = "";
                response._StatusCode = "400";
                response._IsSuccess = false;
                return response;
                //return Result.Failure(exception.Errors.Select(err => err.ErrorMessage).ToList(), "");
            }
            catch(Exception ex)
            {
                response._Errors.Add(ex.Message);
                response._Message = "There is some issue with the data!";
                response._StatusCode = "500";
                response._IsSuccess = false;
                return response;
                //return Result.Failure(Array.Empty<string>(),"Issue found in data.");
            }
        }
    }
}
