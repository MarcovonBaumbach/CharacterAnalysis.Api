using CharacterAnalysis.Infrastructure.SemanticKernel;
using CharacterAnalysis.Api.Application.Analysis;

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
builder.Services.AddSingleton<ReflectionService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
