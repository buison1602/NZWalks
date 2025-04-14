using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        // Đại diện cho middleware kế tiếp trong pipeline. Middleware cần _next để tiếp tục luồng xử lý.
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger,
            RequestDelegate next) 
        {
            this._logger = logger;
            this._next = next;
        }

        // HttpContext là đối tượng đại diện cho toàn bộ thông tin của 1 request HTTP hiện tại – từ lúc
        // client gửi request cho tới khi server trả response.
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // _next là delegate đại diện cho middleware kế tiếp(ví dụ các middleware AuthenticationMiddleware,
                // AuthorizationMiddleware, ... ở trong program.cs).
                // httpContext là dữ liệu của request hiện tại.
                // Khi gọi await _next(...), tức là chuyển request xuống middleware tiếp theo trong pipeline.
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();

                // Log This Exception 
                _logger.LogError(ex, $"{errorId} : {ex.Message}");

                // Return A Custom Error Response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    // Ghi log lỗi kèm theo ID lỗi (dễ truy vết).
                    Id = errorId,
                    ErrorMessage = "Some went wrong! We are looking into resolving this",
                };

                await httpContext.Response.WriteAsJsonAsync(error);


            }
        }
    }
}
