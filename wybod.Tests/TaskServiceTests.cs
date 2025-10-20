using Moq;
using Wybod.TaskTest.Data.Models;
using Wybod.TaskTest.Data.Repositories;
using Wybod.TaskTest.DTOs;
using Wybod.TaskTest.Services;
using Wybod.TaskTest.Services.Interfaces;

namespace wybod.Tests
{
    [TestFixture]
    public class TaskServiceTests
    {
        private Mock<ITaskRepository> _mockRepo = null!;
        private ITaskService _taskService = null!;
        private List<TaskItem> _tasks = null!;

        [SetUp]
        public void Setup()
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

        [Test]
        public void GetAll_ShouldReturnOnlyActiveTasks()
        {
            var result = _taskService.GetAllTasks();
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.IsTrue(result.All(t => t.IsActive));
        }

        [Test]
        public void GetById_ShouldReturnCorrectTask()
        {
            var target = _tasks[0];
            var result = _taskService.GetTaskById(target.Id);
            Assert.IsNotNull(result);
            Assert.That(result!.Id, Is.EqualTo(target.Id));
        }

        [Test]
        public void CreateTask_ShouldAddNewTask()
        {
            var dto = new TaskCreateDto { Title = "New Task", Description = "New Description" };
            var result = _taskService.CreateTask(dto);
            Assert.Contains(result, _tasks);
            Assert.That(result.Title, Is.EqualTo("New Task"));
        }

        [Test]
        public void UpdateTask_ShouldModifyExistingTask()
        {
            var target = _tasks[0];
            var dto = new TaskUpdateDto
            {
                Title = "Updated",
                Description = "Updated Description test",
                IsCompleted = true,
                CompletedAt = DateTime.Now
            };

            var result = _taskService.UpdateTask(target.Id, dto);
            Assert.IsNotNull(result);
            Assert.IsTrue(target.IsCompleted);
        }

        [Test]
        public void UpdateTask_NonExistentTask_ShouldReturnNull()
        {
            var dto = new TaskUpdateDto { Title = "X", Description = "Y", IsCompleted = false };
            var result = _taskService.UpdateTask(Guid.NewGuid(), dto);
            Assert.IsNull(result);
        }

        [Test]
        public void DeleteTask_ShouldMarkTaskInactive()
        {
            var target = _tasks[0];
            var deleted = _taskService.DeleteTask(target.Id);
            Assert.IsTrue(deleted);
            Assert.IsFalse(target.IsActive);
        }

        [Test]
        public void DeleteTask_NonExistentTask_ShouldReturnFalse()
        {
            var deleted = _taskService.DeleteTask(Guid.NewGuid());
            Assert.IsFalse(deleted);
        }
    }
}