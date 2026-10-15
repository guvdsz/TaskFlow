using System;
using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.Dtos.Project;

public class AddUserDto
{
    [Required]
    public required string UserId { get; set; }
    [Required]
    public int ProjectId { get; set; }
}
