namespace Authentication.Auth.Register
{
    // Command Definition
    public record CreateUserCommand(string Username, string Email, string Password, string ConfirmPassword) : ICommand<CreateUserResult>;

    // Command Result
    public record CreateUserResult(string Id, bool IsSuccess, string Message);

    // Command Validator
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .Length(2, 50).WithMessage("Username must be between 2 and 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Your email Address is required.")
                .Length(2, 30).WithMessage("email Address must be between 2 and 30 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Length(6, 100).WithMessage("Password must be between 6 and 100 characters.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords must match.");
        }
    }




    internal class CreateUserCommandHandler(
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager
) : IRequestHandler<CreateUserCommand, CreateUserResult>
    {
        public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            // Check if the user already exists by email
            var existingUser = await userManager.FindByEmailAsync(command.Email);
            if (existingUser != null)
            {
                return new CreateUserResult(string.Empty, false, "A user with this email address already exists.");
            }

            // Create a new user
            var user = new User
            {
                UserName = command.Username,
                Email = command.Email,
                CreatedAt = DateTime.UtcNow
            };

            // Save the user to the database with hashed password
            var result = await userManager.CreateAsync(user, command.Password);
            if (!result.Succeeded)
            {
                return new CreateUserResult(string.Empty, false, $"User creation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            // Assign default role
            string defaultRole = "RegularUser";
            if (!await roleManager.RoleExistsAsync(defaultRole))
            {
                await roleManager.CreateAsync(new IdentityRole(defaultRole));
            }

            await userManager.AddToRoleAsync(user, defaultRole);

            return new CreateUserResult(user.Id, true, "User registered successfully.");
        }
    }



}
