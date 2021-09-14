namespace api.Models.Responses
{
    public class APIResponse<T>
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}