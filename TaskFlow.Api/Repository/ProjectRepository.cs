using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.Interfaces.Project;
using TaskFlow.Api.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskFlow.Api.Repository;

public class ProjectRepository(ApplicationDbContext context) : IProjectRepository
{
    public async Task<Project> CreateAsync(
        Project project,
        CancellationToken cancellationToken = default
    )
    {
        await context.Projects.AddAsync(project, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return project;
    }

    public Task<Project?> GetByIdAsync(
        int id,
        string userId,
        CancellationToken cancellationToken = default
    )
    {
        return context.Projects
            .AsNoTracking()
            .Include(s => s.ProjectUsers)
            .FirstOrDefaultAsync(
                project => project.Id == id
                    && (project.OwnerId == userId
                        || project.ProjectUsers.Any(projectUser => projectUser.UserId == userId)),
                cancellationToken);
    }

    public async Task<IReadOnlyList<Project>> GetByOwnerIdAsync(
        string ownerId,
        CancellationToken cancellationToken = default
    )
    {
        return await context.Projects
            .AsNoTracking()
            .Where(project => project.OwnerId == ownerId)
            .Include(s => s.ProjectUsers)
            .ToListAsync(cancellationToken);
    }

    public async Task AddUserToProjectAsync(
        ProjectUser projectUser,
        CancellationToken cancellationToken = default)
    {
        await context.ProjectUsers.AddAsync(projectUser, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> UserIsInTheProject(
        string userId,
        int projectId,
        CancellationToken cancellationToken = default)
    {
        return await context.Projects.AnyAsync(
            project => project.Id == projectId
                && (project.OwnerId == userId
                    || project.ProjectUsers.Any(projectUser => projectUser.UserId == userId)),
            cancellationToken);
    }

    public async Task RemoveUserFromProjectAsync(
        string userId,
        int projectId,
        CancellationToken cancellationToken = default)
    {
        await context.ProjectUsers
            .Where(projectUser =>
                projectUser.UserId == userId
                && projectUser.ProjectId == projectId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}