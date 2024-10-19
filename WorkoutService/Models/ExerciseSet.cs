namespace WorkoutService.Models
{
    /// <summary>
    /// Подход в упражнении
    /// </summary>
    public class ExerciseSet
    {
        public Guid Id { get; set; }
        public Guid WorkoutExerciseId { get; set; }
        public int Repetitions { get; set; }
        public double? Weight { get; set; }

        public WorkoutExercise WorkoutExercise { get; set; }
    }
}
