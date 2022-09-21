using OfficeManager.Application.Wrappers.Abstract;

namespace OfficeManager.Application.Wrappers.Concrete
{
    public class SuccessResponse : ISuccessResponse
    {
        public bool Success { get; } = true;
        public string Message { get; }
        public string StatusCode { get; }


        public SuccessResponse()
        {

        }

        public SuccessResponse(string statuscode, string message)
        {
            StatusCode = statuscode;
            Message = message;
        }
    }
}
