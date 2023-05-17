using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Entities;

public class AddressEntity
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string StreetName { get; set; } = null!;

    [Required]
    public string PostalCode { get; set; } = null!;

    [Required]
    public string City { get; set; } = null!;

    [Required]
    public string Country { get; set; } = null!;

}
