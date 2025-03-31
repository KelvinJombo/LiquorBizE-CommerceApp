namespace Authentication.Auth.Login
{
    public record LoginRequest(string Username, string Email, string Password);
    public record LoginUserResponse(string Token, string UserId, string CustomerId);
    public class LoginUserEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/login", async (LoginRequest request, ISender sender) =>
            {
                var command = request.Adapt<LoginUserCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<LoginUserResponse>();

                return Results.Ok(response);
                
            })
                .AllowAnonymous();

        }
        
    }
}
