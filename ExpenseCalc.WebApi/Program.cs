using ExpenseCalculation.DAL;
using ExpenseCalculation.DAL.BusinessLogic;
using ExpenseCalculation.DAL.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ExpenseCalcContext>();
builder.Services.AddTransient<ExpenseMemberRepo>();
builder.Services.AddTransient<ExpenseGroupRepo>();
builder.Services.AddTransient<ExpenseCategoryRepo>();
builder.Services.AddTransient<ExpensePaymentRepo>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
