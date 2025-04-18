﻿using Microsoft.AspNetCore.Identity;
using UserService.Interfaces;

namespace UserService.Models
{
    public class User : IdentityUser<Guid>, IEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
