using OfficeManager.Application.Wrappers.Abstract;

namespace OfficeManager.Application.Wrappers.Concrete
{
    public class Response : IResponse
    {
        public bool Success { get; }

        public string StatusCode { get; }

        public Response(bool success, string statuscode)
        {
            Success = success;
            StatusCode = statuscode;
        }

        public Response(bool success)
        {
            Success = success;
        }
    }
}
