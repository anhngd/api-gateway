using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext _context;

        public UserRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<List<IdentityServer.Models.ActivityHistory>> GetAllActivityHistory(CancellationToken cancellationToken)
        {
            return await _context.ActivityHistories.ToListAsync(cancellationToken);
        }

        public async Task<List<IdentityServer.Models.ActivityHistory>> GetActivityHistoryByUserId(string userId, CancellationToken cancellationToken)
        {
            return await _context.ActivityHistories.Where(x => x.UserId == userId).ToListAsync(cancellationToken);
        }

        public async Task<IdentityServer.Models.ActivityHistory> GetActivityHistoryById(int id, CancellationToken cancellationToken)
        {
            return await _context.ActivityHistories.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        
    }
}
