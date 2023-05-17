using Microsoft.AspNetCore.Identity;
using WebAPI.Helpers.Repositories;
using WebAPI.Models.Dtos;
using WebAPI.Models.Entities;
using WebAPI.Models.Schemas;

namespace WebAPI.Helpers.Services;

public class PaymentService
{
    private readonly CreditCardRepo _creditCardRepo;
    private readonly UserProfileCreditCardRepo _userProfileCreditCardRepo;
    private readonly UserManager<IdentityUser> _userManager;

    public PaymentService(CreditCardRepo creditCardRepo, UserManager<IdentityUser> userManager, UserProfileCreditCardRepo userProfileCreditCardRepo)
    {
        _creditCardRepo = creditCardRepo;
        _userManager = userManager;
        _userProfileCreditCardRepo = userProfileCreditCardRepo;
    }

    public async Task<IEnumerable<CreditCardDTO>> GetUserCreditCardsAsync(string userName)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(userName);
            var result = await _userProfileCreditCardRepo.GetListAsync(x => x.UserProfileId == user!.Id);
            if (result != null)
            {
                List<CreditCardDTO> creditCards = new List<CreditCardDTO>();
                foreach (var item in result)
                {
                    var card = await _creditCardRepo.GetAsync(x => x.Id == item.CreditCardId);
                    creditCards.Add(card);
                }
                return creditCards;
            }
        }
        catch { }
        return null!;
    }
    public async Task<bool> RegisterCreditCardsAsync(RegisterCreditCardSchema schema, string userName)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(userName);
            var existingCard = await _creditCardRepo.GetAsync(x => x.CardNo == schema.CardNo && x.NameOnCard == schema.NameOnCard && x.CVV == schema.CVV);

            if(existingCard != null)
            {
                var result = await _userProfileCreditCardRepo.AddAsync(new UserProfileCreditCardEntity { CreditCardId = existingCard.Id, CreditCard = existingCard, UserProfileId = user!.Id });
                if (result != null)
                    return true;
            }
            else
            {
                CreditCardEntity entity = schema;
                var newCard = await _creditCardRepo.AddAsync(entity);                
                if (newCard != null)
                {
                    await _userProfileCreditCardRepo.AddAsync(new UserProfileCreditCardEntity { CreditCardId = newCard.Id, CreditCard = newCard, UserProfileId = user!.Id });
                    return true;
                }
            }
        }
        catch { }
        return false!;
    }
    public async Task<bool> DeleteCreditCardsAsync(int id, string userName)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(userName);
            var result = await _userProfileCreditCardRepo.GetAsync(x => x.UserProfileId == user!.Id && x.CreditCardId == id);
            await _userProfileCreditCardRepo.DeleteAsync(result);
            return true;
        }
        catch { }
        return false!;
    }
}
