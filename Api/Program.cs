using Api.DataAccess.Db;
using Api.DataAccess.Repositories;
using Api.Mappers;
using Api.Middleware;
using Api.Options;
using Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register DbContext (use In-Memory database for simplicity)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("EmployeePaycheckDb"));

// Register dependencies
AddServices(builder); 

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Employee Benefit Cost Calculation Api",
        Description = "Api to support employee benefit cost calculations"
    });
});

var allowLocalhost = "allow localhost";
builder.Services.AddCors(options =>
{
    options.AddPolicy(allowLocalhost,
        policy => { policy.WithOrigins("http://localhost:3000", "http://localhost"); });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    AddTestDataToDb(app);
}

app.UseCors(allowLocalhost);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Use Middlewares
AddMiddlewares(app);

app.Run();

static void AddServices(WebApplicationBuilder builder)
{
    builder.Services.Configure<PaycheckSettings>(builder.Configuration.GetSection(nameof(PaycheckSettings)));

    builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    builder.Services.AddScoped<IDependentRepository, DependentRepository>();

    builder.Services.AddScoped<IEmployeeService, EmployeeService>();
    builder.Services.AddScoped<IDependentService, DependentService>();
    builder.Services.AddScoped<IPaycheckCalculator, PaycheckCalculator>();
    builder.Services.AddScoped<IYearIntervalProvider, YearIntervalProvider>();

    builder.Services.AddAutoMapper(typeof(EmployeeProfile), typeof(DependentProfile));
}

static void AddMiddlewares(WebApplication app)
{
    app.UseMiddleware<ErrorHandlingMiddleware>();
}

static void AddTestDataToDb(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();
        DbInitializer.Initialize(context);
    }
}