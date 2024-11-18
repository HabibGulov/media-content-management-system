WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.AddDbContext();
builder.Services.AddServices();

WebApplication app = builder.Build();

app.UseStaticFiles();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();