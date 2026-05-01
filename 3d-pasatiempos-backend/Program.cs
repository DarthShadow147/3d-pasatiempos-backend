using _3d_pasatiempos_backend.Api.Middleware;
using _3d_pasatiempos_backend.Application.Interfaces.Common;
using _3d_pasatiempos_backend.Application.Interfaces.CustomerInterfaces;
using _3d_pasatiempos_backend.Application.Interfaces.OrderInterfaces;
using _3d_pasatiempos_backend.Application.Interfaces.PaymentInterfaces;
using _3d_pasatiempos_backend.Application.Interfaces.ProductionInterface;
using _3d_pasatiempos_backend.Application.Interfaces.ProjectInterfaces;
using _3d_pasatiempos_backend.Application.Interfaces.QuoteInterfaces;
using _3d_pasatiempos_backend.Application.Services;
using _3d_pasatiempos_backend.Application.Validators;
using _3d_pasatiempos_backend.Infrastructure.Persistence.DataContext;
using _3d_pasatiempos_backend.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePaymentValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProjectValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateQuoteValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateQuoteItemRequestValidator>();

//DI
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IProductionRepository, ProductionRepository>();
builder.Services.AddScoped<IProductionService, ProductionService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
builder.Services.AddScoped<IQuoteService, QuoteService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//ConnectionString
var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(ConnectionString));

//Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

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

//Error middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
