using Microsoft.EntityFrameworkCore;
using SimpleExample.Application.Interfaces;
using SimpleExample.Domain.Entities;
using SimpleExample.Infrastructure.Data;

namespace SimpleExample.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public void DeleteAsync(User existingUser)
    {
        _dbSet.Remove(existingUser);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }
}
