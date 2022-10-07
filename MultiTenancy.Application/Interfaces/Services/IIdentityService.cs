using MultiTenancy.Application.DTOs.Request;
using MultiTenancy.Application.DTOs.Response;

namespace MultiTenancy.Application.Interfaces.Services;

public interface IIdentityService
{
    Task<RegisterUserResponse> RegisterUser(UserRegisterRequest userCad);
    Task<UserLoginResponse> Login(UserLoginRequest usuarioLogin);
}