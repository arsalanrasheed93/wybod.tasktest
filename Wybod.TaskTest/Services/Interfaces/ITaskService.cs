using Wybod.TaskTest.Data.Models;
using Wybod.TaskTest.DTOs;

namespace Wybod.TaskTest.Services.Interfaces
{
    public interface ITaskService
    {
        IEnumerable<TaskItem> GetAllTasks();
        TaskItem? GetTaskById(Guid id);
        TaskItem CreateTask(TaskCreateDto dto);
        TaskItem? UpdateTask(Guid id, TaskUpdateDto dto);
        bool DeleteTask(Guid id);
    }
}
