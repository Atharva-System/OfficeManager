﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;

namespace OfficeManager.Application.Feature.Designations.Queries
{
    public record SearchDesignations : IRequest<IResponse>
    {
        public string Search { get; set; } = string.Empty;
        public int Page_No { get; set; } = 1;
        public int Page_Size { get; set; } = 10;
        public string SortingColumn { get; set; } = "Name";
        public string SortingDirection { get; set; } = "ASC";
    }
    public class SearchDesginatoinsHandler : IRequestHandler<SearchDesignations, IResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SearchDesginatoinsHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IResponse> Handle(SearchDesignations request, CancellationToken cancellationToken)
        {
            var designations = _context.Designation.AsQueryable().OrderBy(request.SortingColumn, (request.SortingDirection.ToLower() == "desc" ? false : true));
            PaginatedList<DesignationDTO> pagedDesignation = new PaginatedList<DesignationDTO>(new List<DesignationDTO>(),0,request.Page_No,request.Page_Size);
            if (string.IsNullOrEmpty(request.Search))
            {
                pagedDesignation = await designations
                    .ProjectTo<DesignationDTO>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync<DesignationDTO>(request.Page_No, request.Page_Size);
            }
            else
            {
                pagedDesignation = await designations
                    .AsNoTracking()
                    .Where(d => d.Name.Contains(request.Search))
                    .ProjectTo<DesignationDTO>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync<DesignationDTO>(request.Page_No, request.Page_Size);
            }
            if(pagedDesignation.Items.Count == 0)
            {
                return new ErrorResponse(StatusCodes.BadRequest, Messages.NoDataFound);
            }
            return new DataResponse<PaginatedList<DesignationDTO>>(pagedDesignation, StatusCodes.Accepted, Messages.DataFound);
        }
    }
}
