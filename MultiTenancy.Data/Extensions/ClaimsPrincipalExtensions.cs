using System.Security.Claims;

namespace MultiTenancy.Data.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null) throw new ArgumentException(nameof(principal));

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);

            return claim?.Value;
        }
    }
}