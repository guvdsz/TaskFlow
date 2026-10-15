using System;

namespace TaskFlow.Api.Models;

public class Project
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required DateTime CreatedAt { get; set; } = DateTime.Now;

    public User Owner { get; set; } = null!;
    public required string OwnerId { get; set; }

    public ICollection<ProjectUser> ProjectUsers { get; set; } = [];

    public ICollection<Task> Tasks { get; set; } = [];
}
