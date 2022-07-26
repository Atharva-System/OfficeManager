﻿using MediatR;
using OfficeManager.Application.Common.Exceptions;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Common.Models;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Application.Feature.Employees.Commands
{
    public record AddEmployee : IRequest<IResponse>
    {
        public int employeeId { get; init; }
        public int employeeNo { get; init; }
        public string employeeName { get; init; } = string.Empty;
        public string email { get; init; } = string.Empty;
        public int roleId { get; init; }
        public int departmentId { get; init; }
        public int designationId { get; init; }
        public DateTime dateOfBirth { get; init; }
        public DateTime dateOfJoining { get; init; }
        public List<EmployeeSkill> skills { get; init; } = new List<EmployeeSkill>();
    }

    public class AddEmployeeCommandHandler : IRequestHandler<AddEmployee, IResponse>
    {
        private readonly IApplicationDbContext Context;

        public AddEmployeeCommandHandler(IApplicationDbContext context)
        {
            Context = Context;
        }

        public async Task<IResponse> Handle(AddEmployee request, CancellationToken cancellationToken)
        {
            try
            {
                Context.BeginTransaction();

                Employee employee = new Employee
                {
                    EmployeeNo = request.employeeNo,
                    EmployeeName = request.employeeName,
                    Email = request.email,
                    DateOfBirth = request.dateOfBirth,
                    DateOfJoining = request.dateOfJoining,
                    DepartmentId = request.departmentId,
                    DesignationId = request.designationId
                };

                Context.Employees.Add(employee);
                await Context.SaveChangesAsync(cancellationToken);

                UserMaster user = new UserMaster
                {
                    EmployeeID = employee.Id,
                    Email = request.email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Atharva@123")
                };

                Context.Users.Add(user);

                foreach (EmployeeSkill skill in request.skills)
                {
                    var existingSkill = Context.EmployeeSkills.FirstOrDefault(empSk => empSk.skillId == skill.skillId && empSk.EmployeeId == request.employeeId);
                    if (existingSkill == null)
                    {
                        skill.EmployeeId = employee.Id;
                        Context.EmployeeSkills.Add(skill);
                    }
                    else
                    {
                        existingSkill.skillId = skill.skillId;
                        existingSkill.levelId = skill.levelId;
                        existingSkill.rateId = skill.rateId;
                    }
                }
                await Context.SaveChangesAsync(cancellationToken);

                UserRoleMapping userRole = new UserRoleMapping
                {
                    UserId = user.Id,
                    RoleId = request.roleId
                };

                Context.UserRoleMapping.Add(userRole);
                await Context.SaveChangesAsync(cancellationToken);

                Context.CommitTransaction();

                return new SuccessResponse(StatusCodes.Accepted, "Employee created successfully");
            }
            catch (ValidationException exception)
            {
                throw exception;
            }
            catch (ForbiddenAccessException exception)
            {
                throw exception;
            }
            catch (NotFoundException exception)
            {
                throw exception;
            }
            catch (Exception ex)
            {
                return new ErrorResponse(StatusCodes.InternalServerError, ex.Message);
            }
        }
    }
}
