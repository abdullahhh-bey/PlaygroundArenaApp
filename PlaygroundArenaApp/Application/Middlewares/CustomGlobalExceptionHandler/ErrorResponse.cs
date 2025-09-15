namespace PlaygroundArenaApp.Application.Middlewares.CustomGlobalExceptionHandler
{
    public class ErrorResponse
    {
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }
}
