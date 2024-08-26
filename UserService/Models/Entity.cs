using UserService.Interfaces;

namespace UserService.Models
{
    public abstract class Entity : IEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
