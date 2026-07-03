var builder = WebApplication.CreateBuilder(args);


var frontendUrl = builder.Configuration["FRONTEND_URL"] ?? builder.Configuration["FrontendUrl"];
var localHostUrl = builder.Configuration["LocalHostUrl"];

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