using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Dtos.Project;
using TaskFlow.Api.Interfaces.Project;
using TaskFlow.Api.Mappers;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Controllers
{
    [Authorize]
    [Route("api/projects")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly UserManager<User> _userManager;

        public ProjectController(
            IProjectRepository projectRepository,
            UserManager<User> userManager
        )
        {
            _projectRepository = projectRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userId is null)
            {
                return Unauthorized();
            }

            var projects = await _projectRepository.GetByOwnerIdAsync(userId);

            var result = projects.Select(p => p.ToProjectDto());

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userId is null)
            {
                return Unauthorized();
            }

            var project = await _projectRepository.GetByIdAsync(id, userId);

            if (project is null)
            {
                return NotFound("The project does not exist or the user does not have access to it.");
            }

            var result = project.ToProjectDto();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectDto createProjectDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userId is null)
            {
                return Unauthorized();
            }

            var project = createProjectDto.ToProject(userId);
            var createdProject = await _projectRepository.CreateAsync(project);

            return Ok(createdProject.ToProjectDto());
        }

        [HttpPost("addUser")]
        public async Task<IActionResult> AddUser(AddUserDto addUserDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userId is null)
            {
                return Unauthorized();
            }

            var userExists = await _userManager.FindByIdAsync(addUserDto.UserId);

            if (userExists is null)
            {
                return NotFound("The user to add to the project was not found.");
            }

            var project = await _projectRepository.GetByIdAsync(addUserDto.ProjectId, userId);

            if (project is null)
            {
                return NotFound("The project does not exist or the user does not have access to it.");
            }

            if (project.OwnerId != userId)
            {
                return Unauthorized("Only the owner of the project can add users to it");
            }

            var userIsAlreadyInTheProject = await _projectRepository.UserIsInTheProject(addUserDto.UserId, addUserDto.ProjectId);

            if (userIsAlreadyInTheProject)
            {
                return Conflict("The user to add to the project is already on it");
            }

            var projectUser = new ProjectUser
            {
                ProjectId = addUserDto.ProjectId,
                UserId = addUserDto.UserId
            };

            await _projectRepository.AddUserToProjectAsync(projectUser);

            return Ok("User added to the project.");
        }

        [HttpPost("removeUser")]
        public async Task<IActionResult> RemoveUser(RemoveUserDto removeUserDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (userId is null)
            {
                return Unauthorized();
            }

            var userExists = await _userManager.FindByIdAsync(removeUserDto.UserId);

            if (userExists is null)
            {
                return NotFound("The user to add to the project was not found.");
            }

            var project = await _projectRepository.GetByIdAsync(removeUserDto.ProjectId, userId);

            if (project is null)
            {
                return NotFound("The project does not exist or the user does not have access to it.");
            }

            if (project.OwnerId != userId)
            {
                return Unauthorized("Only the owner of the project can remove users from it");
            }

            var UserIsInTheProject = await _projectRepository.UserIsInTheProject(removeUserDto.UserId, removeUserDto.ProjectId);

            if (!UserIsInTheProject)
            {
                return Conflict("The user is not in the project.");
            }

            await _projectRepository.RemoveUserFromProjectAsync(
                removeUserDto.UserId,
                removeUserDto.ProjectId);

            return Ok("User removed from the project.");
        }

    }
}
