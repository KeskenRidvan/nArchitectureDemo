﻿using Application.Features.Auths.Rules;
using Application.Services.AuthenticatorService;
using Application.Services.Repositories;
using Application.Services.UserService;
using Core.Application.Pipelines.Authorizations;
using Domain.Entities;
using MediatR;

namespace Application.Features.Auths.Commands.EnableOtpAuthenticator;
public class EnableOtpAuthenticatorCommand : IRequest<EnabledOtpAuthenticatorResponse>, ISecuredRequest
{
    public int UserId { get; set; }

    public string[] Roles => [];

    public class EnableOtpAuthenticatorCommandHandler
        : IRequestHandler<EnableOtpAuthenticatorCommand, EnabledOtpAuthenticatorResponse>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IAuthenticatorService _authenticatorService;
        private readonly IOtpAuthenticatorRepository _otpAuthenticatorRepository;
        private readonly IUserService _userService;

        public EnableOtpAuthenticatorCommandHandler(
            IUserService userService,
            IOtpAuthenticatorRepository otpAuthenticatorRepository,
            AuthBusinessRules authBusinessRules,
            IAuthenticatorService authenticatorService)
        {
            _userService = userService;
            _otpAuthenticatorRepository = otpAuthenticatorRepository;
            _authBusinessRules = authBusinessRules;
            _authenticatorService = authenticatorService;
        }

        public async Task<EnabledOtpAuthenticatorResponse> Handle(
            EnableOtpAuthenticatorCommand request,
            CancellationToken cancellationToken)
        {
            User? user = await _userService.GetAsync(
                predicate: u => u.Id.Equals(request.UserId),
                cancellationToken: cancellationToken
            );
            await _authBusinessRules.UserShouldBeExistsWhenSelected(user);
            await _authBusinessRules.UserShouldNotBeHaveAuthenticator(user!);

            OtpAuthenticator? doesExistOtpAuthenticator = await _otpAuthenticatorRepository.GetAsync(
                predicate: o => o.UserId.Equals(request.UserId),
                cancellationToken: cancellationToken);

            await _authBusinessRules.OtpAuthenticatorThatVerifiedShouldNotBeExists(doesExistOtpAuthenticator);

            if (doesExistOtpAuthenticator is not null)
                await _otpAuthenticatorRepository.DeleteAsync(doesExistOtpAuthenticator);

            OtpAuthenticator newOtpAuthenticator =
                await _authenticatorService.CreateOtpAuthenticator(user!);

            OtpAuthenticator addedOtpAuthenticator =
                await _otpAuthenticatorRepository.AddAsync(newOtpAuthenticator);

            EnabledOtpAuthenticatorResponse enabledOtpAuthenticatorDto = new()
            {
                SecretKey = await _authenticatorService.ConvertSecretKeyToString(addedOtpAuthenticator.SecretKey)
            };

            return enabledOtpAuthenticatorDto;
        }
    }
}