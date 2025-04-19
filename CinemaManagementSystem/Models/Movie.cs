using System.ComponentModel.DataAnnotations;

namespace CinemaManagementSystem.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }

        public string Title { get; set; }

        public string Genre { get; set; }

        public string Language { get; set; }

        public DateOnly ReleaseDate { get; set; }

        public int Duration { get; set; }

        public ICollection<Screening>? Screenings { get; set; }

        public ICollection<Hall>? Halls { get; set; }
    }
    
    public class MovieDto
    {

        public int MovieId { get; set; }

        public string MovieName { get; set; }

        public string Language { get; set; }

        public int? TotalScreenings { get; set; }
    }
}
