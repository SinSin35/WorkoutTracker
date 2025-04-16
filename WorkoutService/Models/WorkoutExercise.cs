namespace WorkoutService.Models
{
    public class WorkoutExercise : Entity
    {
        public Guid WorkoutId { get; set; }
        public Guid ExerciseId { get; set; }
        public List<ExerciseSet> ExerciseSets { get; set; } = [];


        public Workout Workout { get; set; }
        public Exercise Exercise { get; set; }
    }
}
