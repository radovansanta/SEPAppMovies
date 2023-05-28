namespace AppMovies.Models
{
    public class Actor
    {
        public int ActorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Description { get; set;}
        public int NumberOfMovies { get; set;}
        public string Photo { get; set; }
    }
}