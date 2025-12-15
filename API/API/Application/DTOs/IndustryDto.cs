namespace API.Application.DTOs
{
    public class CreateIndustryRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateIndustryRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    public class IndustryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
