namespace WorkoutService.Models
{
    /// <summary>
    /// Упражнение, из которых состоят тренировки.
    /// Есть как дефолтные, добавленные мной изначально, так и кастомные от пользователей.
    /// </summary>
    public class Exercise
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Guid? UserId { get; set; }

        public List<WorkoutExercise> WorkoutExercices { get; set; } = new List<WorkoutExercise>();
    }
}
