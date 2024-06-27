using System.Text.Json;

namespace MinimalApiPractice
{
    public class Result<T> : IResult
    {
        public readonly string _message;
        public readonly bool _isSuccess;
        public readonly T? _data;

        public Result() { }

        public Result(T? data, string message, bool isSuccess)
        {
            _data = data;
            _message = message;
            _isSuccess = isSuccess;
        }

        public Result(T? data)
        {
            _data = data;
            _isSuccess = true;
        }

        public async Task ExecuteAsync(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(new 
            { 
                data = _data,
                isSuccess = _isSuccess,
                message = _message
            });
            await httpContext.Response.WriteAsync(json);
        }
    }
}
