
using System.Security.Policy;
using WebAPI.Models.Entities;

namespace WebAPI.Models.Dtos

{
    public class CouponDTO
    {
        
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Vouchercode{ get; set; } = null!;
        public decimal DiscountAmount { get; set; } 
        
        public DateTime? ExpiryDate { get; set; }
        
        public static implicit operator CouponDTO(CouponEntity couponEntity)
        {
            return new CouponDTO
            {
                
                Id = couponEntity.Id,
                Title = couponEntity.Title,
                Vouchercode= couponEntity.VoucherCode,
                DiscountAmount = couponEntity.DiscountAmount,
                ExpiryDate = couponEntity.ExpiryDate
            };
        }

    }
}
