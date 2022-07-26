namespace OfficeManager.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; set; }
        string GetUserId { get; }
    }
}
