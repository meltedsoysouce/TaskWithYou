using DBKernel;
using DataBase = DBKernel.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Extensions.Configuration;
using TaskWithYou.Shared.Model;

namespace TaskWithYou.Server
{
    public static class StartUp
    {
        public static void SetupDB()
        {
            DbContextOptionsBuilder<AppDbContext> modelbuilder = new();
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = config.GetConnectionString("DefaultConnection");
            modelbuilder.UseSqlite(connectionString);

            using (AppDbContext _DbContext = new(modelbuilder.Options))
            {
                var states = _DbContext.TaskStates.AsNoTracking().ToArray();
                if (states.Count() != 3)
                {
                    states = _DbContext.TaskStates.ToArray();
                    _DbContext.TaskStates.RemoveRange(states);

                    List<DBKernel.Entity.TaskState> initials = new()
                    {
                        new DBKernel.Entity.TaskState() { Gid = Guid.NewGuid(), StateName = "未実行", State = State.BeforeDoing},
                        new DBKernel.Entity.TaskState() { Gid = Guid.NewGuid(), StateName = "実行中", State = State.Doing},
                        new DBKernel.Entity.TaskState() { Gid = Guid.NewGuid(), StateName = "完了", State = State.Finished }
                    };

                    _DbContext.TaskStates.AddRange(initials);
                    _DbContext.SaveChanges();

                    //_DbContext.Database.ExecuteSql($"DELETE FROM TaskStates");
                    //var gid = Guid.NewGuid();
                    //_DbContext.Database.ExecuteSql($"INSERT INTO TaskStates (Gid, StateName) VALUES ('{gid}', '未着手')");
                    //gid = Guid.NewGuid();
                    //_DbContext.Database.ExecuteSql($"INSERT INTO TaskStates (Gid, StateName) VALUES ('{gid}', '実行中')");
                    //gid = Guid.NewGuid();
                    //_DbContext.Database.ExecuteSql($"INSERT INTO TaskStates (Gid, StateName) VALUES ('{gid}', '完了')");
                }

                var clusters = _DbContext.Clusters.AsNoTracking().FirstOrDefault();
                if (clusters == null)
                {
                    DataBase.Cluster cluster = new() { Gid = Guid.NewGuid(), Name = "test", Detail = "Initrialize" };
                    _DbContext.Clusters.Add(cluster);
                    _DbContext.SaveChanges();
                }
            }
        }
    }
}
