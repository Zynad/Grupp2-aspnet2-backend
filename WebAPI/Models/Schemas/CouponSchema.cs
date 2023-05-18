using WebAPI.Models.Entities;

namespace WebAPI.Models.Schemas
{
    public class CouponSchema
    {
       

        public string Code { get; set; } = null!;
        public decimal DiscountAmount { get; set; }
        //public string Products { get; set; } = null!;
        public DateTime? ExpiryDate { get; set; }




        public static implicit operator CouponEntity(CouponSchema schema)
        {
            return new CouponEntity { Code = schema.Code, DiscountAmount = schema.DiscountAmount, };
        }
    }

   
}
