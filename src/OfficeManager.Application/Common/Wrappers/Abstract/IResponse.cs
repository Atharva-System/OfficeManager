namespace OfficeManager.Application.Wrappers.Abstract
{
    public interface IResponse
    {
        bool Success { get; }
        int StatusCode { get; }
    }
}
