namespace API.Application.DTOs
{
    public class CreateProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid IndustryId { get; set; }
    }

    public class UpdateProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid IndustryId { get; set; }
        public string IndustryName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
