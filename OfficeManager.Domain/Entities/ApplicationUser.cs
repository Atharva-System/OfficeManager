using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeManager.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid DesignationId { get; set; }
        public Guid ProfileId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual DepartmentMaster Department { get; set; }
        [ForeignKey("DesignationId")]
        public virtual DesignationMaster Designation { get; set; }
        [ForeignKey("ProfileId")]
        public virtual UserProfile Profile { get; set; }
    }
}
