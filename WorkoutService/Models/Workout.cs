namespace WorkoutService.Models
{
    /// <summary>
    /// Тренировка, состоящая из множества упражнений (Exercises)
    /// </summary>

    public class Workout
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Notes { get; set; }
        public List<WorkoutExercise> WorkoutExercices { get; set; } = new List<WorkoutExercise>();
    }
}
