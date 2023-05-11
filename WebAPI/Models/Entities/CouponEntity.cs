using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Entities
{
    public class CouponEntity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Code { get; set; } = null!;

        [Required]
        public decimal DiscountAmount { get; set; }

        [Required]
        public DateTime? ExpriyDate { get; set; }

        public string PartitionKey { get; set; } = "Coupon";
    }
}
