var builder = WebApplication.CreateBuilder(args);

// Add Services to the DI Container


var app = builder.Build();

 //Configure Http Request Pipeline

app.Run();
