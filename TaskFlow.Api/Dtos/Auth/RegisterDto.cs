using System;
using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.Dtos;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public required string Password { get; set; }
}
