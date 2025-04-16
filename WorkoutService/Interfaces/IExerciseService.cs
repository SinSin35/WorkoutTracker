using WorkoutService.Models;

namespace WorkoutService.Interfaces
{
    public interface IExerciseService
    {
        public Task<Exercise> CreateExerciseAsync(ExerciseCreateDto exerciseCreateDto, CancellationToken cancellationToken);
        public Task<Exercise?> GetExerciseByIdAsync(Guid exerciseId, CancellationToken cancellationToken);
    }
}
