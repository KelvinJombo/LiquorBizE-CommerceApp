namespace Authentication.Auth.Register
{
    public record CreateUserRequest(string Username, string Email, string Password, string ConfirmPassword);
    public record CreateUserResponse(string Id, bool IsSuccess, string Message);
    public class RegisterUserEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/register", async (CreateUserRequest request, ISender sender) =>
            {

                var command = request.Adapt<CreateUserCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateUserResponse>();

                return Results.Ok(response);


            })
                .AllowAnonymous();
        }
    }
}
