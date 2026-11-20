using ELearning.Identity.Domain;
using ELearning.Identity.Infrastructure;

public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApplicationUser?> GetUserByEmail(string email)
    {
        return await _userRepository.GetByEmail(email);
    }

    public async Task<ApplicationUser?> GetUserByUserName(string userName)
    {
        return await _userRepository.GetByUserName(userName);
    }

    public async Task AddUser(ApplicationUser user)
    {
        await _userRepository.AddUser(user);
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
    {
        return await _userRepository.GetAllUsers();
    }

    public async Task<int> GetUserCount()
    {
        return await _userRepository.GetUserCount();
    }
}