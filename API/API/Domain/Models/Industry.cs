using API.Domain.Common;
using API.Domain.Exceptions;

namespace API.Domain.Models
{
    /// <summary>
    /// Industry entity following DDD principles
    /// </summary>
    public class Industry : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;

        // Navigation property for products
        public IReadOnlyCollection<Product> Products => _products.AsReadOnly();
        private readonly List<Product> _products = new();

        // Private constructor for EF
        private Industry() : base() { }

        /// <summary>
        /// Factory method to create a new Industry
        /// </summary>
        public static Industry Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException(nameof(name), "Industry name cannot be empty");
            }

            if (name.Length > 200)
            {
                throw new ValidationException(nameof(name), "Industry name cannot exceed 200 characters");
            }

            return new Industry
            {
                Name = name.Trim()
            };
        }

        /// <summary>
        /// Update the industry name
        /// </summary>
        public void Update(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException(nameof(name), "Industry name cannot be empty");
            }

            if (name.Length > 200)
            {
                throw new ValidationException(nameof(name), "Industry name cannot exceed 200 characters");
            }

            Name = name.Trim();
            MarkAsUpdated();
        }
    }
}
