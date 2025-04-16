using Microsoft.AspNetCore.Mvc;
using WorkoutService.Interfaces;
using WorkoutService.Models;

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
        public async Task<ActionResult<Workout>> Create([FromBody] WorkoutCreateDto workoutCreateDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var workout = await _workoutService.CreateWorkoutAsync(workoutCreateDto, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = workout.Id }, workout);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Workout>> Get(Guid id, CancellationToken cancellationToken)
        {
            var workout = await _workoutService.GetWorkoutByIdAsync(id, cancellationToken);
            if (workout == null)
                return NotFound();

            return Ok(workout);
        }

        [HttpPost("{workoutId}/exercises/{exerciseId}")]
        public async Task<ActionResult<Workout>> AddExerciseToWorkout(Guid workoutId, Guid exerciseId, CancellationToken cancellationToken)
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
    }
}
