using WebAPI.Contexts;
using WebAPI.Models.Entities;

namespace WebAPI.Helpers.Repositories;

public class CategoryRepo : Repo<CategoryEntity>
{
    public CategoryRepo(DataContext dataContext) : base(dataContext)
    {
    }
}