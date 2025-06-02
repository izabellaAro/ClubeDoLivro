using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IUserProfileRepository : IBaseRepository<UserProfile> 
{
    Task<UserProfile> ConsultProfile(Guid userId);
    Task<IEnumerable<UserProfile>> ConsultAllProfiles(int skip, int take);
    Task<UserProfile> ConsultById(string id);
}