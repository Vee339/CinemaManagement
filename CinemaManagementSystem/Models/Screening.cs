using CinemaManagementSystem.Data.Migrations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaManagementSystem.Models
{
    public class Screening
    {

        [Key]
        public int ScreeningId { get; set; }

        [ForeignKey("Movie")] 
        public int? MovieId { get; set; }
        public virtual Movie? Movie { get; set; }

        [ForeignKey("Hall")]
        public int? HallId { get; set; }

        public virtual Hall? Hall { get; set; }

        public DateOnly ScreeningDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

    }

    public class ScreeningDto
    {
        public int ScreeningId { get; set; }
        public string Movie { get; set; }

        public string Hall { get; set; }

        public DateOnly ShowDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }
    } 
}
