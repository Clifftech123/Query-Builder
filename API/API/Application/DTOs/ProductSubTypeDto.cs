namespace API.Application.DTOs
{
    public class CreateProductSubTypeRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid ProductId { get; set; }
    }

    public class UpdateProductSubTypeRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class ProductSubTypeResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
