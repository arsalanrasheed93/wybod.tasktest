using Moq;
using Wybod.TaskTest.Data.Models;
using Wybod.TaskTest.Data.Repositories;
using Wybod.TaskTest.DTOs;
using Wybod.TaskTest.Services;

namespace Wybod.TaskTest.UnitTests;

public class TaskServiceTests
{  
        private readonly Mock<ITaskRepository> _mockRepo;
        private readonly TaskService _taskService;
        private readonly List<TaskItem> _tasks;

        public TaskServiceTests()
        {
            _tasks = new List<TaskItem>
                {
                    new TaskItem { Id = Guid.NewGuid(), Title = "Task 1", Description = "Description 1", IsActive = true, IsCompleted = false },
                    new TaskItem { Id = Guid.NewGuid(), Title = "Task 2", Description = "Description 2", IsActive = true, IsCompleted = true },
                    new TaskItem { Id = Guid.NewGuid(), Title = "Task 3", Description = "Description 3", IsActive = false, IsCompleted = false }
                };

            _mockRepo = new Mock<ITaskRepository>();
            _mockRepo.Setup(r => r.GetAll()).Returns(_tasks);
            _mockRepo.Setup(r => r.GetById(It.IsAny<Guid>())).Returns((Guid id) => _tasks.FirstOrDefault(t => t.Id == id));
            _mockRepo.Setup(r => r.Create(It.IsAny<TaskItem>())).Returns((TaskItem t) =>
            {
                _tasks.Add(t);
                return t;
            });
            _mockRepo.Setup(r => r.Update(It.IsAny<Guid>(), It.IsAny<TaskItem>())).Returns((Guid id, TaskItem t) =>
            {
                var existing = _tasks.FirstOrDefault(x => x.Id == id);
                if (existing == null) return false;
                existing.Title = t.Title;
                existing.Description = t.Description;
                existing.IsCompleted = t.IsCompleted;
                return true;
            });
            _mockRepo.Setup(r => r.Delete(It.IsAny<Guid>())).Returns((Guid id) =>
            {
                var existing = _tasks.FirstOrDefault(x => x.Id == id);
                if (existing == null) return false;
                existing.IsActive = false;
                return true;
            });

            _taskService = new TaskService(_mockRepo.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnOnlyActiveTasks()
        {
            var result = _taskService.GetAllTasks();
            Assert.Equal(2, result.Count());
            Assert.All(result, t => Assert.True(t.IsActive));
        }

        [Fact]
        public void GetById_ShouldReturnCorrectTask()
        {
            var target = _tasks[0];
            var result = _taskService.GetTaskById(target.Id);
            Assert.NotNull(result);
            Assert.Equal(target.Id, result.Id);
        }

        [Fact]
        public void CreateTask_ShouldAddNewTask()
        {
            var dto = new TaskCreateDto { Title = "New Task", Description = "New Description test" };
            var result = _taskService.CreateTask(dto);
            Assert.Contains(result, _tasks);
            Assert.Equal("New Task", result.Title);
        }

        [Fact]
        public void UpdateTask_ShouldModifyExistingTask()
        {
            var target = _tasks[0];
            var dto = new TaskUpdateDto
            {
                Title = "Updated",
                Description = "Updated Description test",
                IsCompleted = true,
                CompletedAt = DateTime.UtcNow
            };

            var result = _taskService.UpdateTask(target.Id, dto);
            Assert.NotNull(result);
            Assert.True(target.IsCompleted);
        }

        [Fact]
        public void UpdateTask_NonExistentTask_ShouldReturnNull()
        {
            var dto = new TaskUpdateDto { Title = "X", Description = "Y", IsCompleted = false };
            var result = _taskService.UpdateTask(Guid.NewGuid(), dto);
            Assert.Null(result);
        }

        [Fact]
        public void DeleteTask_ShouldMarkTaskInactive()
        {
            var target = _tasks[0];
            var deleted = _taskService.DeleteTask(target.Id);
            Assert.True(deleted);
            Assert.False(target.IsActive);
        }

        [Fact]
        public void DeleteTask_NonExistentTask_ShouldReturnFalse()
        {
            var deleted = _taskService.DeleteTask(Guid.NewGuid());
            Assert.False(deleted);
        }

        [Fact]
        public void CreateTask_VerifyRepositoryCalledOnce()
        {
            var mockRepo = new Mock<ITaskRepository>();
            var service = new TaskService(mockRepo.Object);

            var dto = new TaskCreateDto { Title = "Test", Description = "Task description" };
            service.CreateTask(dto);

            mockRepo.Verify(r => r.Create(It.IsAny<TaskItem>()), Times.Once);
        }
}