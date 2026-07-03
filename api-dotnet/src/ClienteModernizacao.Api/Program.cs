var builder = WebApplication.CreateBuilder(args);


string frontendUrl = builder.Configuration["FRONTEND_URL"] ?? builder.Configuration["LocalDevUrls:IP"] ?? "";
string localHostUrl = builder.Configuration["LocalDevUrls:Localhost"] ?? "";

builder.Services.AddCors(options => {
    options.AddPolicy("FrontendPolicy", policy => {

        if (!string.IsNullOrEmpty(frontendUrl) && !string.IsNullOrEmpty(localHostUrl))
        {
            policy.WithOrigins(frontendUrl, localHostUrl)
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
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