using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Entities
{
    public class CouponEntity
    {
        [Key]
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Code { get; set; } = null!;

        [Required]
        public decimal DiscountAmount { get; set; }

        [Required]
        public DateTime? ExpiryDate { get; set; }

        public string PartitionKey { get; set; } = "Coupon";
    }
}
