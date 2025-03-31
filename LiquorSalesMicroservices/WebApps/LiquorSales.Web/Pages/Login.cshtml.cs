using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace LiquorSales.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(IAuthService authService, ILogger<LoginModel> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [BindProperty]
        public LoginRequest LoginRequest { get; set; } = new();

        [BindProperty]
        public SignUpRequest SignUpRequest { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }


        public IActionResult OnGet(string returnUrl = null)
        {
            // Debug log to check if the user is detected as authenticated
            _logger.LogInformation("Checking authentication status. IsAuthenticated: {Status}", User.Identity.IsAuthenticated);

            if (User.Identity.IsAuthenticated)
            {
                // ADD A CHECK TO MAKE SURE AUTHENTICATION IS CORRECTLY WORKING
                _logger.LogWarning("User is marked as authenticated but should not be. Potential authentication issue.");

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToPage("/Index");
            }

            ReturnUrl = returnUrl;
            return Page();
        }


        //public IActionResult OnGet()
        //{
        //    if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
        //    {
        //        return Redirect(ReturnUrl);  // Avoids reloading login page if already logged in
        //    }

        //    return Page();
        //}

        public async Task<IActionResult> OnPostLoginAsync()
        {
            try
            {
                var response = await _authService.Login(LoginRequest);

                if (!string.IsNullOrEmpty(response.Token) && !string.IsNullOrEmpty(response.UserId))
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, LoginRequest.Email),
                new Claim(ClaimTypes.NameIdentifier, response.UserId),
                new Claim("Token", response.Token),
                new Claim("CustomerId", response.CustomerId)  
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), authProperties);

                    Response.Cookies.Append("AuthToken", response.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });

                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }

                    return RedirectToPage("/Index");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, "Login failed.");
                ModelState.AddModelError(string.Empty, "Login failed. Please check your credentials and try again.");
            }

            _logger.LogInformation($"User authenticated: {User.Identity?.IsAuthenticated}");

            return Page();
        }






        public async Task<IActionResult> OnPostSignUpAsync()
        {
            try
            {
                var response = await _authService.SignUp(SignUpRequest);

                if (response.IsSuccess)
                {
                    return RedirectToPage("/Login");
                }

                ModelState.AddModelError(string.Empty, "Sign-up failed.");
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError(string.Empty, $"Sign-up failed: {ex.Content}");
                _logger.LogError(ex, "Sign-up failed.");
            }

            return Page();
        }
    }

    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class SignUpRequest
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
