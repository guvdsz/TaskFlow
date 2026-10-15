using System;
using Microsoft.AspNetCore.Identity;

namespace TaskFlow.Api.Models;

public class User : IdentityUser
{
    public ICollection<ProjectUser> ProjectUsers { get; set; } = [];
    public ICollection<TaskUser> TaskUsers { get; set; } = [];
}
