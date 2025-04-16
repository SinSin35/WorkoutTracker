using Microsoft.AspNetCore.Mvc;
using WorkoutService.Interfaces;
using WorkoutService.Models;

[ApiController]
[Route("api/[controller]")]
public class ExerciseController : ControllerBase
{
    private readonly IExerciseService _exerciseService;

    public ExerciseController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    [HttpPost]
    public async Task<ActionResult<Exercise>> Create([FromBody] ExerciseCreateDto exerciseCreateDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var exercise = await _exerciseService.CreateExerciseAsync(exerciseCreateDto, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = exercise.Id }, exercise);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Exercise>> Get(Guid id, CancellationToken cancellationToken)
    {
        var exercise = await _exerciseService.GetExerciseByIdAsync(id, cancellationToken);
        if (exercise == null)
            return NotFound();

        return Ok(exercise);
    }
}