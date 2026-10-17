using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.Dtos.Shared;
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

    public async Task<PaginatedResponseDto<Project>> GetByOwnerIdAsync(
        string ownerId,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default
    )
    {
        var query = context.Projects
            .AsNoTracking()
            .Where(project => project.OwnerId == ownerId);

        int totalItems = await query.CountAsync(cancellationToken);

        int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        var offset = (pageNumber - 1) * pageSize;

        var projects = await query
            .Include(s => s.ProjectUsers)
            .OrderBy(p => p.Id)
            .Skip(offset)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResponseDto<Project>
        {
            Data = projects,
            CurrentPage = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };
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