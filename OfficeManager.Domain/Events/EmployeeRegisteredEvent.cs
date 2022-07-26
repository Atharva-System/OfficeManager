namespace OfficeManager.Domain.Events
{
    public class EmployeeRegisteredEvent : BaseEvent
    {
        public EmployeeRegisteredEvent(ApplicationUser user)
        {
            User = user;
        }
        public ApplicationUser User { get; }
    }
}
