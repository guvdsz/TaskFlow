using System;

namespace TaskFlow.Api.Models;

public class TaskStatus
{
    public int Id { get; set; }
    public required string Name { get; set; }
}
