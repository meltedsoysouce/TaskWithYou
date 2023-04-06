using DBKernel;
//using DataBase = DBKernel.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Extensions.Configuration;
using TaskWithYou.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection.Metadata.Ecma335;

namespace TaskWithYou.Server
{
    public static class StartUp
    {
        internal static void SetupDB()
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

                    List<TaskState> initials = new()
                    {
                        new TaskState() { Gid = Guid.NewGuid(), Name = "未実行", State = State.BeforeDoing},
                        new TaskState() { Gid = Guid.NewGuid(), Name = "実行中", State = State.Doing},
                        new TaskState() { Gid = Guid.NewGuid(), Name = "完了", State = State.Finished }
                    };

                    _DbContext.TaskStates.AddRange(initials);
                    _DbContext.SaveChanges();
                }

                var clusters = _DbContext.Clusters.AsNoTracking().FirstOrDefault();
                if (clusters == null)
                {
                    Cluster cluster = new() { Gid = Guid.NewGuid(), Name = "test", Detail = "Initrialize" };
                    _DbContext.Clusters.Add(cluster);
                    _DbContext.SaveChanges();
                }
            }
        }

        internal static void SetTestData()
        {
            DbContextOptionsBuilder<AppDbContext> modelbuilder = new();
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = config.GetConnectionString("DefaultConnection");

            modelbuilder.UseSqlite(connectionString);

            // use to generate random string
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            using (AppDbContext _DBContext = new(modelbuilder.Options))
            {
                var state = _DBContext
                    .TaskStates
                    .First(State => State.State == TaskWithYou.Shared.Model.State.BeforeDoing);
                var rand = new Random();
                Func<string> namegenerator = () =>
                {
                    string name = "";
                    foreach (var i in Enumerable.Range(0, 4))
                        name += chars[rand.Next(chars.Count())];

                    return name;
                };

                for (int i = 0; i < 10; i++)
                {
                    var ticketGid = Guid.NewGuid();
                    var cardGid = Guid.NewGuid();
                    TaskTicket ticket = new()
                    {
                        Gid = ticketGid,
                        Name = namegenerator(),
                        IsTodayTask = false,
                        Detail = "",
                        TourokuBi = 20000101,
                        KigenBi = 20000101,
                        ClusterGid = Guid.Empty,
                        StateGid = state.Gid
                    };

                    TicketCard card = new()
                    {
                        Gid = cardGid,
                        XCoordinate = 0f,
                        YCoordinate = 0f,
                        TaskTicket = ticket.Gid
                    };

                    _DBContext.Add(ticket);
                    _DBContext.Add(card);
                    _DBContext.SaveChanges();
                }

            }
        }
    }
}
