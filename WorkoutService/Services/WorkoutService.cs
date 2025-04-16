﻿using Microsoft.EntityFrameworkCore;
using WorkoutService.Data;
using WorkoutService.Interfaces;
using WorkoutService.Models;
using WorkoutService.Models.DTOs;
using WorkoutService.Models.Entities;

namespace WorkoutService.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly WorkoutContext _context;

        public WorkoutService(WorkoutContext context)
        {
            _context = context;
        }

        public async Task<WorkoutDto> CreateWorkoutAsync(WorkoutCreateDto workoutCreateDto, CancellationToken cancellationToken)
        {
            var workout = workoutCreateDto.ToEntity();

            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync(cancellationToken);
            return workout.ToDto();
        }

        public async Task<WorkoutDto?> GetWorkoutByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var workout = await _context.Workouts
                .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return workout?.ToDto();
        }

        public async Task<WorkoutDto> AddExerciseToWorkoutAsync(Guid workoutId, Guid exerciseId, CancellationToken cancellationToken)
        {
            var workout = await _context.Workouts
                .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
                .FirstOrDefaultAsync(x => x.Id == workoutId, cancellationToken);

            if (workout == null)
                throw new Exception("Workout not found");

            var exercise = await _context.Exercises.FindAsync(exerciseId);
            if (exercise == null)
                throw new Exception("Exercise not found");

            var alreadyExists = workout.WorkoutExercises.Any(we => we.ExerciseId == exerciseId);
            if (alreadyExists)
                throw new Exception("Exercise already added to this workout");

            var workoutExercise = new WorkoutExercise
            {
                WorkoutId = workoutId,
                ExerciseId = exerciseId
            };

            _context.WorkoutExercises.Add(workoutExercise);
            await _context.SaveChangesAsync(cancellationToken);

            // Повторно загружаем с включёнными данными, если нужно отдать полные данные
            var updatedWorkout = await _context.Workouts
                .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
                .FirstOrDefaultAsync(w => w.Id == workoutId, cancellationToken);

            return updatedWorkout!.ToDto();
        }
    }
}
