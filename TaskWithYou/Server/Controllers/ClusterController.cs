using DBKernel.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FrontEnd = TaskWithYou.Shared.Model;

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
        public async Task<ActionResult<FrontEnd.Cluster[]>> GetAll()
        {
            return _ClusterRepository
                .GetAll()
                .Select(a =>
                {
                    return new FrontEnd.Cluster()
                    {
                        Gid = a.Gid,
                        Name = a.Name,
                        Detail = a.Detail
                    };
                })
                .ToArray();
        }

        [HttpPost]
        public async Task Add(string pName, string pDetail)
        {
            _ClusterRepository.Add(pName, pDetail);
        }

        [HttpPut]
        public async Task Edit(Guid pGid, string pName, string pDetail)
        {
            _ClusterRepository.Edit(pGid, pName, pDetail);
        }

        [HttpDelete]
        public async Task Delete(Guid pGid)
        {
            _ClusterRepository.Delete(pGid);
        }
    }
}
