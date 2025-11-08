namespace ComplianceAPI.Models
{
    public class Result
    {
        public bool Success { get; set; } = false;

        public string Message { get; set; } = string.Empty;
    }

    public class Result<T> : Result
    {
        public T? Data { get; set; }
    }
}