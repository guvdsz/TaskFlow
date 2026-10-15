using System;

namespace TaskFlow.Api.Models;

public class Task
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required TaskStatus Status { get; set; }
    public DateOnly DueDate { get; set; }

    public Project Project { get; set; } = null!;
    public int ProjectId { get; set; }

    public ICollection<TaskUser> TaskUsers { get; set; } = [];

    public Tag? Tag { get; set; } = null!;
    public int? TagId { get; set; }
}
