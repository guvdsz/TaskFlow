using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.Dtos.Project;

public class CreateProjectDto
{
    [Required]
    [MinLength(3), MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [MinLength(3), MaxLength(500)]
    public required string Description { get; set; }
}