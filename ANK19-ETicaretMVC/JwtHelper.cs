using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ANK19_ETicaretMVC
{

    public class JwtHelper
    {
        public static string GetRoleFromJwt(string jwtToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();

                // Check if the token is valid
                if (!handler.CanReadToken(jwtToken))
                {
                    throw new ArgumentException("Invalid JWT token");
                }

                // Decode the JWT token
                var jwt = handler.ReadJwtToken(jwtToken);

                // Get the "role" claim
                var roleClaim = jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);

                // Return the role value or null if not found
                return roleClaim?.Value;
            }
            catch (Exception ex)
            {
                // Handle errors (e.g., invalid token format)
                Console.WriteLine($"Error decoding JWT token: {ex.Message}");
                return null;
            }
        }
    }

}
