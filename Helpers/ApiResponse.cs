namespace CrudApi.Helpers
{
    public class ApiResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public T Data { get; set; }

        public ApiResponse(string message, int statusCode, T data)
        {
            Message = message;
            StatusCode = statusCode;
            Data = data;
        }
    }
}