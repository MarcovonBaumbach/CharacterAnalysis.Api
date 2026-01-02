using CharacterAnalysis.Infrastructure.SemanticKernel;
using CharacterAnalysis.Api.Application.Analysis;
using CharacterAnalysis.Api.Context;
using CharacterAnalysis.Api.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(p =>
        p.WithOrigins("http://localhost:4200")
         .AllowAnyHeader()
         .AllowAnyMethod());
});

var kernel = KernelFactory.Create(builder.Configuration);
builder.Services.AddSingleton(kernel);
builder.Services.AddScoped<ReflectionService>();
builder.Services.AddSingleton<IShowContextCache, InMemoryShowContextCache>();
builder.Services.AddHttpClient<IContextResearchService, TavilyContextResearchService>();
builder.Services.AddSingleton(new TavilyOptions
{
    ApiKey = builder.Configuration["Tavily:ApiKey"]!
});
builder.Services.AddDbContext<CharacterAnalysisDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
