﻿namespace WorkoutService.Models.DTOs
{
    public class WorkoutCreateDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid UserId { get; set; }
    }
}
