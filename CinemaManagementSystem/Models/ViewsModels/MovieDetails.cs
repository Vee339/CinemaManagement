namespace CinemaManagementSystem.Models.ViewsModels
{
    public class MovieDetails
    {

        // A movie page must have a Movie - FindMovie(id)
        public required MovieDto Movie { get; set; }

        // A movie page must have the movies associated with it - List Screenings For Movie(id)
        public IEnumerable<ScreeningDto>? Screenings { get; set; }

    }
}
