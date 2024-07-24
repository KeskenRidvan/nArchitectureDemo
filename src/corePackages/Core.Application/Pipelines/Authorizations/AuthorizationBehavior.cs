using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Security.Constants;
using Core.Security.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Core.Application.Pipelines.Authorizations;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ISecuredRequest
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationBehavior(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (!_httpContextAccessor.HttpContext.User.Claims.Any())
            throw new AuthorizationException("You are not authenticated.");

        if (request.Roles.Any())
        {
            ICollection<string>? userRoleClaims = _httpContextAccessor.HttpContext.User.GetRoleClaims() ?? [];
            bool isNotMatchedAUserRoleClaimWithRequestRoles = userRoleClaims
                .FirstOrDefault(userRoleClaim =>
                    userRoleClaim == GeneralOperationClaims.Admin || request.Roles.Contains(userRoleClaim)
                )
                .IsNullOrEmpty();
            if (isNotMatchedAUserRoleClaimWithRequestRoles)
                throw new AuthorizationException("You are not authorized.");
        }

        TResponse response = await next();
        return response;
    }
}