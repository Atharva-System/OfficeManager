using OfficeManager.Application.Wrappers.Abstract;

namespace OfficeManager.Application.Wrappers.Concrete
{
    public class ErrorResponse : IErrorResponse
    {
        public bool Success { get; } = false;
        public string StatusCode { get; }
        public List<string> Errors { get; private set; } = new List<string>();

        public ErrorResponse(string statuscode, List<string> errors)
        {
            StatusCode = statuscode;
            Errors = errors;
        }

        public ErrorResponse(string statuscode, string error)
        {
            StatusCode = statuscode;
            Errors.Add(error);
        }
    }
}
