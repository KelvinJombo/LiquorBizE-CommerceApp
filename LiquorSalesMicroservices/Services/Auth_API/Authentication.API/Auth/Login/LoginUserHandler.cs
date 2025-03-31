using Microsoft.AspNetCore.Identity;

namespace Authentication.Auth.Login
{
    public record LoginUserCommand(string Username, string Email, string Password) : ICommand<LoginUserResult>;
    public record LoginUserResult(string Token, string UserId, string CustomerId);


    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        }
    }



    internal class LoginUserCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, IOptions<JwtOptions> jwtOptions) : IRequestHandler<LoginUserCommand, LoginUserResult>
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;

        public async Task<LoginUserResult> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(command.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid login credentials.");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, command.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException("Invalid login credentials.");
            }

            // Generate JWT token
            var token = await GenerateJwtToken(user, user.CustomerId);
            return new LoginUserResult(token, user.Id, user.CustomerId);
        }

        private async Task<string> GenerateJwtToken(User user, string customerId)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_jwtOptions.Secret);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim("CustomerId", user.CustomerId),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256),
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }


}


