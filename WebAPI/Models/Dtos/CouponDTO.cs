namespace WebAPI.Models.Dtos
{
    public class CouponDTO
    {
        
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public decimal DiscountAmount { get; set; } 
        //public string Products { get; set; } = null!;
        public DateTime? ExpiryDate { get; set; }
        

    }
}
