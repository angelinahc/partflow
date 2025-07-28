using api.data.repositories;
using api.services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Creates a single object for the whole application
builder.Services.AddSingleton<IStationRepository, InMemoryStationRepository>();
builder.Services.AddSingleton<IPartRepository, InMemoryPartRepository>();
builder.Services.AddSingleton<IFlowHistoryRepository, InMemoryFlowHistoryRepository>();

// Scoped: Creates a new object for each HTTP request
builder.Services.AddScoped<IStationService, StationService>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/Teste-inicial", () =>
    {
        return "Tudo certo pra começar!";
    }
)
.WithOpenApi();

app.Run();