using Microsoft.AspNetCore.Mvc.Infrastructure;
using ServerSide = DBKernel.Entity;
using ClientSide = TaskWithYou.Shared.Model;

namespace TaskWithYou.Server.Utilities
{
    internal static class TaskTicketUtilities
    {
        public static ClientSide.TaskTicket ServerSideToClientSide(
            ServerSide.TaskTicket pvTicket)
        {
            return ServerSideToClientSide(pvTicket, null, null);
        }

        public static ClientSide.TaskTicket ServerSideToClientSide(
            ServerSide.TaskTicket pvTicket,
            ServerSide.TaskState pvState,
            ServerSide.Cluster pvCluster)
        {
            ClientSide.TaskTicket ticket = new ClientSide.TaskTicket()
            {
                Gid = pvTicket.Gid,
                Name = pvTicket.Name,
                TourokuBi = pvTicket.TourokuBi,
                KigenBi = pvTicket.KigenBi,
                Detail = pvTicket.Detail,
                IsTodayTask = pvTicket.IsTodayTask,
                State = null,
                Cluster = null
            };

            if (pvState != null ) 
            {
                ticket.State = new()
                {
                    Gid = pvState.Gid,
                    State = pvState.State,
                    StateName = pvState.StateName,
                };
            }

            if (pvCluster != null ) 
            {
                ticket.Cluster = new()
                {
                    Gid = pvCluster.Gid,
                    Name = pvCluster.Name,
                    Detail = pvCluster.Detail,
                };
            }

            return ticket;
        }
    }
}
