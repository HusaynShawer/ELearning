using ELearning.Identity.Domain;
using Microsoft.EntityFrameworkCore;

namespace ELearning.Identity.Infrastructure;

public class UserRepository
{
    private readonly AppIdentityDbContext _context;

    public UserRepository(AppIdentityDbContext context)
    {
        _context = context;
    }

    // Find a user using their email
    public async Task<ApplicationUser?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    // Find a user using their username
    public async Task<ApplicationUser?> GetByUserNameAsync(string userName)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName);
    }

    // Add a new user to the database
    public async Task AddUserAsync(ApplicationUser user)
    {
        await _context.Users.AddAsync(user);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<int> GetUserCountAsync()
    {
        return await _context.Users.CountAsync();
    }
}
