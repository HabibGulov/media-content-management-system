using System.Reflection;
using FluentValidation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.AddDbContext();
builder.Services.AddServices();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

WebApplication app = builder.Build();

app.UseStaticFiles();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.UseExceptionHandler("/error");

app.Run();