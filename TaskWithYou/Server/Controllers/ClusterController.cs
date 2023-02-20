using DBKernel.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClientSide = TaskWithYou.Shared.Model;

namespace TaskWithYou.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClusterController : ControllerBase
    {
        private readonly IClusterRepository _ClusterRepository;

        public ClusterController(IClusterRepository clusterRepository) 
        {
            _ClusterRepository = clusterRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ClientSide.Cluster[]>> GetAll()
        {
            return _ClusterRepository
                .GetAll()
                .Select(a =>
                {
                    return new ClientSide.Cluster()
                    {
                        Gid = a.Gid,
                        Name = a.Name,
                        Detail = a.Detail
                    };
                })
                .ToArray();
        }

        [HttpGet("{pGid}")]
        public async Task<ActionResult<ClientSide.Cluster>> GetCluster(Guid pGid)
        {
            var entity = _ClusterRepository.GetByGid(pGid);

            if (entity == null)
                return NotFound();

            return new ClientSide.Cluster()
            {
                Gid = entity.Gid,
                Name = entity.Name,
                Detail = entity.Detail
            };
        }

        [HttpPost]
        public async Task Add(ClientSide.Cluster pCluster)
        {
            _ClusterRepository.Add(pCluster.Name, pCluster.Detail);
        }

        [HttpPut]
        public async Task Edit(ClientSide.Cluster pCluster)
        {
            _ClusterRepository.Edit(pCluster.Gid,
                pCluster.Name,
                pCluster.Detail);
        }

        [HttpDelete("{pGid}")]
        public async Task Delete(Guid pGid)
        {
            _ClusterRepository.Delete(pGid);
        }
    }
}
