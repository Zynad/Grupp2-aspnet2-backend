using WebAPI.Contexts;
using WebAPI.Helpers.Repositories.BaseModels;
using WebAPI.Models.Entities;

namespace WebAPI.Helpers.Repositories;

public class UserProfileRepo : Repo<UserProfileEntity>
{
    public UserProfileRepo(DataContext context) : base(context)
    {
    }
}
