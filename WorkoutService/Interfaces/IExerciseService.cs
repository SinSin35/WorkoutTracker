using WorkoutService.Models.DTOs;
using WorkoutService.Models.Entities;

namespace WorkoutService.Interfaces
{
    public interface IExerciseService
    {
        public Task<Exercise> CreateExerciseAsync(ExerciseCreateDto exerciseCreateDto, CancellationToken cancellationToken);
        public Task<Exercise?> GetExerciseByIdAsync(Guid exerciseId, CancellationToken cancellationToken);
    }
}
