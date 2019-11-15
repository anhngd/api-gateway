using System.Linq;
using System.Security.Claims;

namespace IdentityService.Configurations
{
    /// <summary>
	/// 
	/// </summary>
	internal static class Helpers
    {
        public static string GetUserId(ClaimsPrincipal user)
        {
            return user.Claims
                ?.Where(c => c.Type == "sub")
                .Select(c => c.Value)
                .FirstOrDefault();
        }
    }
}
