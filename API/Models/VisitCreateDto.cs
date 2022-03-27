using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class VisitCreateDto
    {
        [Required]
        public uint Visitors { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "yyyy-MM-ddTHH:mm:ssK}")]
        public DateTime Date { get; set; }
    }
}