using Microsoft.AspNetCore.Mvc;
using Wybod.TaskTest.Data.Models;
using Wybod.TaskTest.Data.Repositories;
using Wybod.TaskTest.DTOs;
using Wybod.TaskTest.Services.Interfaces;

namespace Wybod.TaskTest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public IActionResult GetTasks()
    {
       return Ok(_taskService.GetAllTasks());
    }

        [HttpGet("{id:guid}")]
    public IActionResult GetTask(Guid id)
    {
        var task = _taskService.GetTaskById(id);
        return task is null ? NotFound() : Ok(task);
    }

    [HttpPost]
    public IActionResult CreateTask([FromBody] TaskCreateDto dto)
    {
        var task = _taskService.CreateTask(dto);
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateTask(Guid id, [FromBody] TaskUpdateDto dto)
    {
        var updated = _taskService.UpdateTask(id, dto);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteTask(Guid id)
    {
        var deleted = _taskService.DeleteTask(id);
        return deleted ? NoContent() : NotFound();
    }
}
