namespace api.Models.Responses
{
    /// <summary>Output for the API endpoints, enforcing a pattern.</summary>
    public class APIResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}