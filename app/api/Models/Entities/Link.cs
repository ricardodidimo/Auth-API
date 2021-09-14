namespace api.Models.Entities
{
    public class Link
    {
        public int LinkId { get; set; }
        public string url { get; set; }
        public int ArtistId { get; set; }
    }
}