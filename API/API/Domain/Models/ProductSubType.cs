using API.Domain.Common;
using API.Domain.Exceptions;

namespace API.Domain.Models
{
    /// <summary>
    /// Product SubType entity following DDD principles
    /// ProductSubType belongs to a Product
    /// </summary>
    public class ProductSubType : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }

        // Foreign key to Product
        public Guid ProductId { get; private set; }

        // Navigation property (EF will populate this)
        public Product Product { get; private set; } = null!;

        // Private constructor for EF
        private ProductSubType() : base() { }

        /// <summary>
        /// Factory method to create a new ProductSubType
        /// </summary>
        public static ProductSubType Create(string name, Guid productId, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException(nameof(name), "ProductSubType name cannot be empty");
            }

            if (name.Length > 200)
            {
                throw new ValidationException(nameof(name), "ProductSubType name cannot exceed 200 characters");
            }

            if (productId == Guid.Empty)
            {
                throw new ValidationException(nameof(productId), "Product ID cannot be empty");
            }

            if (description?.Length > 500)
            {
                throw new ValidationException(nameof(description), "Description cannot exceed 500 characters");
            }

            return new ProductSubType
            {
                Name = name.Trim(),
                Description = description?.Trim(),
                ProductId = productId
            };
        }

        /// <summary>
        /// Update subtype details
        /// </summary>
        public void Update(string name, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException(nameof(name), "ProductSubType name cannot be empty");
            }

            if (name.Length > 200)
            {
                throw new ValidationException(nameof(name), "ProductSubType name cannot exceed 200 characters");
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
        /// Change the subtype's parent product
        /// </summary>
        public void ChangeProduct(Guid newProductId)
        {
            if (newProductId == Guid.Empty)
            {
                throw new ValidationException(nameof(newProductId), "Product ID cannot be empty");
            }

            if (ProductId == newProductId)
            {
                return; // No change needed
            }

            ProductId = newProductId;
            MarkAsUpdated();
        }
    }
}
