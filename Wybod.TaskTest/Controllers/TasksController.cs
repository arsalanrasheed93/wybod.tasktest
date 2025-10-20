using Microsoft.AspNetCore.Mvc;
using Wybod.TaskTest.Data.Models;
using Wybod.TaskTest.Data.Repositories;

namespace Wybod.TaskTest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _repository;

    public TasksController(ITaskRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TaskItem>> GetTasks()
    {
        return Ok(_repository.GetAll().Where(p=>p.IsActive));
    }

    [HttpGet("{id:guid}")]
    public ActionResult<TaskItem> GetTask(Guid id)
    {
        return Ok(_repository.GetById(id));
    }

    [HttpPost]
    public ActionResult<TaskItem> CreateTask([FromBody]  TaskItem task)
    {
        try
        {
            return Ok(_repository.Create(task));
        }
        catch (Exception ex)
        {
            //TODO: Log actual exception
            return BadRequest();
        }
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateTask(Guid id, [FromBody] TaskItem task)
    {
        return Ok(_repository.Update(id,task));
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteTask(Guid id)
    {
        var deleted = _repository.Delete(id);

        if (!deleted)
            return NotFound(new { message = $"Task with ID {id} not found." });

        return NoContent();
    }
}
