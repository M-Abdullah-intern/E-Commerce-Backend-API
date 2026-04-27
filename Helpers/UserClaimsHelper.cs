using System.Security.Claims;

namespace ECommerceAPI.Helpers
{
    public static class UserClaimsHelper
    {
        public static int GetUserId(ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
                throw new UnauthorizedAccessException("Invalid token");

            return int.Parse(claim.Value);
        }

        public static string GetRole(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role)?.Value ?? "";
        }

        public static string GetEmail(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Email)?.Value ?? "";
        }
    }
}