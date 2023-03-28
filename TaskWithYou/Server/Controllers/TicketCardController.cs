using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskWithYou.Shared.Model;
using DBKernel.Repositories;

namespace TaskWithYou.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketCardController : ControllerBase
    {
        private readonly ITicketCardRepository _TicketCardRepository;
        private readonly ITaskTicketRepository _TaskTicketRepository;
        private readonly ITaskStateRepository _TaskStateRepository;
        private readonly IClusterRepository _ClusterRepository;

        public TicketCardController(ITicketCardRepository ticketCardRepository,
            ITaskTicketRepository taskTicketRepository,
            ITaskStateRepository taskStateRepository,
            IClusterRepository clusterRepository)
        {
            _TicketCardRepository = ticketCardRepository;
            _TaskTicketRepository = taskTicketRepository;
            _TaskStateRepository = taskStateRepository;
            _ClusterRepository = clusterRepository;
        }

        [HttpGet]
        public ActionResult<TicketCard[]> GetAll()
        {
            return _TicketCardRepository
                .GetAll()
                .Join(_TaskTicketRepository.GetAll(),
                    card => card.TaskTicket,
                    task => task.Gid,
                    (card, task) => new { card, task })                
                .Select(a =>
                {
                    TicketCard card = new TicketCard()
                    {
                        Gid = a.card.Gid,
                        XCoordinate = a.card.XCoordinate,
                        YCoordinate = a.card.YCoordinate
                    };

                    TaskTicket ticket = new()
                    {
                        Gid = a.task.Gid,
                        Name = a.task.Name,
                        TourokuBi = a.task.TourokuBi,
                        IsTodayTask = a.task.IsTodayTask,
                        KigenBi = a.task.KigenBi,
                        Detail = a.task.Detail
                    };

                    TaskState state = new();
                    if (a.task.TaskState != Guid.Empty)
                    {
                        var _state = _TaskStateRepository.GetByGid(a.task.TaskState);

                        state.Gid = _state.Gid;
                        state.StateName = _state.StateName;
                        state.State = _state.State;
                    }
                    ticket.State = state;

                    Cluster cluster = new();
                    if (a.task.Cluster != Guid.Empty)
                    {
                        var _cluster = _ClusterRepository.GetByGid(a.task.Cluster);

                        cluster.Gid = _cluster.Gid;
                        cluster.Name = _cluster.Name;
                        cluster.Detail = _cluster.Detail;
                    }
                    ticket.Cluster = cluster;

                    card.Task = ticket;
                    return card;
                })
                .ToArray();
            //var result = _TicketCardRepository
            //    .GetAll()
            //    .Join(TaskTicketController.GetTicketsFromServer(),
            //        card => card.TaskTicket,
            //        task => task.Gid,
            //        (card, task) => new { card, task })
            //    .Select(a =>
            //    {
            //        TicketCard card = new()
            //        {
            //            Gid = a.card.Gid,
            //            XCoordinate = a.card.XCoordinate,
            //            YCoordinate = a.card.YCoordinate,   
            //            Task = a.task
            //        };

            //        return card;
            //    })
            //    .ToArray();
            //return result;
        }
    }
}
