namespace OfficeManager.Application.Common.Exceptions
{
    [Serializable]
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(): base() {}
    }
}
