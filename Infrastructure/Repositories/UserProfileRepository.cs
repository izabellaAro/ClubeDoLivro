using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserProfileRepository : BaseRepository<UserProfile>, IUserProfileRepository
{
    public UserProfileRepository(DataContext context) : base(context) { }

    public async Task<UserProfile> ConsultProfile(Guid userId)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<IEnumerable<UserProfile>> ConsultAllProfiles(int skip, int take)
    {
        return await _dbSet.Skip(skip).Take(take).ToListAsync();
    }

    public async Task<UserProfile> ConsultById(string id)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
    }
}