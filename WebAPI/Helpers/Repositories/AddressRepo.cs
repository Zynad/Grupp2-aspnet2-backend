using WebAPI.Contexts;
using WebAPI.Helpers.Repositories.BaseModels;
using WebAPI.Models.Entities;

namespace WebAPI.Helpers.Repositories;

public class AddressRepo : Repo<AddressEntity>
{
    public AddressRepo(DataContext context) : base(context)
    {
    }
}
