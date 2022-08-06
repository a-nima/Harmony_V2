var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// Add to enable Backend to serve Frontend assets
app.UseDefaultFiles();
app.UseStaticFiles();

// Add any Backend managed routes you'd like to handle 404's (https://stackoverflow.com/questions/61268117/how-to-map-fallback-in-asp-net-core-web-api-so-that-blazor-wasm-app-only-interc/61275033#61275033)
app.Map("swagger/{**slug}", HandleApiFallback);
app.Map("api/{**slug}", HandleApiFallback);

// Fallback to the Frontend to complete Routing
app.MapFallbackToFile("{**slug}", "index.html");

app.UseAuthorization();

app.MapControllers();

app.Run();

// Local Function To Handle 404
Task HandleApiFallback(HttpContext context)
{
    context.Response.StatusCode = StatusCodes.Status404NotFound;
    return Task.CompletedTask;
}