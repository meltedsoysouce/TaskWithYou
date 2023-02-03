using System.ComponentModel.DataAnnotations;

namespace DBKernel.Repositories
{
    public interface IRepositoryBase<T>
    {
        T[] GetAll();

        T? GetByGid(Guid pGid);
    }
}
