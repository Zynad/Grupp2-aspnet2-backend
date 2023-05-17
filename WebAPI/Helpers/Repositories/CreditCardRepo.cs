using WebAPI.Contexts;
using WebAPI.Helpers.Repositories.BaseModels;
using WebAPI.Models.Entities;

namespace WebAPI.Helpers.Repositories;

public class CreditCardRepo : Repo<CreditCardEntity>
{
    public CreditCardRepo(DataContext context) : base(context)
    {
    }
}
