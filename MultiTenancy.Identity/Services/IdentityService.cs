using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MultiTenancy.Application.DTOs.Request;
using MultiTenancy.Application.DTOs.Response;
using MultiTenancy.Application.Interfaces.Services;
using MultiTenancy.Identity.Configurations;

namespace MultiTenancy.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtOptions _jwtOptions;

        public IdentityService(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IOptions<JwtOptions> jwtOptions
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<RegisterUserResponse> RegisterUser(UserRegisterRequest userCad)
        {
            var identityUser = new IdentityUser
            {
                UserName = userCad.Email,
                Email = userCad.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(identityUser, password : userCad.Password);

            if (result.Succeeded) 
                await _userManager.SetLockoutEnabledAsync(identityUser, enabled: false);

            var RegisterUserResponse = new RegisterUserResponse(result.Succeeded);

            if (!result.Succeeded && result.Errors.Count() > 0)
                RegisterUserResponse.AddErrors(result.Errors.Select(r => r.Description));

            return RegisterUserResponse;
        }

        public async Task<UserLoginResponse> Login(UserLoginRequest userLogin)
        {
            var result = await _signInManager.PasswordSignInAsync(
                userName: userLogin.Email, 
                password: userLogin.Password, 
                isPersistent: false, 
                lockoutOnFailure: true
            );

            if (result.Succeeded)
                return await GenerateCredentials(userLogin.Email);

            var UserLoginResponse = new UserLoginResponse();

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    UserLoginResponse.AddError("Essa conta está bloqueada");
                else if (result.IsNotAllowed)
                    UserLoginResponse.AddError("Essa conta não tem permissão para fazer login");
                else if (result.RequiresTwoFactor)
                    UserLoginResponse.AddError("É necessário confirmar o login no seu segundo fator de autenticação");
                else
                    UserLoginResponse.AddError("Usuário ou senha estão incorretos");
            }

            return UserLoginResponse;
        }

        private async Task<UserLoginResponse> GenerateCredentials(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var accessTokenClaims = await GetClaims(user, addClaimsUser: true);

            var dataExpirationAccessToken = DateTime.Now.AddSeconds(_jwtOptions.AccessTokenExpiration);

            var accessToken = GenerateToken(accessTokenClaims, dataExpirationAccessToken);

            return new UserLoginResponse
            (
                success: true,
                accessToken: accessToken,
                refreshToken: null
            );
        }

        private string GenerateToken(IEnumerable<Claim> claims, DateTime dataExpiration)
        {
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: dataExpiration,
                signingCredentials: _jwtOptions.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private async Task<IList<Claim>> GetClaims(IdentityUser user, bool addClaimsUser)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));

            if (addClaimsUser)
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                
                var roles = await _userManager.GetRolesAsync(user);

                claims.AddRange(userClaims);

                foreach (var role in roles)
                    claims.Add(new Claim("role", role));
            }

            return claims;
        }
    }
}