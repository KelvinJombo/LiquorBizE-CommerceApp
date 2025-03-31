using LiquorSales.Web.Pages;
using System.Net.Http.Headers;

namespace LiquorSales.Web.Services
{
    public interface IAuthService
    {
        [Post("/auth/login")]
        Task<LoginResponse> Login([Body] LoginRequest request);

        [Post("/auth/register")]
        Task<SignUpResponse> SignUp([Body] SignUpRequest request);
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
    }

    public class SignUpResponse
    {
        public string Id { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }


    public class AuthHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"];

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    } 

}

