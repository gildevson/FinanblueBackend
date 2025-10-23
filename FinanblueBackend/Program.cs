using FinanblueBackend.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o serviço Dapper (injeção de dependência)
builder.Services.AddSingleton<DbContextDapper>();

// Controladores e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FinanblueBackend API",
        Version = "v1",
        Description = "API desenvolvida por Gilson Fonseca usando Dapper e SQL Server"
    });
});

var app = builder.Build();

// Ativa HTTPS (opcional mas recomendado)
app.UseHttpsRedirection();

// Middleware de roteamento
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FinanblueBackend API v1");
    });
}

app.MapControllers();
app.Run();
