namespace API.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to operate on a deleted entity
    /// </summary>
    public class EntityDeletedException : DomainException
    {
        public string EntityName { get; }
        public object EntityId { get; }

        public EntityDeletedException(string entityName, object entityId)
            : base($"{entityName} with ID '{entityId}' has been deleted", "ENTITY_DELETED")
        {
            EntityName = entityName;
            EntityId = entityId;
        }
    }
}
