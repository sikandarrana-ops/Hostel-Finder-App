using System.ComponentModel.DataAnnotations;

namespace HostelFinderApp.Models
{
    public class Advertisement
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public string UserId { get; set; } = string.Empty;
        
        public string UserName { get; set; } = string.Empty;

        public DateTime DatePosted { get; set; }
        
        public string? ImageUrl { get; set; }
        
        public List<string> ImageUrls { get; set; } = new List<string>();
        
        [Phone]
        public string? ContactPhone { get; set; }
        
        [EmailAddress]
        public string? ContactEmail { get; set; }
    }
}
