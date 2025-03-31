using System.Diagnostics;

namespace LiquorSales.Web.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public int StatusCode { get; private set; }
        public string? ErrorMessage { get; private set; }

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(int? statusCode = null)
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            StatusCode = statusCode ?? HttpContext.Response.StatusCode;

            switch (StatusCode)
            {
                case 401:
                    ErrorMessage = "Unauthorized access. Please log in.";
                    break;
                case 403:
                    ErrorMessage = "Access forbidden. You don't have permission to access this resource.";
                    break;
                case 404:
                    ErrorMessage = "The requested resource could not be found.";
                    break;
                case 500:
                    ErrorMessage = "An internal server error occurred. Please try again later.";
                    break;
                default:
                    ErrorMessage = "An unexpected error occurred.";
                    break;
            }

            _logger.LogError("Error encountered: {StatusCode} - {ErrorMessage}", StatusCode, ErrorMessage);
        }
    }
}
