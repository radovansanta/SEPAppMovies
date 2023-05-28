using System;

namespace AppMovies.Models
{
    public class Director
    {
        public int DirectorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BornDate { get; set; }
        public DateTime DiedDate { get; set; }
        
        public int Age { get; set; }
        public int NumberOfMovies { get; set; }
        public string Description { get; set; }
        public string Photo { get; set; }
    }
}