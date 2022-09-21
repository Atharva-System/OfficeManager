using FluentValidation.Results;

namespace OfficeManager.Application.Common.Exceptions
{

    [Serializable]
    public class ValidationException : Exception
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ValidationException()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base("One more validation failure have occured.")
        { }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            :this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}
