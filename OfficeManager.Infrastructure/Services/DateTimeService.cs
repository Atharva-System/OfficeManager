using OfficeManager.Application.Common.Interfaces;

namespace OfficeManager.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
