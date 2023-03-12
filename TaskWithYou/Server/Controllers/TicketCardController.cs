using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskWithYou.Shared.Model;
using DBKernel.Repositories;
using TaskWithYou.Client.Pages.Content.Tasks;

namespace TaskWithYou.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketCardController : ControllerBase
    {
        private readonly ITicketCardRepository _TicketCardRepository;
        private readonly ITaskTicketRepository _TaskTicketRepository;

        public TicketCardController(ITicketCardRepository ticketCardRepository,
            ITaskTicketRepository taskTicketRepository)
        {
            _TicketCardRepository = ticketCardRepository;
            _TaskTicketRepository = taskTicketRepository;
        }

        //[HttpGet]
        //public async Task<ActionResult<TicketCard[]>> GetAll()
        //{
        //    return _TicketCardRepository
        //        .GetAll()
        //        .Join(_TaskTicketRepository.GetAll(),
        //            card => card.TaskTicket,
        //            task => task.Gid,
        //            (card, task) => new { card, task })
        //        .Select(a =>
        //        {
        //            TicketCard card = new TicketCard()
        //            { 
        //                Gid = a.card.Gid,
        //                XCoordinate = a.card.XCoordinate,
        //                YCoordinate = a.card.YCorrdinate
        //            };

        //            TaskTicket ticket =
        //        });
        //}
    }
}
