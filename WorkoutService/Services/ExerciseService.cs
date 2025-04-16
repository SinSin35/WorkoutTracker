using Microsoft.EntityFrameworkCore;
using WorkoutService.Data;
using WorkoutService.Interfaces;
using WorkoutService.Models;

namespace WorkoutService.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly WorkoutContext _context;

        public ExerciseService(WorkoutContext context)
        {
            _context = context;
        }

        public async Task<Exercise> CreateExerciseAsync(ExerciseCreateDto exerciseCreateDto, CancellationToken cancellationToken)
        {
            Exercise exercise = new()
            {
                Name = exerciseCreateDto.Name,
                Description = exerciseCreateDto.Description
            };

            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync(cancellationToken);
            return exercise;
        }

        public async Task<Exercise?> GetExerciseByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Exercises
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }
    }
}
