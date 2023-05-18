using System.ComponentModel.DataAnnotations;
using WebAPI.Models.Entities;

namespace WebAPI.Models.Schemas
{
    public class CouponSchema
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string VoucherCode { get; set; } = null!;
        [Required]
        public decimal DiscountAmount { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }




        public static implicit operator CouponEntity(CouponSchema schema)
        {
            return new CouponEntity { Title = schema.Title ,VoucherCode = schema.VoucherCode, DiscountAmount = schema.DiscountAmount,ExpiryDate = schema.ExpiryDate  };
        }
    }

   
}
