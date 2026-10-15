namespace TaskFlow.Api.Dtos.Project;

public class ProjectDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string OwnerId { get; set; }
    public List<string> Users { get; set; } = [];
}
