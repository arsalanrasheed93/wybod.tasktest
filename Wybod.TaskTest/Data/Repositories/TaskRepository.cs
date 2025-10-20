using Wybod.TaskTest.Data.Models;

namespace Wybod.TaskTest.Data.Repositories;

public interface ITaskRepository
{
    IEnumerable<TaskItem> GetAll();
    TaskItem? GetById(Guid id);
    TaskItem Create(TaskItem task);
    bool Update(Guid id, TaskItem task);
    bool Delete(Guid id);
}

public class TaskRepository : ITaskRepository
{
    private readonly IDataContext _dataContext;

    public TaskRepository(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public IEnumerable<TaskItem> GetAll()
    {
        return _dataContext.Tasks;
    }

    public TaskItem? GetById(Guid id)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }

    public TaskItem Create(TaskItem task)
    {
        _dataContext.Tasks.Add(task);
        return task;
    }

    public bool Update(Guid  id, TaskItem task)
    {
        var taskData = _dataContext.Tasks.FirstOrDefault(t => t.Id == id);
        if (taskData == null)
        {
            return false;
        }
        taskData.IsCompleted = task.IsCompleted;
        if(task.IsCompleted)
        taskData.CompletedAt = DateTime.Now;
        return true;
    }

    public bool Delete(Guid id)
    {
        var task = _dataContext.Tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
        {
            return false; 
        }
        task.IsActive = false;
        return true; 
    }
}