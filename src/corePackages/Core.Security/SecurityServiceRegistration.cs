using Core.Security.EmailAuthenticators;
using Core.Security.JWT;
using Core.Security.OtpAuthenticators;
using Core.Security.OtpAuthenticators.OtpNet;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Security;
public static class SecurityServiceRegistration
{
    public static IServiceCollection AddSecurityServices<TUserId, TOperationClaimId, TRefreshTokenId>(
        this IServiceCollection services,
        TokenOptions tokenOptions
    )
    {
        services.AddScoped<
            ITokenHelper<TUserId, TOperationClaimId, TRefreshTokenId>,
            JwtHelper<TUserId, TOperationClaimId, TRefreshTokenId>
        >(_ => new JwtHelper<TUserId, TOperationClaimId, TRefreshTokenId>(tokenOptions));
        services.AddScoped<IEmailAuthenticatorHelper, EmailAuthenticatorHelper>();
        services.AddScoped<IOtpAuthenticatorHelper, OtpNetOtpAuthenticatorHelper>();

        return services;
    }
}
