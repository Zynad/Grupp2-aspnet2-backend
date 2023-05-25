using System.Security.Policy;
using WebAPI.Models.Entities;

namespace WebAPI.Models.Dtos

{
    public class CouponDTO
    {   
        public string Title { get; set; } = null!;
        public decimal DiscountAmount { get; set; } 
        
        public DateTime? ExpiryDate { get; set; }
        
        public static implicit operator CouponDTO(CouponEntity couponEntity)
        {
            return new CouponDTO
            { 
                Title = couponEntity.Title,
                DiscountAmount = couponEntity.DiscountAmount,
                ExpiryDate = couponEntity.ExpiryDate
            };
        }

    }
}
