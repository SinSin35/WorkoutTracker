using Microsoft.EntityFrameworkCore;
using WorkoutService.Data;
using WorkoutService.Models.DTOs;
using WorkoutService.Services;

namespace WorkoutService.Tests.Services;

public class ExerciseServiceTests
{
    private WorkoutContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<WorkoutContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new WorkoutContext(options);
    }

    [Fact]
    public async Task CreateExerciseAsync_CreatesExercise()
    {
        // Arrange
        var context = GetInMemoryContext();
        var service = new ExerciseService(context);

        var dto = new ExerciseCreateDto
        {
            Name = "Push Up",
            Description = "A basic bodyweight exercise"
        };

        // Act
        var result = await service.CreateExerciseAsync(dto, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(dto.Description, result.Description);

        var fromDb = await context.Exercises.FindAsync(result.Id);
        Assert.NotNull(fromDb);
        Assert.Equal(dto.Name, fromDb.Name);
        Assert.Equal(dto.Description, fromDb.Description);
    }

    [Fact]
    public async Task GetExerciseByIdAsync_ReturnsCorrectExercise()
    {
        // Arrange
        var context = GetInMemoryContext();
        var service = new ExerciseService(context);

        var exercise = new Models.Entities.Exercise
        {
            Id = Guid.NewGuid(),
            Name = "Squat",
            Description = "Leg exercise"
        };

        context.Exercises.Add(exercise);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetExerciseByIdAsync(exercise.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(exercise.Id, result.Id);
    }

    [Fact]
    public async Task GetExerciseByIdAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var context = GetInMemoryContext();
        var service = new ExerciseService(context);

        // Act
        var result = await service.GetExerciseByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
