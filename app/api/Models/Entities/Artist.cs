using System.Collections.Generic;

namespace api.Models.Entities
{
    public class Artist
    {
        public int ArtistId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<Link> Links { get; set; }
    }
}