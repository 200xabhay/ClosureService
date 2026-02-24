using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClosureServices.Application.DTO
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public ErrorDetails? Error { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Request successful")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Error = null
            };
        }

        public static ApiResponse<T> ErrorResponse(string message, string code = "", string details = "")
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
                Error = new ErrorDetails
                {
                    Code = code,
                    Details = details
                }
            };
        }
    }

    public class ErrorDetails
    {
        public string Code { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }

    public class ApiResponse : ApiResponse<object>
    {
        public static ApiResponse SuccessMessage(string message = "Request successful")
        {
            return new ApiResponse
            {
                Success = true,
                Message = message,
                Data = null,
                Error = null
            };
        }

        public static new ApiResponse ErrorResponse(string message, string code = "", string details = "")
        {
            return new ApiResponse
            {
                Success = false,
                Message = message,
                Data = null,
                Error = new ErrorDetails
                {
                    Code = code,
                    Details = details
                }
            };
        }

    }
}
