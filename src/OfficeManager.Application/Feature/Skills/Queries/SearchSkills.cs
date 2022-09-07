﻿using AutoMapper;
using FluentValidation;
using MediatR;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Application.Common.Models;
using System.ComponentModel.DataAnnotations;
using AutoMapper.QueryableExtensions;
using ValidationException = FluentValidation.ValidationException;

namespace OfficeManager.Application.Feature.Skills.Queries
{
    public record SearchSkills : IRequest<Response<PaginatedList<SkillDTO>>>
    {
        public string Search { get; init; } = string.Empty;
        public int Page_No { get; init; } = 1;
        public int Page_Size { get; set; } = 10;
    }

    public class SearchSkillQueryHandler : IRequestHandler<SearchSkills, Response<PaginatedList<SkillDTO>>>
    {
        private readonly IApplicationDbContext Context;
        private readonly IMapper Mapper;
        public SearchSkillQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<Response<PaginatedList<SkillDTO>>> Handle(SearchSkills request, CancellationToken cancellationToken)
        {
            Response<PaginatedList<SkillDTO>> response = new Response<PaginatedList<SkillDTO>>();
            try
            {
                response.Data = new PaginatedList<SkillDTO>(new List<SkillDTO>(), 0, request.Page_No, request.Page_Size);
                if (string.IsNullOrEmpty(request.Search))
                    response.Data = await Context.Skill
                        .OrderBy(x => x.Name)
                        .ProjectTo<SkillDTO>(Mapper.ConfigurationProvider)
                        .PaginatedListAsync(request.Page_No, request.Page_Size);
                else
                    response.Data = await Context.Skill
                        .Where(x => x.Name.Contains(request.Search))
                        .OrderBy(x => x.Name)
                        .ProjectTo<SkillDTO>(Mapper.ConfigurationProvider)
                        .PaginatedListAsync(request.Page_No, request.Page_Size);

                response.Message = response.Data.Items.Count > 0 ? Messages.DataFound : Messages.NoDataFound;
                response.StatusCode = StausCodes.Accepted;
                response.IsSuccess = true;
                return response;
            }
            catch (ValidationException exception)
            {
                response.Errors = exception.Errors.Select(err => err.ErrorMessage)
                    .ToList();
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
