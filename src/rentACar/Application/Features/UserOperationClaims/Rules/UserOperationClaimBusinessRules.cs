using Application.Features.UserOperationClaims.Constants;
using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Domain.Entities;

namespace Application.Features.UserOperationClaims.Rules;
public class UserOperationClaimBusinessRules : BaseBusinessRules
{
    private readonly IUserOperationClaimRepository _userOperationClaimRepository;

    public UserOperationClaimBusinessRules(IUserOperationClaimRepository userOperationClaimRepository)
    {
        _userOperationClaimRepository = userOperationClaimRepository;
    }

    public async Task UserOperationClaimShouldExistWhenSelected(UserOperationClaim? userOperationClaim)
    {
        if (userOperationClaim is null)
            throw new BusinessException(UserOperationClaimsMessages.UserOperationClaimNotExists);
    }

    public async Task UserOperationClaimIdShouldExistWhenSelected(int id)
    {
        bool doesExist = await _userOperationClaimRepository.AnyAsync(
            predicate: b => b.Id.Equals(id));

        if (!doesExist)
            throw new BusinessException(UserOperationClaimsMessages.UserOperationClaimNotExists);
    }

    public async Task UserOperationClaimShouldNotExistWhenSelected(UserOperationClaim? userOperationClaim)
    {
        if (userOperationClaim is not null)
            throw new BusinessException(UserOperationClaimsMessages.UserOperationClaimAlreadyExists);
    }

    public async Task UserShouldNotHasOperationClaimAlreadyWhenInsert(int userId, int operationClaimId)
    {
        bool doesExist = await _userOperationClaimRepository.AnyAsync(
            predicate: u => u.UserId.Equals(userId) && u.OperationClaimId.Equals(operationClaimId));

        if (doesExist)
            throw new BusinessException(UserOperationClaimsMessages.UserOperationClaimAlreadyExists);
    }

    public async Task UserShouldNotHasOperationClaimAlreadyWhenUpdated(int id, int userId, int operationClaimId)
    {
        bool doesExist = await _userOperationClaimRepository.AnyAsync(
            predicate: uoc => uoc.Id.Equals(id) && uoc.UserId.Equals(userId) && uoc.OperationClaimId.Equals(operationClaimId));

        if (doesExist)
            throw new BusinessException(UserOperationClaimsMessages.UserOperationClaimAlreadyExists);
    }
}