using _3d_pasatiempos_backend.Application.Interfaces.CustomerInterfaces;
using _3d_pasatiempos_backend.Application.Interfaces.ProjectInterfaces;
using _3d_pasatiempos_backend.Application.Interfaces.QuoteInterfaces;
using _3d_pasatiempos_backend.Application.Services;
using _3d_pasatiempos_backend.Infrastructure.Persistence.DataContext;
using _3d_pasatiempos_backend.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//DI
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
builder.Services.AddScoped<IQuoteService, QuoteService>();

//ConnectionString
var ConnectionString = "Host=localhost;Database=tdpasatiempos;Username=postgres;Password=D4rth5h4d0w";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(ConnectionString));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
