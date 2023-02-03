using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskWithYou.Shared.Model;
using DBKernel;
using System.Security.AccessControl;
using DBKernel.Repositories;
//using TaskWithYou.Shared.Content;
using Microsoft.AspNetCore.Components.Forms;

namespace TaskWithYou.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTicketController : ControllerBase
    {
        private readonly ITaskTicketRepository _TaskTicketRepository;
        private readonly ITaskStateRepository _TaskStateRepository;

        public TaskTicketController(ITaskTicketRepository taskRepository,
            ITaskStateRepository taskStateRepository)
        {
            _TaskTicketRepository = taskRepository;
            _TaskStateRepository = taskStateRepository;
        }

        [HttpGet]
        public async Task<ActionResult<TaskTicket[]>> GetAll()
        {
            return _TaskTicketRepository
                .GetAll()
                .Join(_TaskStateRepository.GetAll(),
                    task => task.TaskState,
                    state => state.Gid,
                    (task, state) => new { task, state})
                .Select(a =>
                {
                    TaskTicket ticket = new()
                    {
                        Gid = a.task.Gid,
                        Name = a.task.Name,
                        TourokuBi = a.task.TourokuBi,
                        KigenBi = a.task.KigenBi,
                        Detail = a.task.Detail,
                    };

                    TaskState state = new()
                    {
                        Gid = a.state.Gid,
                        StateName = a.state.StateName,
                        State = a.state.State
                    };

                    ticket.State = state;
                    return ticket;
                })
                .ToArray();
        }

        [HttpGet("{pGid}")]
        public async Task<ActionResult<TaskTicket?>> GetTask(Guid pGid)
        {
            var task = _TaskTicketRepository
                .GetByGid(pGid);

            if (task == null)
                return NotFound();

            TaskTicket ticket = new()
            {
                Gid = task.Gid,
                Name = task.Name,
                TourokuBi = task.TourokuBi,
                KigenBi = task.KigenBi,
                Detail = task.Detail
            };

            var _state = _TaskStateRepository
                .GetByGid(task.TaskState);
            TaskState state = new()
            {
                Gid = _state.Gid,
                StateName = _state.StateName,
                State = _state.State
            };

            ticket.State = state;
            return ticket;
        }

        [HttpPost]
        public bool AddTask(TaskTicket pTask)
        {
            _TaskTicketRepository.Add(
                pTask.Gid,
                pTask.TourokuBi,
                pTask.Name,
                pTask.KigenBi,
                pTask.Detail,
                pTask.State.Gid);

            return true;
        }

        [HttpPut]
        public async Task EditTask(TaskTicket pTask) 
        {
            await _TaskTicketRepository.Edit(
                pTask.Gid,
                pTask.TourokuBi,
                pTask.Name,
                pTask.KigenBi,
                pTask.Detail,
                pTask.State.Gid);

            //return true;
        }

        [HttpDelete("{pGid}")]
        public async Task DeleteTask(Guid pGid)
        {
            await _TaskTicketRepository.Delete(pGid);
        }
    }
}
