using API.Domain.Common;
using API.Domain.Exceptions;

namespace API.Domain.Models
{
    /// <summary>
    /// Product entity following DDD principles
    /// Product belongs to an Industry
    /// </summary>
    public class Product : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }

        // Foreign key to Industry
        public Guid IndustryId { get; private set; }

        // Navigation property (EF will populate this)
        public Industry Industry { get; private set; } = null!;

        // Navigation property for subtypes
        public IReadOnlyCollection<ProductSubType> SubTypes => _subTypes.AsReadOnly();
        private readonly List<ProductSubType> _subTypes = new();

        // Private constructor for EF
        private Product() : base() { }

        /// <summary>
        /// Factory method to create a new Product
        /// </summary>
        public static Product Create(string name, Guid industryId, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException(nameof(name), "Product name cannot be empty");
            }

            if (name.Length > 200)
            {
                throw new ValidationException(nameof(name), "Product name cannot exceed 200 characters");
            }

            if (industryId == Guid.Empty)
            {
                throw new ValidationException(nameof(industryId), "Industry ID cannot be empty");
            }

            if (description?.Length > 500)
            {
                throw new ValidationException(nameof(description), "Description cannot exceed 500 characters");
            }

            return new Product
            {
                Name = name.Trim(),
                Description = description?.Trim(),
                IndustryId = industryId
            };
        }

        /// <summary>
        /// Update product details
        /// </summary>
        public void Update(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException(nameof(name), "Product name cannot be empty");
            }

            if (name.Length > 200)
            {
                throw new ValidationException(nameof(name), "Product name cannot exceed 200 characters");
            }

            if (description?.Length > 500)
            {
                throw new ValidationException(nameof(description), "Description cannot exceed 500 characters");
            }

            Name = name.Trim();
            Description = description?.Trim();
            MarkAsUpdated();
        }

        /// <summary>
        /// Change the product's industry
        /// </summary>
        public void ChangeIndustry(Guid newIndustryId)
        {
            if (newIndustryId == Guid.Empty)
            {
                throw new ValidationException(nameof(newIndustryId), "Industry ID cannot be empty");
            }

            if (IndustryId == newIndustryId)
            {
                return; // No change needed
            }

            IndustryId = newIndustryId;
            MarkAsUpdated();
        }
    }
}
