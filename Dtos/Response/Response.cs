namespace authenAutherApp.Dtos.Response
{
    public interface IResponseModel
    {
        string Message { get; set; }
        bool Success { get; set; }
    }
    public class ResponseModel<T> : IResponseModel
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public T? Result { get; set; }

    }
}
