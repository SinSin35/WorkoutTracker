using WorkoutService.Models.DTOs;
using WorkoutService.Models.Entities;

public static class WorkoutMapper
{
    public static WorkoutDto ToDto(this Workout workout)
    {
        return new WorkoutDto
        {
            Id = workout.Id,
            Name = workout.Name,
            Description = workout.Description,
            UserId = workout.UserId,
            WorkoutExercises = workout.WorkoutExercises?.Select(we => new WorkoutExerciseDto
            {
                ExerciseId = we.ExerciseId,
                ExerciseName = we.Exercise?.Name
            }).ToList()
        };
    }

    public static Workout ToEntity(this WorkoutCreateDto dto)
    {
        return new Workout
        {
            Name = dto.Name,
            Description = dto.Description,
            UserId = dto.UserId
        };
    }
}