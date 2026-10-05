using Clone.Models;

namespace Clone.DataAccess.Repositories.IRepository
{
    public interface IUnitOfWork
    {
        IRepositoryAsync<T> Repository<T>() where T : AuditableEntity;

        Task SaveAsync();

    }
}
