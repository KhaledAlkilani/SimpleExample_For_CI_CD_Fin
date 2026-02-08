using SimpleExample.Domain.Entities;

namespace SimpleExample.Application.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    void DeleteAsync(User existingUser);
    Task<User?> GetByEmailAsync(string email);
}
