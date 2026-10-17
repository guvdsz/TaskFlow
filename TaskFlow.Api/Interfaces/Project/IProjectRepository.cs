using System.Runtime.CompilerServices;
using TaskFlow.Api.Dtos.Shared;
using TaskFlow.Api.Models;
using ProjectModel = TaskFlow.Api.Models.Project;
using Task = System.Threading.Tasks.Task;

namespace TaskFlow.Api.Interfaces.Project;

public interface IProjectRepository
{
    Task<ProjectModel> CreateAsync(
        ProjectModel project,
        CancellationToken cancellationToken = default);

    Task<ProjectModel?> GetByIdAsync(
        int id,
        string userId,
        CancellationToken cancellationToken = default);

    Task AddUserToProjectAsync(
        ProjectUser projectUser,
        CancellationToken cancellationToken = default);

    Task RemoveUserFromProjectAsync(
        string userId,
        int projectId,
        CancellationToken cancellationToken = default);

    Task<bool> UserIsInTheProject(
    string userId,
    int projectId,
    CancellationToken cancellationToken = default);

    Task<PaginatedResponseDto<ProjectModel>> GetByOwnerIdAsync(
        string ownerId,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default);
}