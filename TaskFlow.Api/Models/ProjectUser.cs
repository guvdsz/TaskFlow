using System;

namespace TaskFlow.Api.Models;

public class ProjectUser
{
    public User User { get; set; } = null!;
    public required string UserId { get; set; }

    public Project Project { get; set; } = null!;
    public int ProjectId { get; set; }
}
