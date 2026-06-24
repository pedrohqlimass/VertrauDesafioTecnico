using Microsoft.EntityFrameworkCore;
using VertrauDesafioTecnico.DB;
using VertrauDesafioTecnico.Service;

var builder = WebApplication.CreateBuilder(args);

// DATABASE
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// SERVICES
builder.Services.AddScoped<UserService>();


// CONTROLLERS
builder.Services.AddControllers();

// SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// PIPELINE
// Swagger SEM if (para não dar dor de cabeça em Docker/desafio)
app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// MIGRATIONS AUTOMÁTICAS
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();