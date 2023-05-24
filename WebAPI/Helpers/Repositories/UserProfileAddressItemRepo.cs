using WebAPI.Contexts;
using WebAPI.Helpers.Repositories.BaseModels;
using WebAPI.Models.Entities;

namespace WebAPI.Helpers.Repositories;

public class UserProfileAddressItemRepo : Repo<UserProfileAddressItemEntity>
{
    public UserProfileAddressItemRepo(DataContext context) : base(context)
    {
    }
}
