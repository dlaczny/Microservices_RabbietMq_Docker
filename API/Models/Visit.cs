using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Visit
    {
        [Key]
        public int Id { get; set; }

        public int Visitors { get; set; }
        public string Country { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "yyyy-MM-ddTHH:mm:ssK}")]
        public DateTime Date { get; set; }
    }
}