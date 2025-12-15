using API.Domain.Exceptions;

namespace API.Domain.Common
{
    /// <summary>
    /// Base entity class with common properties for all domain entities
    /// </summary>
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        public DateTime? DeletedAt { get; protected set; }
        public bool IsDeleted => DeletedAt.HasValue;

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Soft delete the entity
        /// </summary>
        public virtual void Delete()
        {
            if (IsDeleted)
            {
                throw new InvalidOperationDomainException("Entity is already deleted");
            }
            DeletedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Mark the entity as updated
        /// </summary>
        protected void MarkAsUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Restore a soft-deleted entity
        /// </summary>
        public void Restore()
        {
            if (!IsDeleted)
            {
                throw new InvalidOperationDomainException("Entity is not deleted");
            }
            DeletedAt = null;
            MarkAsUpdated();
        }
    }
}
