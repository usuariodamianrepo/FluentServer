using System.Security.Claims;
using Clone.Models;

namespace Clone.DataAccess.Repositories.IRepository
{
	public interface IReceiptRepository 
	{
        Task<Receipt> DeactivateAsync(int receiptId, ClaimsPrincipal? userLogued);

        Task<Receipt> GenerateAgainstAsync(int receiptId, ClaimsPrincipal? userLogued, decimal amount = 0);
    }
}
