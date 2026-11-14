namespace ELearning.SharedKernal
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAtUtc { get; protected set; } = DateTime.UtcNow;
        public DateTime UpdatedAtUtc {get; set;}
    }
}