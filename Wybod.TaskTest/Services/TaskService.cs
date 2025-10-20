using Wybod.TaskTest.Data.Models;
using Wybod.TaskTest.Data.Repositories;
using Wybod.TaskTest.DTOs;
using Wybod.TaskTest.Services.Interfaces;

namespace Wybod.TaskTest.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<TaskItem> GetAllTasks() =>
        _repository.GetAll().Where(t => t.IsActive);

    public TaskItem? GetTaskById(Guid id) =>
        _repository.GetById(id);

    public TaskItem CreateTask(TaskCreateDto dto)
    {
        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            IsCompleted = false
        };

        return _repository.Create(task);
    }

    public TaskItem? UpdateTask(Guid id, TaskUpdateDto dto)
    {
        var existing = _repository.GetById(id);
        if (existing == null) return null;

        existing.Title = dto.Title;
        existing.Description = dto.Description;
        existing.IsCompleted = dto.IsCompleted;
        existing.CompletedAt = dto.CompletedAt;
        existing.Priority = dto.Priority;

        _repository.Update(id, existing);
        return existing;
    }

    public bool DeleteTask(Guid id) =>
        _repository.Delete(id);


}