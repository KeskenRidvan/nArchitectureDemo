using Application.Features.Users.Constants;
using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Core.Security.Hashings;
using Domain.Entities;

namespace Application.Features.Users.Rules;
public class UserBusinessRules : BaseBusinessRules
{
    private readonly IUserRepository _userRepository;

    public UserBusinessRules(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task UserShouldBeExistsWhenSelected(User? user)
    {
        if (user is null)
            throw new BusinessException(UsersMessages.UserDontExists);
    }

    public async Task UserIdShouldBeExistsWhenSelected(int id)
    {
        bool doesExist =
            await _userRepository.AnyAsync(predicate: u => u.Id.Equals(id));

        if (doesExist)
            throw new BusinessException(UsersMessages.UserDontExists);
    }

    public async Task UserPasswordShouldBeMatched(User user, string password)
    {
        if (!HashingHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            throw new BusinessException(UsersMessages.PasswordDontMatch);
    }

    public async Task UserEmailShouldNotExistsWhenInsert(string email)
    {
        bool doesExists = await _userRepository.AnyAsync(
            predicate: u => u.Email.Equals(email));

        if (doesExists)
            throw new BusinessException(UsersMessages.UserMailAlreadyExists);
    }

    public async Task UserEmailShouldNotExistsWhenUpdate(int id, string email)
    {
        bool doesExists = await _userRepository.AnyAsync(
            predicate: u => !u.Id.Equals(id) && u.Email.Equals(email));

        if (doesExists)
            throw new BusinessException(UsersMessages.UserMailAlreadyExists);
    }
}