using API.Domain.Common;
using API.Domain.Enums;
using API.Domain.Exceptions;

namespace API.Domain.Models
{
    /// <summary>
    /// Configuration entity following DDD principles
    /// Represents a configuration that combines Industry, Product, ProductSubType and settings
    /// </summary>
    public class Configuration : BaseEntity
    {
        public ConfigurationType ConfigurationType { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public TransactionMode TransactionMode { get; private set; }
        public string? Settings { get; private set; } // JSON string for additional settings

        // Foreign keys
        public Guid IndustryId { get; private set; }
        public Guid ProductId { get; private set; }
        public Guid ProductSubTypeId { get; private set; }

        // Navigation properties (EF will populate these)
        public Industry Industry { get; private set; } = null!;
        public Product Product { get; private set; } = null!;
        public ProductSubType ProductSubType { get; private set; } = null!;

        // Private constructor for EF
        private Configuration() : base() { }

        /// <summary>
        /// Factory method to create a new Configuration
        /// </summary>
        public static Configuration Create(
            ConfigurationType configurationType,
            string name,
            TransactionMode transactionMode,
            Guid industryId,
            Guid productId,
            Guid productSubTypeId,
            string? settings = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException(nameof(name), "Configuration name cannot be empty");
            }

            if (name.Length > 200)
            {
                throw new ValidationException(nameof(name), "Configuration name cannot exceed 200 characters");
            }

            if (industryId == Guid.Empty)
            {
                throw new ValidationException(nameof(industryId), "Industry ID cannot be empty");
            }

            if (productId == Guid.Empty)
            {
                throw new ValidationException(nameof(productId), "Product ID cannot be empty");
            }

            if (productSubTypeId == Guid.Empty)
            {
                throw new ValidationException(nameof(productSubTypeId), "ProductSubType ID cannot be empty");
            }

            if (!Enum.IsDefined(typeof(ConfigurationType), configurationType))
            {
                throw new ValidationException(nameof(configurationType), "Invalid configuration type");
            }

            if (!Enum.IsDefined(typeof(TransactionMode), transactionMode))
            {
                throw new ValidationException(nameof(transactionMode), "Invalid transaction mode");
            }

            return new Configuration
            {
                ConfigurationType = configurationType,
                Name = name.Trim(),
                TransactionMode = transactionMode,
                IndustryId = industryId,
                ProductId = productId,
                ProductSubTypeId = productSubTypeId,
                Settings = settings?.Trim()
            };
        }

        /// <summary>
        /// Update configuration details
        /// </summary>
        public void Update(
            ConfigurationType configurationType,
            string name,
            TransactionMode transactionMode,
            string? settings = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException(nameof(name), "Configuration name cannot be empty");
            }

            if (name.Length > 200)
            {
                throw new ValidationException(nameof(name), "Configuration name cannot exceed 200 characters");
            }

            if (!Enum.IsDefined(typeof(ConfigurationType), configurationType))
            {
                throw new ValidationException(nameof(configurationType), "Invalid configuration type");
            }

            if (!Enum.IsDefined(typeof(TransactionMode), transactionMode))
            {
                throw new ValidationException(nameof(transactionMode), "Invalid transaction mode");
            }

            ConfigurationType = configurationType;
            Name = name.Trim();
            TransactionMode = transactionMode;
            Settings = settings?.Trim();
            MarkAsUpdated();
        }

        /// <summary>
        /// Change the configuration's industry, product, and subtype
        /// </summary>
        public void ChangeHierarchy(Guid industryId, Guid productId, Guid productSubTypeId)
        {
            if (industryId == Guid.Empty)
            {
                throw new ValidationException(nameof(industryId), "Industry ID cannot be empty");
            }

            if (productId == Guid.Empty)
            {
                throw new ValidationException(nameof(productId), "Product ID cannot be empty");
            }

            if (productSubTypeId == Guid.Empty)
            {
                throw new ValidationException(nameof(productSubTypeId), "ProductSubType ID cannot be empty");
            }

            IndustryId = industryId;
            ProductId = productId;
            ProductSubTypeId = productSubTypeId;
            MarkAsUpdated();
        }

        /// <summary>
        /// Update just the settings
        /// </summary>
        public void UpdateSettings(string? settings)
        {
            Settings = settings?.Trim();
            MarkAsUpdated();
        }
    }
}
