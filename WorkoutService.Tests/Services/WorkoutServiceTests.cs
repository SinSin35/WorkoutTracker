using Microsoft.EntityFrameworkCore;
using WorkoutService.Data;
using WorkoutService.Models.DTOs;
using WorkoutService.Models.Entities;
using WorkoutService.Services;

namespace WorkoutService.Tests.Services;

public class WorkoutServiceTests
{
    private WorkoutContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<WorkoutContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // уникальная БД на каждый тест
            .Options;

        return new WorkoutContext(options);
    }

    [Fact]
    public async Task CreateWorkoutAsync_ShouldCreateWorkout()
    {
        var context = GetInMemoryContext();
        var service = new WorkoutService.Services.WorkoutService(context);

        var dto = new WorkoutCreateDto
        {
            Name = "Test Workout",
            Description = "Test Description"
        };

        var result = await service.CreateWorkoutAsync(dto, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(dto.Description, result.Description);

        var workoutInDb = await context.Workouts.FindAsync(result.Id);
        Assert.NotNull(workoutInDb);
        Assert.Equal(dto.Name, workoutInDb.Name);
        Assert.Equal(dto.Description, workoutInDb.Description);
    }

    [Fact]
    public async Task GetWorkoutByIdAsync_ShouldReturnWorkout()
    {
        var context = GetInMemoryContext();
        var service = new WorkoutService.Services.WorkoutService(context);

        var workout = new Workout
        {
            Id = Guid.NewGuid(),
            Name = "Test Workout",
            Description = "Test Description"
        };

        context.Workouts.Add(workout);
        await context.SaveChangesAsync();

        var result = await service.GetWorkoutByIdAsync(workout.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(workout.Id, result.Id);
        Assert.Equal(workout.Name, result.Name);
        Assert.Equal(workout.Description, result.Description);
    }

    [Fact]
    public async Task GetWorkoutByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        var context = GetInMemoryContext();
        var service = new WorkoutService.Services.WorkoutService(context);

        // Act
        var result = await service.GetWorkoutByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddExerciseToWorkoutAsync_ShouldAddExercise_WhenValid()
    {
        // Arrange
        var context = GetInMemoryContext();
        var service = new WorkoutService.Services.WorkoutService(context);

        var workout = new Workout
        {
            Id = Guid.NewGuid(),
            Name = "Test Workout",
            Description = ""
        };

        var exercise = new Exercise
        {
            Id = Guid.NewGuid(),
            Name = "Push Ups"
        };

        context.Workouts.Add(workout);
        context.Exercises.Add(exercise);
        await context.SaveChangesAsync();

        // Act
        var result = await service.AddExerciseToWorkoutAsync(workout.Id, exercise.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.WorkoutExercises);
        Assert.Single(result.WorkoutExercises);
        Assert.Equal(exercise.Id, result.WorkoutExercises.First().ExerciseId);
    }

    [Fact]
    public async Task AddExerciseToWorkoutAsync_ShouldThrow_WhenWorkoutNotFound()
    {
        // Arrange
        var context = GetInMemoryContext();
        var service = new WorkoutService.Services.WorkoutService(context);

        var exercise = new Exercise { Id = Guid.NewGuid(), Name = "Squats" };
        context.Exercises.Add(exercise);
        await context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await service.AddExerciseToWorkoutAsync(Guid.NewGuid(), exercise.Id, CancellationToken.None);
        });
    }

    [Fact]
    public async Task AddExerciseToWorkoutAsync_ShouldThrow_WhenExerciseNotFound()
    {
        // Arrange
        var context = GetInMemoryContext();
        var service = new WorkoutService.Services.WorkoutService(context);

        var workout = new Workout { Id = Guid.NewGuid(), Name = "Test" };
        context.Workouts.Add(workout);
        await context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await service.AddExerciseToWorkoutAsync(workout.Id, Guid.NewGuid(), CancellationToken.None);
        });
    }

    [Fact]
    public async Task AddExerciseToWorkoutAsync_ShouldThrow_WhenAlreadyAdded()
    {
        // Arrange
        var context = GetInMemoryContext();
        var service = new WorkoutService.Services.WorkoutService(context);

        var workout = new Workout { Id = Guid.NewGuid(), Name = "Workout" };
        var exercise = new Exercise { Id = Guid.NewGuid(), Name = "Plank" };

        context.Workouts.Add(workout);
        context.Exercises.Add(exercise);
        context.WorkoutExercises.Add(new WorkoutExercise
        {
            WorkoutId = workout.Id,
            ExerciseId = exercise.Id
        });

        await context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await service.AddExerciseToWorkoutAsync(workout.Id, exercise.Id, CancellationToken.None);
        });
    }

    [Fact]
    public async Task AddExerciseSetAsync_ShouldAddSet_WhenWorkoutExerciseExists()
    {
        // Arrange
        var context = GetInMemoryContext();
        var service = new WorkoutService.Services.WorkoutService(context);

        var workout = new Workout { Id = Guid.NewGuid(), Name = "Legs", UserId = Guid.NewGuid() };
        var exercise = new Exercise { Id = Guid.NewGuid(), Name = "Squat" };
        var workoutExercise = new WorkoutExercise { Id = Guid.NewGuid(), WorkoutId = workout.Id, ExerciseId = exercise.Id };

        context.Workouts.Add(workout);
        context.Exercises.Add(exercise);
        context.WorkoutExercises.Add(workoutExercise);
        await context.SaveChangesAsync();

        var dto = new ExerciseSetCreateDto { Repetitions = 12, Weight = 100 };

        // Act
        var result = await service.AddExerciseSetAsync(workoutExercise.Id, dto, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Repetitions, result.Repetitions);
        Assert.Equal(dto.Weight, result.Weight);
    }

    [Fact]
    public async Task AddExerciseSetAsync_ShouldThrow_WhenWorkoutExerciseNotFound()
    {
        // Arrange
        var context = GetInMemoryContext();
        var service = new WorkoutService.Services.WorkoutService(context);

        var dto = new ExerciseSetCreateDto { Repetitions = 8, Weight = 80 };
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() =>
            service.AddExerciseSetAsync(nonExistentId, dto, CancellationToken.None));
    }

    [Fact]
    public async Task GetExerciseSetByIdAsync_ShouldReturnSet_WhenExists()
    {
        // Arrange
        var context = GetInMemoryContext();
        var service = new WorkoutService.Services.WorkoutService(context);

        var workoutExercise = new WorkoutExercise { Id = Guid.NewGuid(), WorkoutId = Guid.NewGuid(), ExerciseId = Guid.NewGuid() };
        var set = new ExerciseSet { Id = Guid.NewGuid(), Repetitions = 10, Weight = 90, WorkoutExerciseId = workoutExercise.Id };

        context.WorkoutExercises.Add(workoutExercise);
        context.ExerciseSets.Add(set);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetExerciseSetByIdAsync(set.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(set.Repetitions, result.Repetitions);
        Assert.Equal(set.Weight, result.Weight);
    }

    [Fact]
    public async Task GetExerciseSetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        var context = GetInMemoryContext();
        var service = new WorkoutService.Services.WorkoutService(context);

        var fakeId = Guid.NewGuid();

        // Act
        var result = await service.GetExerciseSetByIdAsync(fakeId, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
