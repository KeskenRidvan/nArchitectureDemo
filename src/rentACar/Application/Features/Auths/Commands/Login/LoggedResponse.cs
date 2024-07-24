using Core.Security.Enums;
using Core.Security.JWT;

namespace Application.Features.Auths.Commands.Login;
public class LoggedResponse
{
    public AccessToken? AccessToken { get; set; }
    public Domain.Entities.RefreshToken? RefreshToken { get; set; }
    public AuthenticatorType? RequiredAuthenticatorType { get; set; }

    public LoggedHttpResponse ToHttpResponse()
    {
        return new()
        {
            AccessToken = AccessToken,
            RequiredAuthenticatorType = RequiredAuthenticatorType
        };
    }

    public class LoggedHttpResponse
    {
        public AccessToken? AccessToken { get; set; }
        public AuthenticatorType? RequiredAuthenticatorType { get; set; }
    }
}