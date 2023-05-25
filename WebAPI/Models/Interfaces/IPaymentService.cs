using WebAPI.Models.Dtos;
using WebAPI.Models.Schemas;

namespace WebAPI.Models.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> DeleteCreditCardsAsync(int id, string userName);
        Task<IEnumerable<CreditCardDTO>> GetUserCreditCardsAsync(string userName);
        Task<bool> RegisterCreditCardsAsync(RegisterCreditCardSchema schema, string userName);
    }
}