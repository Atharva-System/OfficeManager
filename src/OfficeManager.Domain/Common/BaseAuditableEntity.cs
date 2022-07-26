﻿namespace OfficeManager.Domain.Common
{
    public abstract class BaseAuditableEntity : BaseEntity
    {
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public int ModifiedBy { get; set; }
        //public bool IsDeleted { get; set; } = false;
        //public int DeletedBy { get; set; } = 0;
        //public DateTime? DeletedOn { get; set; }

    }
}
