using OfficeManager.Application.Wrappers.Abstract;

namespace OfficeManager.Application.Wrappers.Concrete
{
    public class PagedDataResponse<T> : IPagedDataResponse<T>
    {
        public bool Success { get; } = true;
        public int TotalItems { get; }

        public T Data { get; }

        public string StatusCode { get; }

        public PagedDataResponse(T data, string statuscode, int totalitems)
        {
            Data = data;
            StatusCode = statuscode;
            TotalItems = totalitems;
        }
    }
}
