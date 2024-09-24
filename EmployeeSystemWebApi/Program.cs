
using dotenv.net;
using EmployeeSystem.Provider;
using EmployeeSystemWebApi.Extention;
using Microsoft.EntityFrameworkCore;

DotEnv.Load();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Add services to the container.
builder.Services.AddControllers(options =>
{
    // this is for json patch only
    options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// added the CORS to allow all the origins
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));


// added authentication and authorization service
builder.Services.AddAuthenticationAuthorizationServices();

// getting connection string and connection establishment

// this is from the appSettings file
//var connectionString = builder.Configuration.GetConnectionString("SqlServerConnectionString");

// this is coming from .env file
var connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
// adding other services

builder.Services.AddCustomerServices();


// added custom swagger gen services
builder.Services.AddSwaggerGenService();

var app = builder.Build();

app.UseCors("MyPolicy");
// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{*/
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
//}
//app.MapIdentityApi<IdentityUser>();
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

