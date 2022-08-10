﻿namespace OfficeManager.Application.Common.Models
{
    public class Result
    {
        public Result(bool succeeded, IEnumerable<string> errors, string message,object data)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
            Message = message;
            Data = data;
        }
        public object Data { get; set; }
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }
        public static Result Success(string message,object Data)
        {
            return new Result(true,Array.Empty<string>(),message,Data);
        }
        public static Result Failure(IEnumerable<string> errors,string message)
        {
            return new Result(false,errors,message,null);
        }
    }
}
