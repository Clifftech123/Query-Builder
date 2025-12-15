using API.Domain.Enums;

namespace API.Application.DTOs
{
    public class CreateConfigurationRequest
    {
        public ConfigurationType ConfigurationType { get; set; }
        public string Name { get; set; } = string.Empty;
        public TransactionMode TransactionMode { get; set; }
        public Guid IndustryId { get; set; }
        public Guid ProductId { get; set; }
        public Guid ProductSubTypeId { get; set; }
        public string? Settings { get; set; }
    }

    public class UpdateConfigurationRequest
    {
        public ConfigurationType ConfigurationType { get; set; }
        public string Name { get; set; } = string.Empty;
        public TransactionMode TransactionMode { get; set; }
        public string? Settings { get; set; }
    }

    public class ConfigurationResponse
    {
        public Guid Id { get; set; }
        public ConfigurationType ConfigurationType { get; set; }
        public string Name { get; set; } = string.Empty;
        public TransactionMode TransactionMode { get; set; }
        public Guid IndustryId { get; set; }
        public string IndustryName { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public Guid ProductSubTypeId { get; set; }
        public string ProductSubTypeName { get; set; } = string.Empty;
        public string? Settings { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
