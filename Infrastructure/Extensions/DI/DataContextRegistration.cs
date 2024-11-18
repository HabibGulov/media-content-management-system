using Microsoft.EntityFrameworkCore;

public static class DbContextRegistration
{
    public static WebApplicationBuilder AddDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationContext>(x =>x.UseNpgsql(builder.Configuration["ConnectionString"]));
        return builder;
    }
}