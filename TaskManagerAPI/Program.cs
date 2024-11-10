using System.Data;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Npgsql;
using TaskManagerAPI.Core.Mappers;
using TaskManagerAPI.Infrastructure.DataAccess;
using TaskManagerAPI.Infrastructure.Interfaces;
using TaskManagerAPI.Infrastructure.Repositories;
using TaskManagerAPI.Middlewares;
using TaskManagerAPI.Services.Interfaces;
using TaskManagerAPI.Services.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskManagerAPI", Version = "v1" });
    options.UseInlineDefinitionsForEnums();
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Configuração do banco de dados
builder.Services.Configure<DatabaseConfiguration>(builder.Configuration.GetSection("DatabaseConfiguration"));

// Adicionando serviços
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var config = sp.GetRequiredService<IOptions<DatabaseConfiguration>>().Value;
    return new NpgsqlConnection(config.ConnectionString);
});

// Injeção de dependência dos repositórios e serviços
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITodoTaskRepository, TodoTaskRepository>();
builder.Services.AddScoped<IHistoryRepository, HistoryRepository>();
builder.Services.AddScoped<ISqlDataAccess, SqlDataAccess>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITodoTaskService, TodoTaskService>();
builder.Services.AddScoped<IHistoryService, HistoryService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
// Configuração do AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Adicionando controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Construindo a aplicação
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares personalizados
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Aplicação condicional do RoleMiddleware somente para rotas específicas
app.MapWhen(
    context => context.Request.Path.StartsWithSegments("/api/reports"),
    appBuilder => appBuilder.UseMiddleware<RoleMiddleware>("manager")
);

// Configuração de Swagger no ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares padrão da aplicação
app.UseHttpsRedirection();
app.UseAuthorization();

// Mapeamento de controllers
app.MapControllers();

// Iniciando a aplicação
app.Run();