using Microsoft.IdentityModel.Tokens;

namespace Core.Security.Encryptions;

public static class SigningCredentialsHelper
{
    public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey)
    {
        return new(securityKey, SecurityAlgorithms.HmacSha512Signature);
    }
}