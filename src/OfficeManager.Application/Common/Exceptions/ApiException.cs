using System.Runtime.Serialization;

namespace OfficeManager.Application.Common.Exceptions
{
    [Serializable]
    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public List<string> Errors { get; private set; } = new List<string>();

        public ApiException(int statusCode, List<string> errors)
        {
            StatusCode = statusCode;
            Errors = errors;
        }

        public ApiException(int statusCode, string error)
        {
            StatusCode = statusCode;
            Errors.Add(error);
        }
        public ApiException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
