namespace OfficeManager.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        Guid UserId { get; set; }
        Guid GetUserId { get; }
    }
}
