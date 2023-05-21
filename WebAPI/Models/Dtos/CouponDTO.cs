using System.Security.Policy;
using WebAPI.Models.Entities;

namespace WebAPI.Models.Dtos
{
    public class CouponDTO
    {
        
        public Guid Id { get; set; }

        public string UserId { get; set; } = null!;
        public string Vouchercode{ get; set; } = null!;
        public decimal DiscountAmount { get; set; } 
        
        public DateTime? ExpiryDate { get; set; }
        
        public static implicit operator CouponDTO(CouponEntity couponEntity)
        {
            return new CouponDTO
            {
                
                Id = couponEntity.Id,
                UserId = couponEntity.UserId,
                Vouchercode= couponEntity.VoucherCode,
                DiscountAmount = couponEntity.DiscountAmount,
                ExpiryDate = couponEntity.ExpiryDate
            };
        }
    }
}
