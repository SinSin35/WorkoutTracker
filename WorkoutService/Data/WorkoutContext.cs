using Microsoft.EntityFrameworkCore;
using WorkoutService.Models.Entities;

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
                .WithMany(w => w.WorkoutExercises)
                .HasForeignKey(we => we.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade); // Удаление Workout удаляет все WorkoutExercise

            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Exercise)
                .WithMany(e => e.WorkoutExercises)
                .HasForeignKey(we => we.ExerciseId)
                .OnDelete(DeleteBehavior.Restrict); // Упражнения нельзя удалить, если они где-то используются

            modelBuilder.Entity<WorkoutExercise>()
                .HasIndex(we => new { we.WorkoutId, we.ExerciseId })
                .IsUnique();

            modelBuilder.Entity<ExerciseSet>()
                .HasOne(es => es.WorkoutExercise)
                .WithMany(we => we.ExerciseSets)
                .HasForeignKey(es => es.WorkoutExerciseId)
                .OnDelete(DeleteBehavior.Cascade); // Удаление WorkoutExercise удаляет все его ExerciseSet

            modelBuilder.Entity<Exercise>()
                .HasIndex(e => e.UserId); // Добавляем индекс для оптимизации запросов

            modelBuilder.Entity<Exercise>()
                .Property(e => e.UserId)
                .IsRequired(false); // UserId может быть null, если это дефолтное упражнение
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Entity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
