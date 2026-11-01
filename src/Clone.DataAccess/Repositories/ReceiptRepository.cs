using Clone.DataAccess.Repositories.IRepository;
using Clone.Models;

namespace Clone.DataAccess.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly IRepositoryAsync<Receipt> _repository;

        public ReceiptRepository(IRepositoryAsync<Receipt> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
    }
}
