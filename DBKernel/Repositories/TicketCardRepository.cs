using Microsoft.EntityFrameworkCore;
using TaskWithYou.Shared.Model;

namespace DBKernel.Repositories
{
    public interface ITicketCardRepository : IRepositoryBase<TicketCard>
    {

    }

    public class TicketCardRepository : ITicketCardRepository
    {
        private readonly AppDbContext _DbContext;

        public TicketCardRepository(AppDbContext context)
        {
            _DbContext = context;
        }

        public TicketCard[] GetAll()
        {
            return _DbContext
                .TicketCards
                .AsNoTracking()
                .ToArray();
        }

        public TicketCard? GetByGid(Guid pGid) 
        {
            return _DbContext
                .TicketCards
                .AsNoTracking()
                .FirstOrDefault(a => a.Gid == pGid);
        }
    }
}

