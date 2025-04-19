namespace CinemaManagementSystem.Models.ViewsModels
{
    public class MoviesHalls
    {
        public ScreeningDto? Screening { get; set; }
        public IEnumerable<MovieDto>? Movies { get; set; }

        public IEnumerable<Hall>? Halls { get; set; }
    }
}
