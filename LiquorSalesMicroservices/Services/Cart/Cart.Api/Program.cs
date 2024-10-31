var builder = WebApplication.CreateBuilder(args);

// Add Services to the DI Container
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCarts>().Identity(x => x.UserName);
}).UseLightweightSessions();

var app = builder.Build();

//Configure Http Request Pipeline
app.MapCarter();

app.Run();
