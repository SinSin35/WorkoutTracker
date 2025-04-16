namespace WorkoutService.Models
{
    /// <summary>
    /// Тренировка, состоящая из множества упражнений (Exercises)
    /// </summary>

    public class Workout : Entity
    {
        public Guid UserId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<WorkoutExercise> WorkoutExercises { get; set; } = [];
    }
}
