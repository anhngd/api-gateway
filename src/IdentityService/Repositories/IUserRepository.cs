using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityService.Repositories
{
    public interface IUserRepository
    {
        Task<List<IdentityServer.Models.ActivityHistory>> GetAllActivityHistory(CancellationToken cancellationToken);

        Task<List<IdentityServer.Models.ActivityHistory>> GetActivityHistoryByUserId(string userId, CancellationToken cancellationToken);

        Task<IdentityServer.Models.ActivityHistory> GetActivityHistoryById(int id, CancellationToken cancellationToken);
        
    }
}
