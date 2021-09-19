namespace api.Models.Inputs
{
    /// <summary>DTO to limit data sent in requests.</summary>
    /// <remarks>Main way for prevent over-posting attacks.</remarks>
    public class UserInputModel
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}