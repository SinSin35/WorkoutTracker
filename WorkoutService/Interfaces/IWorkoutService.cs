using WorkoutService.Models;

namespace WorkoutService.Interfaces
{
    public interface IWorkoutService
    {
        public Task<Workout> CreateWorkoutAsync(WorkoutCreateDto workoutCreateDto, CancellationToken cancellationToken);

        public Task<Workout?> GetWorkoutByIdAsync(Guid id, CancellationToken cancellationToken);

        public Task<Workout> AddExerciseToWorkoutAsync(Guid workoutId, Guid exerciseId, CancellationToken cancellationToken);
    }
}
