using Clone.DataAccess.Repositories.IRepository;
using Clone.Models;
using System.Security.Claims;

namespace Clone.DataAccess.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly IRepositoryAsync<Receipt> _repository;

        public ReceiptRepository(IRepositoryAsync<Receipt> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Receipt> DeactivateAsync(int receiptId, ClaimsPrincipal? userLogued)
        {
            var receiptToDeactivate = await _repository.GetByIdAsync(receiptId);
            if (receiptToDeactivate is null)
                throw new ArgumentNullException($"Receipt Id={receiptId} does not exist. The receipt could not be deactivated.");
            if (!receiptToDeactivate.Active)
                throw new ArgumentNullException($"Receipt Id={receiptId} is not active. The receipt could not be deactivated.");

            receiptToDeactivate.UpdateAudit(userLogued);
            receiptToDeactivate.OrderId = null;
            receiptToDeactivate.Active = false;

            return receiptToDeactivate;
        }

        public async Task<Receipt> GenerateAgainstAsync(int receiptId, ClaimsPrincipal? userLogued)
        {
            var receiptFrom = await _repository.GetByIdAsync(receiptId);
            if (receiptFrom is null)
                throw new ArgumentNullException($"Receipt Id={receiptId} does not exist. The receipt could not be generated.");
            if (!receiptFrom.Active)
                throw new ArgumentNullException($"Receipt Id={receiptId} is not active. The receipt could not be generated.");

            var newReceipt = new Receipt()
            {
                Active = true,
                ApplicationUserId = receiptFrom.ApplicationUserId,
                CostCenterId = receiptFrom.CostCenterId,
                Date = DateTime.Now,
                Number = receiptFrom.Number,
                Amount = receiptFrom.Amount,
            };

            newReceipt.InsertAudit(userLogued);
            bool isIngress = receiptFrom.Amount >= 0;
            newReceipt.SetAmount(!isIngress);

            return newReceipt;
        }
    }
}
