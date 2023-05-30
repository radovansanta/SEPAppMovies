namespace AppMovies.Models
{
    public class FavoriteWithMovie
    {
        public int FavoritesId { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public string Picture { get; set; }
    }
}