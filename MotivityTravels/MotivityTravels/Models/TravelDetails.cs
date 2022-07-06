using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MotivityTravels.Models
{
    public class TravelDetails
    {
        [Key]
        public int Id { get; set; } = 0;
        [DisplayName("From Source")]
        public string FromSource { get; set; } = string.Empty;
        [DisplayName("To Destination")]
        public string ToDestination { get; set; } = string.Empty ;
        [DisplayName("From Date")]
        [Required]
        public DateTime? FromDate { get; set; } 
        [DisplayName("To Date")]
        [Required]
        public DateTime? ToDate { get; set; }
        [DisplayName("Number of adults")]
        public int ParentsCount { get; set; } = 0;
        [DisplayName("Number of childrens")]
        public int ChildernCount { get; set; } = 0;
    }
}
