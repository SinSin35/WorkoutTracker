using WorkoutService.Models.DTOs;
using WorkoutService.Models.Entities;

namespace WorkoutService.Models.Mapping
{
    public static class ExerciseSetMapper
    {
        public static ExerciseSetDto ToDto(this ExerciseSet entity)
        {
            return new ExerciseSetDto
            {
                Id = entity.Id,
                WorkoutExerciseId = entity.WorkoutExerciseId,
                Repetitions = entity.Repetitions,
                Weight = entity.Weight
            };
        }

        public static ExerciseSet ToEntity(this ExerciseSetCreateDto dto)
        {
            return new ExerciseSet
            {
                Repetitions = dto.Repetitions,
                Weight = dto.Weight
            };
        }
    }
}
