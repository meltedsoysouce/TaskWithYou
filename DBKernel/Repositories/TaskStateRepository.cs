using DBKernel.Entity;
using Microsoft.EntityFrameworkCore;

namespace DBKernel.Repositories
{
    public interface ITaskStateRepository : IRepositoryBase<TaskState>
    {

    }

    public class TaskStateRepository : ITaskStateRepository
    {
        private readonly AppDbContext _DbContext;

        public TaskStateRepository(AppDbContext context) 
        {
            _DbContext = context;
        }

        public TaskState[] GetAll()
        {
            return _DbContext
                .TaskStates
                .AsNoTracking()
                .ToArray();
        }

        public TaskState? GetByGid(Guid pGid)
        {
            return _DbContext
                .TaskStates
                .AsNoTracking()
                .FirstOrDefault(a => a.Gid == pGid);
        }
    }
}
