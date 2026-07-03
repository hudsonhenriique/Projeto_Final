var builder = WebApplication.CreateBuilder(args);

// Obtem a URL do frontend atraves das variaveis de ambiente.
// Caso nao exista (como no seu PC local), usa o localhost do Live Server como padrao.
var frontendUrl = builder.Configuration["FRONTEND_URL"] ?? "http://127.0.0.1:5500";

builder.Services.AddCors(options => {
    options.AddPolicy("FrontendPolicy", policy => {
        // Permite apenas as origens especificadas, fechando a brecha de seguranca do SonarCloud
        policy.WithOrigins(frontendUrl, "http://localhost:5500")
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