using System.ComponentModel.DataAnnotations;

namespace WorkerService.Models
{
    public class VisitCreateDto
    {
        [Required]
        public uint Visitors { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ssK}")]
        public DateTime Date { get; set; }
    }
}