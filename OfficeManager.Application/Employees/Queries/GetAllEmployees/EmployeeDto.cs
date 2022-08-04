using AutoMapper;
using OfficeManager.Application.Common.Mappings;
using OfficeManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeManager.Application.Employees.Queries.GetAllEmployees
{
    public class EmployeeDto : IMapFrom<Employee>
    {
        public string Id { get; set; }
        public int EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Employee, EmployeeDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(x => x.EmployeeName, opt => opt.MapFrom(s => s.EmployeeName))
                .ForMember(x => x.Email, opt => opt.MapFrom(s => s.Email))
                .ForMember(x => x.EmployeeNo, opt => opt.MapFrom(s => s.EmployeeNo));
        }
    }
}
