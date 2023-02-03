using DBKernel.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskWithYou.Shared.Model;

namespace TaskWithYou.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskStateController : ControllerBase
    {
        private readonly ITaskStateRepository _TaskStateRepository;

        public TaskStateController(ITaskStateRepository taskStateRepository)
        {
            _TaskStateRepository = taskStateRepository;
        }

        [HttpGet]
        public async Task<ActionResult<TaskState[]>> GetAll()
        {
            return _TaskStateRepository
                .GetAll()
                .Select(a =>
                {
                    return new TaskState()
                    {
                        Gid = a.Gid,
                        StateName = a.StateName,
                        State = a.State
                    };
                })
                .ToArray();
        }

        [HttpGet("{pGid}")]
        public async Task<ActionResult<TaskState?>> GetTaskState(Guid pGid)
        {
            var state = _TaskStateRepository
                .GetByGid(pGid);

            if (state == null) 
                return NotFound();

            return new TaskState()
            {
                Gid = state.Gid,
                StateName = state.StateName,
                State = state.State
            };
        }
    }
}
