using DBKernel.Repositories;
using Microsoft.AspNetCore.Mvc;
using TaskWithYou.Shared.Model;

namespace TaskWithYou.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTicketController : ControllerBase
    {
        private readonly ITaskTicketRepository _TaskTicketRepository;

        public TaskTicketController(ITaskTicketRepository taskRepository)
        {
            _TaskTicketRepository = taskRepository;
        }

        [HttpGet]
        public async Task<ActionResult<TaskTicket[]>> GetAll()
        {
            return _TaskTicketRepository.GetAll();
        }

        [HttpGet("{pGid}")]
        public async Task<ActionResult<TaskTicket?>> GetTask(Guid pGid)
        {
            return DBKernel.Queries.TaskTicketQuery.GetTaskByTaskOid(pGid);
        }

        [HttpGet("today")]
        public ActionResult<TaskTicket[]> GetTodayList()
        {
            TaskTicket[] tasks = { };
            return tasks;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddTask(TaskTicket pTask)
        {
            await _TaskTicketRepository.Add(
                pTask.Gid,
                pTask.TourokuBi,
                pTask.Name,
                pTask.KigenBi,
                pTask.Detail,
                pTask.IsTodayTask,
                pTask.State.Gid,
                pTask.Cluster.Gid);

            return true;
        }

        [HttpPost("updatetasks")]
        public void UpdateTasks(TaskTicket[] pTaskTicktets)
        {
            var entites = pTaskTicktets
                .Select(a =>
                {
                    return new TaskTicket()
                    {
                        Gid = a.Gid,
                        Name = a.Name,
                        IsTodayTask = a.IsTodayTask,
                        TourokuBi = a.TourokuBi,
                        KigenBi = a.KigenBi,
                        Detail = a.Detail,
                        StateGid = a.State.Gid,
                        ClusterGid = a.Cluster.Gid
                    };
                })
                .ToArray();

            _TaskTicketRepository.UpdateTasks(entites);
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
                pTask.IsTodayTask,
                pTask.State.Gid,
                pTask.Cluster.Gid);

            /*return true*/
            ;
        }

        [HttpDelete("{pGid}")]
        public async Task DeleteTask(Guid pGid)
        {
            await _TaskTicketRepository.Delete(pGid);
        }
    }
}
