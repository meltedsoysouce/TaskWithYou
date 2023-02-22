using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskWithYou.Shared.Model;
using ServerSide = DBKernel;
using DBKernel;
using System.Security.AccessControl;
using DBKernel.Repositories;
using Microsoft.AspNetCore.Components.Forms;

namespace TaskWithYou.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTicketController : ControllerBase
    {
        private readonly ITaskTicketRepository _TaskTicketRepository;
        private readonly ITaskStateRepository _TaskStateRepository;
        private readonly IClusterRepository _ClusterRepository;

        public TaskTicketController(ITaskTicketRepository taskRepository,
            ITaskStateRepository taskStateRepository,
            IClusterRepository clusterRepository)
        {
            _TaskTicketRepository = taskRepository;
            _TaskStateRepository = taskStateRepository;
            _ClusterRepository = clusterRepository;
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
                .GroupJoin(_ClusterRepository.GetAll(),
                    a => a.task.Cluster,
                    b => b.Gid,
                    (a, b) => new
                    {
                        a.task,
                        a.state,
                        b
                    })
                .SelectMany(a =>
                    a.b.DefaultIfEmpty(),
                    (a, b) => new
                    {
                        task = a.task,
                        state = a.state,
                        cluster = b
                    })
                .ToArray()
                .Select(a =>
                {
                    TaskTicket ticket = new()
                    {
                        Gid = a.task.Gid,
                        Name = a.task.Name,
                        TourokuBi = a.task.TourokuBi,
                        KigenBi = a.task.KigenBi,
                        Detail = a.task.Detail,
                        IsTodayTask = a.task.IsTodayTask,
                    };

                    TaskState state = new()
                    {
                        Gid = a.state.Gid,
                        StateName = a.state.StateName,
                        State = a.state.State
                    };
                    ticket.State = state;

                    Cluster cluster = new();
                    if (a.cluster != null)
                    {
                        cluster = new()
                        {
                            Gid = a.cluster.Gid,
                            Name = a.cluster.Name,
                            Detail = a.cluster.Detail
                        };
                    }
                    ticket.Cluster = cluster;

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
                Detail = task.Detail,
                IsTodayTask = task.IsTodayTask,
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

            var _cluster = _ClusterRepository
                .GetByGid(task.Cluster);
            Cluster cluster = null;
            if (_cluster != null)
            {
                cluster = new()
                {
                    Gid = _cluster.Gid,
                    Name = _cluster.Name,
                    Detail = _cluster.Detail,
                };
            }
            ticket.Cluster = cluster;

            return ticket;
        }

        [HttpGet("today")]
        public async Task<ActionResult<TaskTicket[]>> GetTodayList()
        {
            return _TaskTicketRepository
                .GetAll()
                .Where(a => a.IsTodayTask == true)
                .Join(_TaskStateRepository.GetAll(),
                    task => task.TaskState,
                    state => state.Gid,
                    (task, state) => new { task, state })
                .GroupJoin(_ClusterRepository.GetAll(),
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
                .ToArray()
                .Select(a =>
                {
                    TaskTicket ticket = new()
                    {
                        Gid = a.task.Gid,
                        Name = a.task.Name,
                        TourokuBi = a.task.TourokuBi,
                        KigenBi = a.task.KigenBi,
                        Detail = a.task.Detail,
                        IsTodayTask = a.task.IsTodayTask,
                    };

                    TaskState state = new()
                    {
                        Gid = a.state.Gid,
                        StateName = a.state.StateName,
                        State = a.state.State
                    };
                    ticket.State = state;

                    Cluster cluster = new();
                    if (a.cluster != null)
                    {
                        cluster.Gid = a.cluster.Gid;
                        cluster.Name = a.cluster.Name;
                        cluster.Detail = a.cluster.Detail;
                    }
                    ticket.Cluster = cluster;

                    return ticket;
                })
                .ToArray();
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
                    return new ServerSide.Entity.TaskTicket()
                    {
                        Gid = a.Gid,
                        Name = a.Name,
                        IsTodayTask = a.IsTodayTask,
                        TourokuBi = a.TourokuBi,
                        KigenBi = a.KigenBi,
                        Detail = a.Detail,
                        TaskState = a.State.Gid,
                        Cluster = a.Cluster.Gid
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

            //return true;
        }

        [HttpDelete("{pGid}")]
        public async Task DeleteTask(Guid pGid)
        {
            await _TaskTicketRepository.Delete(pGid);
        }
    }
}
