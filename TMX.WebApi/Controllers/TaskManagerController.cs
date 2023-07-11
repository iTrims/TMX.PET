using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMX.TaskService.Application.Common.Models;
using TMX.TaskService.WebApi.Services;

namespace TMX.TaskService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskManagerController : ControllerBase
    {
        private readonly ITaskManagerService _service;

        public TaskManagerController(ITaskManagerService taskManagerService) => 
            _service = taskManagerService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTaskDto>>> GetAll()
        {
            var UserTask = await _service.GetUserTasks();
            return Ok(UserTask);
        }

        [HttpGet("{TaskId}")]
        public async Task<ActionResult<UserTaskDto>> GetById([FromRoute] Guid TaskId)
        {
            var userTask = await _service.GetUserTaskByTaskId(TaskId);
            return Ok(userTask);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateUserTaskDto userTaskDto, 
            CancellationToken cancellationToken)
        {
            var userTask = await _service.Create(userTaskDto, cancellationToken);
            return Created($"api/taskmanager/{userTask.Id}", null);
        }

        [HttpPut("{TaskId}")]
        public async Task<ActionResult> Update([FromBody] UpdateUserTaskDto updateDto,
            [FromRoute] Guid TaskId, CancellationToken cancellationToken)
        {
            await _service.Update(updateDto, TaskId, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{TaskId}")]
        public async Task<ActionResult> Delete([FromRoute] Guid TaskId, 
            CancellationToken cancellationToken)
        {
            await _service.Delete(TaskId, cancellationToken);
            return NoContent();
        }
    }
}
