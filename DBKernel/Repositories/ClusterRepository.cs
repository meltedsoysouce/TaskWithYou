using DBKernel.Entity;
using Microsoft.EntityFrameworkCore;

namespace DBKernel.Repositories
{
    public interface IClusterRepository : IRepositoryBase<Cluster>
    {
        void Add(string pName, string pDetail);

        void Edit(Guid pGid ,string pName, string pDetail);

        void Delete(Guid pGid);
    }

    public class ClusterRepository : IClusterRepository
    {
        private readonly AppDbContext _DbContext;

        public ClusterRepository(AppDbContext context) 
        {
            _DbContext = context;
        }

        public Cluster[] GetAll()
        {
            return _DbContext.Clusters.AsNoTracking().ToArray();
        }

        public Cluster? GetByGid(Guid pGid)
        {
            return _DbContext
                .Clusters
                .AsNoTracking()
                .FirstOrDefault(a => a.Gid == pGid);
        }

        public void Add(string pName, string pDetail)
        {
            Cluster cluster = new Cluster()
            { 
                Gid = Guid.NewGuid(),
                Name = pName,
                Detail = pDetail
            };

            _DbContext.Clusters.Add(cluster);
            _DbContext.SaveChanges();
        }

        public void Edit(Guid pGid, string pName, string pDetail)
        {
            var cluster = _DbContext
                .Clusters
                .FirstOrDefault(a => a.Gid == pGid);

            if (cluster == null)
                return;

            cluster.Name = pName;
            cluster.Detail = pDetail;
            _DbContext.SaveChanges();
        }

        public void Delete(Guid pGid) 
        {
            var cluster = _DbContext.Clusters.FirstOrDefault(a => a.Gid == pGid);
            _DbContext.Clusters.Remove(cluster);
            _DbContext.SaveChanges();
        }
    }
}
