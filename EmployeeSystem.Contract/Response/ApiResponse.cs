using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeSystem.Contract.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; } = true;
        public int Status { get; set; } = 200;
        public String Message { get; set; }
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
