using WebAPI.Contexts;
using WebAPI.Models.Entities;

namespace WebAPI.Helpers.Repositories;

public class TagRepo : Repo<TagEntity>
{
    public TagRepo(DataContext dataContext) : base(dataContext)
    {
    }
}
