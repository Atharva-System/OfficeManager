using OfficeManager.Application.Wrappers.Abstract;
using JsonConstructorAttribute = Newtonsoft.Json.JsonConstructorAttribute;

namespace OfficeManager.Application.Wrappers.Concrete
{
    public class DataResponse<T> : IDataResponse<T>
    {
        public bool Success { get; } = true;
        public T Data { get; set; }

        public string StatusCode { get; }
        public string Message { get; set; }

        [JsonConstructor]
        public DataResponse(T data, string statuscode)
        {
            Data = data;
            StatusCode = statuscode;
        }

        public DataResponse(T data, string statuscode, string message)
        {
            Data = data;
            StatusCode = statuscode;
            Message = message;
        }
    }
}