using WorkoutService.Models.DTOs;
using WorkoutService.Models.Entities;

namespace WorkoutService.Interfaces
{
    public interface IWorkoutService
    {
        public Task<WorkoutDto> CreateWorkoutAsync(WorkoutCreateDto workoutCreateDto, CancellationToken cancellationToken);

        public Task<WorkoutDto?> GetWorkoutByIdAsync(Guid id, CancellationToken cancellationToken);

        public Task<WorkoutDto> AddExerciseToWorkoutAsync(Guid workoutId, Guid exerciseId, CancellationToken cancellationToken);

        public Task<ExerciseSetDto> AddExerciseSetAsync(Guid workoutExerciseId, ExerciseSetCreateDto dto, CancellationToken cancellationToken);
    
        public Task<ExerciseSetDto?> GetExerciseSetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
