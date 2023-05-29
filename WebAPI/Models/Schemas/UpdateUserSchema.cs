using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models.Entities;

namespace WebAPI.Models.Schemas;

public class UpdateUserSchema
{
    [Required]
    [MinLength(2)]
    public string FirstName { get; set; } = null!;
    [Required]
    [MinLength(2)]
    public string LastName { get; set; } = null!;
    [Required]
    [MinLength(6)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    public string Email { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public static implicit operator IdentityUser(UpdateUserSchema schema)
    {
            return new IdentityUser
            {
                UserName = schema.Email,
                Email = schema.Email,
            };
    }

    public static implicit operator UserProfileEntity(UpdateUserSchema schema)
    {
        if (schema.ImageUrl == null || schema.ImageUrl == "")
        {
            return new UserProfileEntity
            {
                FirstName = schema.FirstName,
                LastName = schema.LastName,
            };
        }
        else
        {
            return new UserProfileEntity
            {
                FirstName = schema.FirstName,
                LastName = schema.LastName,
                ImageUrl = schema.ImageUrl,
            };
        }
    }
}
