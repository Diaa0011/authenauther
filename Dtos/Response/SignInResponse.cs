namespace authenAutherApp.Dtos.Response
{
    public class SignInResponse
    {
        public string Token { get; set; }
        public bool isSuccess { get; set; }
        public string Message { get; set; }
    }
}