namespace API.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to create an entity that already exists
    /// </summary>
    public class EntityAlreadyExistsException : DomainException
    {
        public string EntityName { get; }
        public string PropertyName { get; }
        public object PropertyValue { get; }

        public EntityAlreadyExistsException(string entityName, string propertyName, object propertyValue)
            : base($"{entityName} with {propertyName} '{propertyValue}' already exists", "ENTITY_ALREADY_EXISTS")
        {
            EntityName = entityName;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }
    }
}
