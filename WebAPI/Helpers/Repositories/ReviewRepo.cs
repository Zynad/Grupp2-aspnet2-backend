using WebAPI.Contexts;
using WebAPI.Helpers.Repositories.BaseModels;
using WebAPI.Models.Entities;

namespace WebAPI.Helpers.Repositories;

public class ReviewRepo : CosmosRepo<ReviewEntity>
{
    public ReviewRepo(CosmosContext context) : base(context)
    {
    }
}