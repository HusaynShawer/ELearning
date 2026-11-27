using ELearning.Identity.Domain;
using ELearning.Identity.Infrastructure;

namespace ELearning.Identity.Application.Service;

public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
    {
        return await _userRepository.GetByEmailAsync(email);
    }

    public async Task<ApplicationUser?> GetUserByUserNameAsync(string userName)
    {
        return await _userRepository.GetByUserNameAsync(userName);
    }

    public async Task AddUserAsync(ApplicationUser user)
    {
        await _userRepository.AddUserAsync(user);
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task<int> GetUserCountAsync()
    {
        return await _userRepository.GetUserCountAsync();
    }
}
