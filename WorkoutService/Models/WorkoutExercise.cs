namespace WorkoutService.Models
{
    public class WorkoutExercise
    {
        public Guid Id { get; set; }
        public Guid WorkoutId { get; set; }
        public Guid ExerciseId { get; set; }
        public List<ExerciseSet> Sets { get; set; } = new List<ExerciseSet>();


        public Workout Workout { get; set; }
        public Exercise Exercise { get; set; }
    }
}
