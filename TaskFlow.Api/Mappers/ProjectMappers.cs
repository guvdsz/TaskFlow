using TaskFlow.Api.Dtos.Project;
using ProjectModel = TaskFlow.Api.Models.Project;

namespace TaskFlow.Api.Mappers;

public static class ProjectMappers
{
    public static ProjectModel ToProject(
        this CreateProjectDto projectDto,
        string ownerId)
    {
        return new ProjectModel
        {
            Name = projectDto.Name,
            Description = projectDto.Description,
            CreatedAt = DateTime.UtcNow,
            OwnerId = ownerId
        };
    }

    public static ProjectDto ToProjectDto(this ProjectModel project)
    {
        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            CreatedAt = project.CreatedAt,
            OwnerId = project.OwnerId,
            Users = project.ProjectUsers.Select(u => u.UserId).ToList()
        };
    }
}