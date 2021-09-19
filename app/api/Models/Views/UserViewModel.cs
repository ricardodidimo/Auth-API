namespace api.Models.Views
{
    /// <summary>DTO to limit data sent in response</summary>
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string username { get; set; }
    }
}