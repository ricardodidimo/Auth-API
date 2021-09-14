namespace api.Models.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string username { get; set; }
        public string normalized_username { get; set; }
        public string password { get; set; }
    }
}