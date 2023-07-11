using Microsoft.EntityFrameworkCore;
using TMX.ContextLibrary.Contexts;
using TMX.TaskService.Application.Common.Exceptions;
using TMX.TaskService.Application.Common.Models;
using TMX.TaskService.Application.Common.Validators;
using TMX.TaskService.Domain.Enums;
using TMX.TaskService.WebApi.Services;
using TMX.Tests.Common;


namespace TMX.Tests.UserTasks
{
    public class UserTaskManagerServiceTests : TestBase
    {
        private readonly TaskManagerService _sut;
        private readonly TaskServiceDbContext _dbContext;
        public UserTaskManagerServiceTests()
        {
            _dbContext = TaskServiceContext;
            _sut = new TaskManagerService(TaskServiceContext, Mapper, ContextAccessor) ;
        }

        [Fact]
        public async Task GetUserTasks_Succes()
        {
            // Arrange

            // Act
            var userTasks = await _sut.GetUserTasks();

            // Assert
            Assert.NotEmpty(userTasks);
        }

        [Fact]
        public async Task GetUserTaskById_Succes()
        {
            // Arrange
            
            // Act
            var userTask = await _sut.GetUserTaskByTaskId(TaskServiceContextFactory.TaskId_1);

            // Assert
            Assert.NotNull(userTask);
            Assert.Equal("Title1", userTask.Title);
            Assert.Equal(UserTaskStatus.InProgress, userTask.Status);
        }

        [Fact]
        public async Task GetUserTaskByInvalidTaskId_ThrowsNotFoundException()
        {
            // Arrange
            var invalidTaskId = Guid.NewGuid(); 

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _sut.GetUserTaskByTaskId(invalidTaskId);
            });
        }

        [Fact]
        public async Task GetEmptyUserTasks()
        {
            // Arrange
            var userId = Guid.NewGuid(); 
            var httpContextAccessor = TaskServiceContextFactory.CreateHttpContextAccessor(userId);

            var sut = new TaskManagerService(TaskServiceContext, Mapper, httpContextAccessor);

            // Act
            var userTasks = await sut.GetUserTasks();

            // Assert
            Assert.Empty(userTasks);
        }

        [Fact]
        public async Task Create_Success()
        {
            // Arrange
            var userTaskDto = new CreateUserTaskDto
            {
                Title = "Test",
                Description = "Test",
                DueDate = DateTime.Today,
                Status = UserTaskStatus.Open,
                Priority = UserTaskPriority.None,
            };

            // Act
            var createdUserTask = await _sut.Create(userTaskDto, CancellationToken.None);

            // Assert
            Assert.NotNull(
                await _dbContext.UserTasks.SingleOrDefaultAsync(x => 
                x.Id == createdUserTask.Id &&
                userTaskDto.Title == createdUserTask.Title && 
                userTaskDto.Description == createdUserTask.Description));
        }


        [Fact]
        public async Task Create_EmptyUserId()
        {
            // Arrange
            var userTaskDto = new CreateUserTaskDto
            {
                Title = "Test",
                Description = "Test",
                DueDate = DateTime.Today,
                Status = UserTaskStatus.Open,
                Priority = UserTaskPriority.None,
                UserId = Guid.Empty
            };
            var validator = new CreateUserTaskValidator();

            // Act
            var validationResult = await validator.ValidateAsync(userTaskDto);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Contains(validationResult.Errors, e => e.PropertyName == "UserId");
        }

        [Fact]
        public async Task Create_InvalidDueDate()
        {
            // Arrange
            var userTaskDto = new CreateUserTaskDto
            {
                Title = "Test",
                Description = "Test",
                DueDate = DateTime.Today.AddDays(-1), 
                Status = UserTaskStatus.Open,
                Priority = UserTaskPriority.None,
                UserId = Guid.NewGuid()
            };

            var validator = new CreateUserTaskValidator();

            // Act
            var validationResult = await validator.ValidateAsync(userTaskDto);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Contains(validationResult.Errors, e => e.PropertyName == "DueDate");
        }


        [Fact]
        public async Task Update_Success()
        {
            // Arrange
            var userTaskDto = new UpdateUserTaskDto
            {
                Title = "UpdatedTitle",
                Description = "UpdatedTitle",
                DueDate = DateTime.Today.AddHours(25),
                Status = UserTaskStatus.InProgress,
                Priority = UserTaskPriority.None
            };

            // Act
            await _sut.Update(userTaskDto, TaskServiceContextFactory.UserTaskIdForUpdate, 
                CancellationToken.None);

            // Assert
            Assert.NotNull(
                await _dbContext.UserTasks.SingleOrDefaultAsync(x => 
                x.Id == TaskServiceContextFactory.UserTaskIdForUpdate &&
                userTaskDto.Title == x.Title && 
                userTaskDto.Status == x.Status &&
                x.NotificationSent == false));

        }


        [Fact]
        public async Task Update_FailOnWrongUserTaskId()
        {
            // Arrange
            var userTaskDto = new UpdateUserTaskDto
            {
                Title = "UpdatedTitle",
                Description = "UpdatedTitle",
                DueDate = DateTime.Today.AddHours(25),
                Status = UserTaskStatus.InProgress,
                Priority = UserTaskPriority.None
            };
            var fakeUserTaskId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _sut.Update(userTaskDto, fakeUserTaskId, CancellationToken.None);
            });

        }


        [Fact]
        public async Task Update_InvalidDueDate()
        {
            // Arrange
            var userTaskDto = new UpdateUserTaskDto
            {
                Title = "UpdatedTitle",
                Description = "UpdatedTitle",
                DueDate = DateTime.Today.AddHours(-25),
                Status = UserTaskStatus.InProgress,
                Priority = UserTaskPriority.None
            };

            var validator = new UpdateUserTaskValidator();

            // Act
            var validationResult = await validator.ValidateAsync(userTaskDto);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Contains(validationResult.Errors, e => e.PropertyName == "DueDate");
        }

        [Fact]
        public async Task Delete_Success()
        {
            // Arrange

            // Act
            await _sut.Delete(TaskServiceContextFactory.UserTaskIdForDelete, CancellationToken.None);

            // Assert
            Assert.Null(
                await _dbContext.UserTasks.SingleOrDefaultAsync(x => 
                x.Id == TaskServiceContextFactory.UserTaskIdForDelete));
        }


        [Fact]
        public async Task Delete_FailOnWrongUserTaskId()
        {
            // Arrange
            var invalidUserTaskId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _sut.Delete(invalidUserTaskId, CancellationToken.None);
            });
        }
    }
}
