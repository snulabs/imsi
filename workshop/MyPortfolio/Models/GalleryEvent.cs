namespace MyPortfolio.Models
{
    public class GalleryEvent
    {
        public string? EventId { get; set; }
        public string? Title { get; set; }
        public string? Date { get; set; }
        public string? Year { get; set; }
        public List<string>? Images { get; set; }
        public string? Description { get; set; }
    }
}
