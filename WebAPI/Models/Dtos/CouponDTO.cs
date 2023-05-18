using System.Security.Policy;
using WebAPI.Models.Entities;

namespace WebAPI.Models.Dtos
{
    public class CouponDTO
    {
        
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public decimal DiscountAmount { get; set; } 
        
        public DateTime? ExpiryDate { get; set; }
        
        public static implicit operator CouponDTO(CouponEntity couponEntity)
        {
            return new CouponDTO
            {
                Id = couponEntity.Id,
                Code = couponEntity.Code,
                DiscountAmount = couponEntity.DiscountAmount,
                ExpiryDate = couponEntity.ExpiryDate
            };
        }
    }
}
