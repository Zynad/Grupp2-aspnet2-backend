using System.ComponentModel.DataAnnotations;
using WebAPI.Models.Entities;

namespace WebAPI.Models.Schemas;

public class RegisterAddressSchema
{
    [Required]
    [MinLength(2)]
    public string Title { get; set; } = null!;

    [Required]
    [MinLength(2)]
    public string StreetName { get; set; } = null!;

    [Required]
    public string PostalCode { get; set; } = null!;

    [Required]
    [MinLength(2)]
    public string City { get; set; } = null!;

    [Required]
    [MinLength(2)]
    public string Country { get; set; } = null!;


    public static implicit operator AddressEntity(RegisterAddressSchema schema)
    {
        return new AddressEntity
        {
            StreetName = schema.StreetName,
            PostalCode = schema.PostalCode,
            City = schema.City,
            Country = schema.Country,
        };
    }
}
