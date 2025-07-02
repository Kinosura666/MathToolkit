using MathCore.Interfaces;
using MathCore.Mappers;
using Web.Interfaces;
using Web.Middleware;
using Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMatrixMapper, MatrixMapper>();
builder.Services.AddScoped<ISortFactory, SortFactory>();
builder.Services.AddScoped<ISortService, SortService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
