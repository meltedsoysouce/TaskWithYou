using DBKernel.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using TaskWithYou.Shared.Model;
using ClientSide = TaskWithYou.Shared.Model;
using ServerSide = DBKernel.Entity;
using DBKernel;

namespace TaskWithYou.Server.Controllers
{
    public partial class TaskTicketController
    {
        public static ClientSide.TaskTicket ConvertToClientSide(
            ServerSide.TaskTicket pvTicket,
            ServerSide.TaskState pvState,
            ServerSide.Cluster pvCluster)
        {
            ClientSide.TaskTicket ticket = new()
            {
                Gid = pvTicket.Gid,
                Name = pvTicket.Name,
                TourokuBi = pvTicket.TourokuBi,
                KigenBi = pvTicket.KigenBi,
                Detail = pvTicket.Detail,
                IsTodayTask = pvTicket.IsTodayTask
            };

            if (pvState != null)
            {
                ticket.State = new()
                {
                    Gid = pvState.Gid,
                    State = pvState.State,
                    StateName = pvState.StateName,
                };
            }
            else
                ticket.State = new();

            if (pvCluster != null)
            {
                ticket.Cluster = new()
                {
                    Gid = pvCluster.Gid,
                    Name = pvCluster.Name,
                    Detail = pvCluster.Detail,
                };
            }
            else
                ticket.Cluster = new();

            return ticket;
        }

        public static IEnumerable<ClientSide.TaskTicket> GetTicketsFromServer()
        {
            IConfiguration conf = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = conf.GetConnectionString("DefaultConnection");
            DbContextOptionsBuilder<AppDbContext> option 
                = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={connectionString}");

            IEnumerable<ClientSide.TaskTicket> result;
            using (var context = new AppDbContext(option.Options))
            {
                result = context
                    .TaskTickets
                    .AsNoTracking()
                    .Join(context.TaskStates.AsNoTracking().AsEnumerable(),
                        task => task.TaskState,
                        state => state.Gid,
                        (task, state) => new { task, state })
                    .GroupJoin(context.Clusters.AsNoTracking().AsEnumerable(),
                        a => a.task.Cluster,
                        b => b.Gid,
                        (a, b) => new { a.task, a.state, b })
                    .SelectMany(a =>
                        a.b.DefaultIfEmpty(),
                        (a, b) => new
                        {
                            task = a.task,
                            state = a.state,
                            cluster = b
                        })
                    .Select(a => ConvertToClientSide(a.task, a.state, a.cluster))
                    .AsEnumerable();
            }

            return result;
        }
    }
}
