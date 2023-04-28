using Microsoft.AspNetCore.Identity;
using WebAPI.Contexts;

namespace WebAPI.Helpers.Repositories;

public class IdentityUserRepo : Repo<IdentityUser>
{
    public IdentityUserRepo(DataContext context) : base(context)
    {
    }
}
