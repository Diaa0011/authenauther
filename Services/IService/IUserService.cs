using authenAutherApp.Dtos.Request;
using authenAutherApp.Dtos.Response;
using authenAutherApp.Response;

namespace authenAutherApp.Services.IService
{
    public interface IUserService
    {
        Task<SignInResponse> SiginAsync(SignInRequest request);
        Task<SignUpResponse> SignupAsync(SignUpRequest request);
        Task<ResponseModel<List<UserReponse>>> GetAllUsers();
    }
}