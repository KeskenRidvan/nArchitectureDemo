using Application.Features.Auths.Rules;
using Application.Services.AuthService;
using Application.Services.UserService;
using Core.Security.JWT;
using Domain.Entities;
using MediatR;

namespace Application.Features.Auths.Commands.RefleshToken;
public class RefreshTokenCommand : IRequest<RefreshedTokensResponse>
{
    public string RefreshToken { get; set; }
    public string IpAddress { get; set; }

    public RefreshTokenCommand()
    {
        RefreshToken = string.Empty;
        IpAddress = string.Empty;
    }

    public RefreshTokenCommand(string refreshToken, string ipAddress)
    {
        RefreshToken = refreshToken;
        IpAddress = ipAddress;
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshedTokensResponse>
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly AuthBusinessRules _authBusinessRules;

        public RefreshTokenCommandHandler(
            IAuthService authService,
            IUserService userService,
            AuthBusinessRules authBusinessRules)
        {
            _authService = authService;
            _userService = userService;
            _authBusinessRules = authBusinessRules;
        }

        public async Task<RefreshedTokensResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            RefreshToken? refreshToken =
                await _authService.GetRefreshTokenByToken(request.RefreshToken);

            await _authBusinessRules.RefreshTokenShouldBeExists(refreshToken);

            if (refreshToken!.RevokedDate is not null)
                await _authService.RevokeDescendantRefreshTokens(
                    refreshToken,
                    request.IpAddress,
                    reason: $"Attempted reuse of revoked ancestor token: {refreshToken.Token}");

            await _authBusinessRules.RefreshTokenShouldBeActive(refreshToken);

            User? user = await _userService.GetAsync(
                predicate: u => u.Id.Equals(refreshToken.UserId),
                cancellationToken: cancellationToken);

            await _authBusinessRules.UserShouldBeExistsWhenSelected(user);

            RefreshToken newRefreshToken = await _authService.RotateRefreshToken(
                user: user!,
                refreshToken,
                request.IpAddress);

            RefreshToken addedRefreshToken =
                await _authService.AddRefreshToken(newRefreshToken);

            await _authService.DeleteOldRefreshTokens(refreshToken.UserId);

            AccessToken createdAccessToken = await _authService.CreateAccessToken(user!);

            RefreshedTokensResponse refreshedTokensResponse = new()
            {
                AccessToken = createdAccessToken,
                RefreshToken = addedRefreshToken
            };

            return refreshedTokensResponse;
        }
    }
}