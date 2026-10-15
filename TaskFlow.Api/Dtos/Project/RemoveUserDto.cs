using System;
using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.Dtos.Project;

public class RemoveUserDto
{
    [Required]
    public required string UserId { get; set; }
    [Required]
    public int ProjectId { get; set; }
}
