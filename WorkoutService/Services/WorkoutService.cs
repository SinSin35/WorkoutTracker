using Microsoft.EntityFrameworkCore;
using WorkoutService.Data;
using WorkoutService.Interfaces;
using WorkoutService.Models;

namespace WorkoutService.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly WorkoutContext _context;

        public WorkoutService(WorkoutContext context)
        {
            _context = context;
        }

        public async Task<Workout> CreateWorkoutAsync(WorkoutCreateDto workoutCreateDto, CancellationToken cancellationToken)
        {
            Workout workout = new()
            {
                Name = workoutCreateDto.Name,
                Description = workoutCreateDto.Description,
                UserId = workoutCreateDto.UserId
            };

            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync(cancellationToken);
            return workout;
        }

        public async Task<Workout?> GetWorkoutByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Workouts
                .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Workout> AddExerciseToWorkoutAsync(Guid workoutId, Guid exerciseId, CancellationToken cancellationToken)
        {
            var workout = await _context.Workouts
                .Include(w => w.WorkoutExercises)
                .FirstOrDefaultAsync(x => x.Id == workoutId, cancellationToken);

            if (workout == null)
                throw new Exception("Workout not found");

            var exercise = await _context.Exercises.FindAsync(exerciseId);
            if (exercise == null)
                throw new Exception("Exercise not found");

            var workoutExercise = new WorkoutExercise
            {
                WorkoutId = workoutId,
                ExerciseId = exerciseId
            };

            workout.WorkoutExercises.Add(workoutExercise);
            await _context.SaveChangesAsync(cancellationToken);
            return workout;
        }
    }
}
