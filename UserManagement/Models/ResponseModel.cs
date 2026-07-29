namespace UserManagement.Models
{
    public class ResponseModel<T>
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public T? Data { get; set; }
    }
}
