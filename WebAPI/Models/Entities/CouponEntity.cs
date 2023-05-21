using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Entities
{
    public class CouponEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string VoucherCode { get; set; } = null!;
        public decimal DiscountAmount { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string PartitionKey { get; set; } = "Coupon";
    }
}
