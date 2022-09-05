using OfficeManager.Application.Wrappers.Abstract;

namespace OfficeManager.Application.Wrappers.Concrete
{
    public class SuccessResponse : ISuccessResponse
    {
        public bool Success { get; } = true;
        public string Message { get; }
        public int StatusCode { get; }


        public SuccessResponse()
        {

        }

        public SuccessResponse(int statuscode, string message)
        {
            StatusCode = statuscode;
            Message = message;
        }
    }
}
