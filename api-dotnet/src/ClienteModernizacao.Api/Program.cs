var builder = WebApplication.CreateBuilder(args);


string frontendUrl = builder.Configuration["FRONTEND_URL"] ?? builder.Configuration["LocalDevUrls:IP"] ?? "";
string localHostUrl = builder.Configuration["LocalDevUrls:Localhost"] ?? "";

builder.Services.AddCors(options => {
    options.AddPolicy("FrontendPolicy", policy => {
        var allowedOrigins = new List<string>();

        if (!string.IsNullOrEmpty(frontendUrl))
        {
            allowedOrigins.Add(frontendUrl);
        }

        if (!string.IsNullOrEmpty(localHostUrl))
        {
            allowedOrigins.Add(localHostUrl);
        }

        allowedOrigins.Add("https://projeto-final-sigma-eight.vercel.app");

        policy.WithOrigins(allowedOrigins.ToArray())
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ClienteModernizacao.Api.Services.IClientIntegrationService, ClienteModernizacao.Api.Services.FileIntegrationService>();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        context.Response.Headers["Access-Control-Allow-Origin"] = "https://projeto-final-sigma-eight.vercel.app";
        context.Response.Headers["Access-Control-Allow-Methods"] = "GET,PUT,POST,DELETE,OPTIONS";
        context.Response.Headers["Access-Control-Allow-Headers"] = "Content-Type";

        var errorFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
        var errorMessage = errorFeature?.Error?.Message ?? "Internal Server Error";

        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new { error = errorMessage }));
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("FrontendPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); 
// Trigger analysis
await app.RunAsync();