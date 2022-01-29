using Microsoft.EntityFrameworkCore;
using CountryAPI.Models;
using CountryAPI.Data;

var builder = WebApplication.CreateBuilder(args);
var myAllowSpecificOrigins = "myAllowSpecificOrigins";
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    /*migrate and update database to apply tables
     * dotnet ef migrations add InitialCreate
    dotnet ef database update*/

});
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins, builder =>
    {
        builder.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
}); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//use the static files from wwwroot
app.UseStaticFiles();

app.UseAuthorization();
app.UseCors(myAllowSpecificOrigins);
app.MapControllers();

app.Run();

/* youtube tutorial https://www.youtube.com/watch?v=rzPFEuKlPhM&t=1840s
*/