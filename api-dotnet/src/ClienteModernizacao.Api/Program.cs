var builder = WebApplication.CreateBuilder(args);


string frontendUrl = builder.Configuration["FRONTEND_URL"] ?? "http://127.0.0.1:5500";
string localHostUrl = builder.Configuration["LocalHostUrl"] ?? "http://localhost:5500";

builder.Services.AddCors(options => {
    options.AddPolicy("FrontendPolicy", policy => {
        policy.WithOrigins(frontendUrl, localHostUrl)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ClienteModernizacao.Api.Services.IClientIntegrationService, ClienteModernizacao.Api.Services.FileIntegrationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("FrontendPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); 

await app.RunAsync();