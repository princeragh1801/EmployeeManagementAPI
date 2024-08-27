namespace EmployeeSystem.Contract.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public int Status { get; set; } = 200;
        public string Message { get; set; } = "Details fetched";
        public T ?Data { get; set; }

        /*public ApiResponse(bool success, int statusCode, string message, T data)
        {
            Success = success;
            Status = statusCode;
            Message = message;
            Data = data;
        }*/
    }
}
