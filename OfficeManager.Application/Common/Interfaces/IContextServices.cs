namespace OfficeManager.Application.Common.Interfaces
{
    public interface IContextServices
    {
        Task<string> GetConnectionString();
    }
}
