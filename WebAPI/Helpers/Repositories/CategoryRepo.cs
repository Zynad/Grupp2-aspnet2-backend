using WebAPI.Contexts;
using WebAPI.Helpers.Repositories.BaseModels;
using WebAPI.Models.Entities;

namespace WebAPI.Helpers.Repositories;

public class CategoryRepo : CosmosRepo<CategoryEntity>
{
    public CategoryRepo(CosmosContext context) : base(context)
    {
    }
}