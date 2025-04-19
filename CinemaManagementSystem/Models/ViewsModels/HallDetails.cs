namespace CinemaManagementSystem.Models.ViewsModels
{
    public class HallDetails
    {
        // A hall page must have the information about the hall
        public required Hall Hall { get; set; }

        public IEnumerable<ScreeningDto>? HallScreenings { get; set; }
    }
}
