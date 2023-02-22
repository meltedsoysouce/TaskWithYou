using Microsoft.EntityFrameworkCore;
using System.Linq;
using DBKernel.Entity;

namespace DBKernel.Repositories
{
    public interface ITaskTicketRepository : IRepositoryBase<TaskTicket>
    {
        Task Add(Guid pGid,
                int pTourokuBi,
                string pName,
                int pKigenBi,
                string pDetail,
                bool pIsTodayTask,
                Guid pTaskStateGid,
                Guid pCluster);

        Task Edit(Guid pGid,
                int pTourokuBi,
                string pName,
                int pKigenBi,
                string pDetail,
                bool pIsTodayTask,
                Guid pTaskStateGid,
                Guid pCluster);

        void UpdateTasks(TaskTicket[] pTasks);

        Task Delete(Guid pGid);
    }

    public class TaskTicketRepository : ITaskTicketRepository
    {
        private readonly AppDbContext _DbContext;
        
        public TaskTicketRepository(AppDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public TaskTicket[] GetAll()
        {
            return _DbContext
                .TaskTickets
                .AsNoTracking()
                .ToArray();
        }

        public TaskTicket? GetByGid(Guid pGid)
        {
            return _DbContext
                .TaskTickets
                .AsNoTracking()
                .FirstOrDefault(a => a.Gid == pGid);
        }

        public async Task Edit(Guid pGid,
                int pTourokuBi,
                string pName,
                int pKigenBi,
                string pDetail,
                bool pIsTodayTask,
                Guid pTaskStateGid,
                Guid pCluster)
        {
            var task = _DbContext.TaskTickets.First(a => a.Gid == pGid);

            task.Name = pName;
            task.TourokuBi = pTourokuBi;
            task.KigenBi = pKigenBi;
            task.Detail = pDetail;
            task.TaskState = pTaskStateGid;
            task.IsTodayTask = pIsTodayTask;
            task.Cluster = pCluster;
            
            await _DbContext.SaveChangesAsync();
        }

        public void UpdateTasks(TaskTicket[] pTasks)
        {
            _DbContext.AddRange(pTasks);
            _DbContext.SaveChanges();
        }

        public async Task Add(Guid pGid,
                int pTourokuBi,
                string pName,
                int pKigenBi,
                string pDetail,
                bool pIsTodayTask,
                Guid pTaskStateGid,
                Guid pCluster)
        {
            TaskTicket entity = new()
            {
                Gid = pGid,
                TourokuBi = pTourokuBi,
                Name = pName,
                KigenBi = pKigenBi,
                Detail = pDetail,
                TaskState = pTaskStateGid,
                IsTodayTask = pIsTodayTask,
                Cluster = pCluster
            };

            _DbContext.Add(entity);
            await _DbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid pGid)
        {
            var target = _DbContext
                .TaskTickets
                .First(a => a.Gid == pGid);

            _DbContext.Remove(target);
            await _DbContext.SaveChangesAsync();
        }
    }
}
