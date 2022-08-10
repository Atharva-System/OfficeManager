namespace OfficeManager.Application.Common.Models
{
    public class Response<T>
    {
        private string Message;
        public T Data;
        private List<string> Errors;
        private string StatusCode;
        private bool IsSuccess;

        public string _Message
        {
            get => this.Message;
            set => this.Message = value;
        }

        public T _Data
        {
            get { return this.Data; }
            set { this.Data = value; }
        }

        public string _StatusCode
        {
            get { return this.StatusCode;  }
            set { this.StatusCode = value; }
        }

        public List<string> _Errors
        {
            get { return this.Errors; }
            set { this._Errors = value; }
        }

        public bool _IsSuccess
        {
            get => this.IsSuccess;
            set { this.IsSuccess = value; }
        }
    }
}
