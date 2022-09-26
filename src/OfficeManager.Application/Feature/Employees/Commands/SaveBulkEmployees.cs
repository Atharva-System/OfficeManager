using Dapper;
using MediatR;
using OfficeManager.Application.Common.Exceptions;
using OfficeManager.Application.Common.Interfaces;
using OfficeManager.Application.Dtos;
using OfficeManager.Application.Wrappers.Abstract;
using OfficeManager.Application.Wrappers.Concrete;
using OfficeManager.Domain.Entities;
using System.Data;
using System.Data.SqlClient;

namespace OfficeManager.Application.Feature.Employees.Commands
{
    public record SaveBulkEmployees : IRequest<IResponse>
    {
        public List<BulkImportEmployeeDTO> NewEmployees { get; set; } = new List<BulkImportEmployeeDTO>();
        public List<BulkImportEmployeeDTO> UpdateEmployees { get; set; } = new List<BulkImportEmployeeDTO>();
    }

    public class SaveBulkEmployeesCommandHandler : IRequestHandler<SaveBulkEmployees, IResponse>
    {
        private readonly IApplicationDbContext Context;

        public SaveBulkEmployeesCommandHandler(IApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<IResponse> Handle(SaveBulkEmployees request, CancellationToken cancellationToken)
        {
            try
            {
                #region Adding new employees
                Context.BeginTransaction();

                request.NewEmployees.ForEach(emp =>
                {
                    Employee employee = new Employee()
                    {
                        EmployeeNo = emp.EmployeeNo,
                        EmployeeName = emp.EmployeeName,
                        DepartmentId = emp.DepartmentId,
                        DesignationId = emp.DesignationId,
                        Email = emp.Email,
                        DateOfBirth = emp.DateOfBirth,
                        DateOfJoining = emp.DateOfJoining
                    };

                    Context.Employees.Add(employee);
                    Context.SaveChangesAsync(cancellationToken);

                    UserMaster user = new UserMaster
                    {
                        EmployeeID = employee.Id,
                        Email = employee.Email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Atharva@123")
                    };

                    Context.Users.Add(user);
                    Context.SaveChangesAsync(cancellationToken);

                    UserRoleMapping userRole = new UserRoleMapping
                    {
                        UserId = user.Id,
                        RoleId = emp.RoleId
                    };

                    Context.UserRoleMapping.Add(userRole);
                    Context.SaveChangesAsync(cancellationToken);
                });

                Context.CommitTransaction();
                #endregion

                #region Updating existing employees
                Context.BeginTransaction();

                request.UpdateEmployees.ForEach(emp =>
                {
                    Employee employee = Context.Employees.Find(emp.Id);

                    employee.EmployeeName = emp.EmployeeName;
                    employee.Email = emp.Email;
                    employee.DepartmentId = emp.DepartmentId;
                    employee.DesignationId = emp.DesignationId;
                    employee.DateOfBirth = emp.DateOfBirth;
                    employee.DateOfJoining = emp.DateOfJoining;

                    UserMaster user = Context.Users.FirstOrDefault(user => user.EmployeeID == emp.Id);

                    user.Email = emp.Email;
                });

                await Context.SaveChangesAsync(cancellationToken);

                Context.CommitTransaction();
                #endregion

                return new SuccessResponse(StatusCodes.Accepted, Messages.Success);
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