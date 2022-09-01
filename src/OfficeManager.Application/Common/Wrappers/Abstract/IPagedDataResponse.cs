namespace OfficeManager.Application.Wrappers.Abstract
{
    interface IPagedDataResponse<T> : IResponse
    {
        int TotalItems { get; }
        T Data { get; }
    }
}
