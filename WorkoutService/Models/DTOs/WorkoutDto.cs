namespace WorkoutService.Models.DTOs
{
    public class WorkoutDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid UserId { get; set; }
        public List<WorkoutExerciseDto>? WorkoutExercises { get; set; }
    }
}
