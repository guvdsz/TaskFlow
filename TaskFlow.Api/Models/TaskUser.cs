using System;

namespace TaskFlow.Api.Models;

public class TaskUser
{
    public User User { get; set; } = null!;
    public required string UserId { get; set; }

    public Task Task { get; set; } = null!;
    public int TaskId { get; set; }
}
