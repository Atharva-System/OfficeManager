using OfficeManager.Domain.Common;
using OfficeManager.Domain.Entities;

namespace OfficeManager.Domain.Events
{
    public class EmployeeRegisteredEvent : BaseEvent
    {
        public EmployeeRegisteredEvent(UserMaster user)
        {
            User = user;
        }
        public UserMaster User { get; }
    }
}
