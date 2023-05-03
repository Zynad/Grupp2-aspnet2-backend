﻿using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Schemas;

public class LoginAccountSchema
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    public bool RememberMe { get; set; } 
}
