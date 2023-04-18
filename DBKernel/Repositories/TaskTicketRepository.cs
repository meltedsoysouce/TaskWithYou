using Microsoft.EntityFrameworkCore;
using TaskWithYou.Shared.Model;
using DBKernel.Queries;

namespace DBKernel.Repositories
{
    public interface ITaskTicketRepository : IRepositoryBase<TaskTicket>
    {
        IEnumerable<TaskTicket> GetAllTask();

        IEnumerable<TaskTicket> GetTodayTaskList();

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
            //return _DbContext
            //    .TaskTickets
            //    .AsNoTracking()
            //    .FirstOrDefault(a => a.Gid == pGid);
            return Queries.TaskTicketQuery.GetTaskByTaskOid(pGid);
        }

        public IEnumerable<TaskTicket> GetAllTask()
        {
            return Queries.TaskTicketQuery.GetAllTask();
        }
        
        public IEnumerable<TaskTicket> GetTodayTaskList()
        {
            return Queries.TaskTicketQuery.GetTodayTaskList();
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
            task.StateGid = pTaskStateGid;
            task.IsTodayTask = pIsTodayTask;
            task.ClusterGid = pCluster;
            
            await _DbContext.SaveChangesAsync();
        }

        public void UpdateTasks(TaskTicket[] pTasks)
        {
            var entities = pTasks
                .Select(a =>
                {
                    var entity = _DbContext
                        .TaskTickets
                        .First(b => b.Gid == a.Gid);

                    entity.Name = a.Name;
                    entity.TourokuBi = a.TourokuBi;
                    entity.KigenBi = a.KigenBi;
                    entity.Detail = a.Detail;
                    entity.State = a.State;
                    entity.IsTodayTask = a.IsTodayTask;
                    entity.Cluster = a.Cluster;

                    return entity;
                })
                .ToArray();
            _DbContext.UpdateRange(entities);
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
            var cardGid = Guid.NewGuid();
            TaskTicket entity = new()
            {
                Gid = pGid,
                TourokuBi = pTourokuBi,
                Name = pName,
                KigenBi = pKigenBi,
                Detail = pDetail,
                StateGid = pTaskStateGid,
                IsTodayTask = pIsTodayTask,
                ClusterGid = pCluster,
                CardGid = cardGid
            };
            _DbContext.Add(entity);

            TicketCard card = new()
            {
                Gid = cardGid,
                TaskTicket = entity.Gid,
                XCoordinate = 0,
                YCoordinate = 0
            };
            _DbContext.Add(card);

            await _DbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid pGid)
        {
            var target = _DbContext
                .TaskTickets
                .First(a => a.Gid == pGid);
            var card = _DbContext
                .TicketCards
                .First(a => a.Gid == target.CardGid);
            _DbContext.Remove(target);
            _DbContext.Remove(card);

            await _DbContext.SaveChangesAsync();
        }
    }
}
