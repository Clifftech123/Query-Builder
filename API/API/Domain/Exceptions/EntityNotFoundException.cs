namespace API.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when an entity is not found
    /// </summary>
    public class EntityNotFoundException : DomainException
    {
        public string EntityName { get; }
        public object EntityId { get; }

        public EntityNotFoundException(string entityName, object entityId)
            : base($"{entityName} with ID '{entityId}' was not found", "ENTITY_NOT_FOUND")
        {
            EntityName = entityName;
            EntityId = entityId;
        }

        public EntityNotFoundException(string entityName, object entityId, string message)
            : base(message, "ENTITY_NOT_FOUND")
        {
            EntityName = entityName;
            EntityId = entityId;
        }
    }
}
