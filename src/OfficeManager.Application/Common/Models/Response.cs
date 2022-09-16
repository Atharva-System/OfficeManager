namespace OfficeManager.Application.Common.Models
{
    public class Response<T>
    {
        private string _Message;
        public T _Data;
        private List<string> _Errors;
        private string _StatusCode;
        private bool _IsSuccess;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Response()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _Message = "";
            _Errors = new List<string>();
            _StatusCode = StausCodes.Accepted;
            _IsSuccess = true;
        }

        public string Message
        {
            get => this._Message;
            set => this._Message = value;
        }

        public T Data
        {
            get { return this._Data; }
            set { this._Data = value; }
        }

        public string StatusCode
        {
            get { return this._StatusCode;  }
            set { this._StatusCode = value; }
        }

        public List<string> Errors
        {
            get { return this._Errors; }
            set { this._Errors = value; }
        }

        public bool IsSuccess
        {
            get => this._IsSuccess;
            set { this._IsSuccess = value; }
        }

    }
}
