using TaskWithYou.Shared.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace DBKernel.Queries
{
    public static class TaskTicketQuery
    {
        public static IEnumerable<TaskTicket> GetAllTask()
        {
            var dbOption = SetupDBOptions();
            using (AppDbContext _DBContext = new(dbOption))
            {
                // 各種テーブルからあらかじめ全件取得をしておく → おそらくメモリ上に先に確保するほうが早い
                var tasks = _DBContext.TaskTickets.AsNoTracking().AsEnumerable();
                var taskStates = _DBContext.TaskStates.AsNoTracking().AsEnumerable();
                var clusters = _DBContext.Clusters.AsNoTracking().AsEnumerable();
                var cards = _DBContext.TicketCards.AsNoTracking().AsEnumerable();

                var taskandStates = tasks
                    .Join(taskStates,
                        t => t.StateGid,
                        s => s.Gid,
                        (task, state) => new { task, state })
                    .Select(task =>
                    {
                        task.task.StateGid = task.state.Gid;
                        task.task.State = task.state;
                        return task.task;
                    })
                    .AsEnumerable();
                var tsCluster = taskandStates
                    .GroupJoin(clusters,
                        t => t.ClusterGid,
                        c => c.Gid,
                        (task, cluster) => new { task, cluster })
                    .SelectMany(ts => ts.cluster.DefaultIfEmpty(),
                        (ts, cluster) => new
                        {
                            task = ts.task,
                            cluster = cluster
                        })
                    .Select(t =>
                    {
                        var task = t.task;
                        task.ClusterGid = t.cluster.Gid;
                        task.Cluster = t.cluster;
                        return task;
                    })
                    .AsEnumerable();
                var tscCard = tsCluster
                    .Join(cards,
                        tcs => tcs.CardGid,
                        card => card.Gid,
                        (tcs, catd) => new
                        {
                            task = tcs,
                            card = catd
                        })
                    .Select(t =>
                    {
                        var task = t.task;
                        task.CardGid = t.card.Gid;
                        task.Card = t.card;
                        return task;
                    })
                    .AsEnumerable();

                return tscCard;
            }
        }

        public static IEnumerable<TaskTicket> GetTodayTaskList()
        {
            var alltask = GetAllTask();
            return alltask
                .Where(t => t.IsTodayTask == true)
                .AsEnumerable();
        }

        public static TaskTicket? GetTaskByTaskOid(Guid pTaskGid)
        {
            var dbOption = SetupDBOptions();
            using (AppDbContext _DbContext = new(dbOption))
            {
                var target = _DbContext
                    .TaskTickets
                    .FirstOrDefault(t => t.Gid == pTaskGid);

                if (target is null)
                    return null;

                target.State = _DbContext.TaskStates.First(s => s.Gid == target.StateGid);
                target.Card = _DbContext.TicketCards.First(c => c.Gid == target.CardGid);
                if (target.ClusterGid != Guid.Empty)
                    target.Cluster = _DbContext.Clusters.First(c => c.Gid == target.ClusterGid);

                return target;
            }
        }

        private static IConfiguration SetupConfiguration()
        {
            var conf = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            return conf;
        }

        private static DbContextOptions<AppDbContext> SetupDBOptions()
        {
            DbContextOptionsBuilder<AppDbContext> dbOptionBuilder = new();
            var conf = SetupConfiguration();
            var connectionString = conf.GetConnectionString("DefaultConnection");
            dbOptionBuilder.UseSqlite($"Data Source={connectionString}");
            return dbOptionBuilder.Options;
        }
    }
}
