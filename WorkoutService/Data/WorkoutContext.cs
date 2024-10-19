using Microsoft.EntityFrameworkCore;
using WorkoutService.Models;

namespace WorkoutService.Data
{
    public class WorkoutContext : DbContext
    {
        public WorkoutContext(DbContextOptions<WorkoutContext> options) : base(options) { }

        public DbSet<Workout> Workouts { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public DbSet<ExerciseSet> ExerciseSets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkoutExercise>()
                .HasKey(we => we.Id);

            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Workout)
                .WithMany(w => w.WorkoutExercices)
                .HasForeignKey(we => we.WorkoutId);

            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Exercise)
                .WithMany(e => e.WorkoutExercices)
                .HasForeignKey(we => we.ExerciseId);

            modelBuilder.Entity<ExerciseSet>()
                .HasOne(es => es.WorkoutExercise)
                .WithMany(we => we.Sets)
                .HasForeignKey(es => es.WorkoutExerciseId);
        }
    }
}
