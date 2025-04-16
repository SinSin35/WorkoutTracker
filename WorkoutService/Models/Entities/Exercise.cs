namespace WorkoutService.Models.Entities
{
    /// <summary>
    /// Упражнение, из которых состоят тренировки.
    /// Есть как дефолтные, добавленные мной изначально, так и кастомные от пользователей.
    /// </summary>
    public class Exercise : Entity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }

        public Guid? UserId { get; set; }

        public List<WorkoutExercise> WorkoutExercises { get; set; } = [];
    }
}
