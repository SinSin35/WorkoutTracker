namespace WorkoutService.Models.DTOs
{
    public class ExerciseSetCreateDto
    {
        public int Repetitions { get; set; }
        public double? Weight { get; set; }
        public Guid WorkoutExerciseId { get; set; }
    }
}
