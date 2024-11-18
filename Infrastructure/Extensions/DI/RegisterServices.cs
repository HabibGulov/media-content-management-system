public static class RegisterServices
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IContentService, ContentService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IContentCategoryService, ContentCategoryService>();

        
        services.AddScoped(typeof(IGenericDeleteRepository<>), typeof(GenericDeleteRepository<>));
        services.AddScoped(typeof(IGenericAddRepository<>), typeof(GenericAddRepository<>));
        services.AddScoped(typeof(IGenericUpdateRepository<>), typeof(GenericUpdateRepository<>));
        services.AddScoped(typeof(IGenericFindRepository<>), typeof(GenericFindRepository<>));
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        return services;
    }
}