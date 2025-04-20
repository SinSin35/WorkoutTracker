using Microsoft.AspNetCore.Mvc;
using WorkoutService.Interfaces;
using WorkoutService.Models.DTOs;
using WorkoutService.Models.Entities;

namespace WorkoutService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : Controller
    {
        private readonly IWorkoutService _workoutService;
        public WorkoutController(IWorkoutService workoutService) { 
            _workoutService = workoutService;
        }

        [HttpPost]
        public async Task<ActionResult<WorkoutDto>> Create([FromBody] WorkoutCreateDto workoutCreateDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var workout = await _workoutService.CreateWorkoutAsync(workoutCreateDto, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = workout.Id }, workout);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            var workout = await _workoutService.GetWorkoutByIdAsync(id, cancellationToken);
            if (workout == null)
                return NotFound();

            return Ok(workout);
        }

        [HttpPost("{workoutId}/exercises/{exerciseId}")]
        public async Task<ActionResult<WorkoutDto>> AddExerciseToWorkout(Guid workoutId, Guid exerciseId, CancellationToken cancellationToken)
        {
            try
            {
                var updatedWorkout = await _workoutService.AddExerciseToWorkoutAsync(workoutId, exerciseId, cancellationToken);
                return Ok(updatedWorkout);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("workout-exercises/{id}/sets")]
        public async Task<ActionResult<ExerciseSetDto>> AddExerciseSet(Guid id, [FromBody] ExerciseSetCreateDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var createdSet = await _workoutService.AddExerciseSetAsync(id, dto, cancellationToken);
                return CreatedAtAction(nameof(AddExerciseSet), new { id = createdSet.Id }, createdSet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("exercise-sets/{id}")]
        public async Task<ActionResult<ExerciseSetDto>> GetExerciseSetById(Guid id, CancellationToken cancellationToken)
        {
            var set = await _workoutService.GetExerciseSetByIdAsync(id, cancellationToken);

            if (set == null)
                return NotFound();

            return Ok(set);
        }
    }
}
