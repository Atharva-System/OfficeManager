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
