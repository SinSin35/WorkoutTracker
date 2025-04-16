namespace WorkoutService.Models.DTOs
{
    public class ExerciseSetDto
    {
        public Guid Id { get; set; }
        public int Repetitions { get; set; }
        public double? Weight { get; set; }
        public Guid WorkoutExerciseId { get; set; }
    }
}
