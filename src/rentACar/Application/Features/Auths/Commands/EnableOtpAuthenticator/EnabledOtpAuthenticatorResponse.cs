namespace Application.Features.Auths.Commands.EnableOtpAuthenticator;
public class EnabledOtpAuthenticatorResponse
{
    public string SecretKey { get; set; }

    public EnabledOtpAuthenticatorResponse()
    {
        SecretKey = string.Empty;
    }

    public EnabledOtpAuthenticatorResponse(string secretKey)
    {
        SecretKey = secretKey;
    }
}