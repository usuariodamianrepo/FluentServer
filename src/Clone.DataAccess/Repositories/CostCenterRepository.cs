using Clone.DataAccess.Repositories.IRepository;
using Clone.Models;

namespace Clone.DataAccess.Repositories
{
    public class CostCenterRepository : ICostCenterRepository
    {
        private readonly IRepositoryAsync<CostCenter> _repository;

        public CostCenterRepository(IRepositoryAsync<CostCenter> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
    }
}
