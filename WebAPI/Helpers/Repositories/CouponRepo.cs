using WebAPI.Contexts;
using WebAPI.Helpers.Repositories.BaseModels;
using WebAPI.Models.Entities;

namespace WebAPI.Helpers.Repositories
{
    public class CouponRepo : CosmosRepo<CouponEntity>
    {
        public CouponRepo(CosmosContext context) : base(context)
        {
        }

        internal Task DeleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
