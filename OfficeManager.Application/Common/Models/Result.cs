namespace OfficeManager.Application.Common.Models
{
    public class Result
    {
        public Result(bool succeeded, IEnumerable<string> errors, string message)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
            Message = message;
        }
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }
        public static Result Success(string message)
        {
            return new Result(true,Array.Empty<string>(),message);
        }
        public static Result Failure(IEnumerable<string> errors,string message)
        {
            return new Result(false,errors,message);
        }
    }
}
