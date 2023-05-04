using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Dtos;
    public class UserProfileDTO
    {
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? ImageUrl { get; set; }
    public string? Location { get; set; }
}

