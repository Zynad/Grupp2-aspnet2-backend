using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using Microsoft.Azure.Cosmos;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models.Entities;

namespace WebAPI.Models.Schemas
{
    public class UserCouponSchema
    {
        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public Guid CouponId { get; set; }

        public static implicit operator UserCouponEntity(UserCouponSchema schema)
        {
            return new UserCouponEntity
            {
                UserId = schema.UserId,
                CouponId = schema.CouponId,

            };
          
               
        }
    }
}
