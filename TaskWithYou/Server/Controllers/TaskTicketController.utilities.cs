using DBKernel.Entity;
using Microsoft.AspNetCore.Mvc;
using ClientSide = TaskWithYou.Shared.Model;
using ServerSide = DBKernel.Entity;

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
    }
}
