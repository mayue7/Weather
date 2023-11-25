using AspNetCoreRateLimit;
using Weather.Interfaces;
using Weather.Services;
using Weather.Commands;

public class Startup
{

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }
    public string  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
            builder =>
            {
                builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
            });
        });

        services.AddHttpClient("WeatherApi", client =>
        {
            client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
        });

        services.AddScoped<IWeatherService, WeatherService>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetCurrentWeatherRequestCommand>());

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Register the rate limiting services
        services.AddMemoryCache();
        services.Configure<ClientRateLimitOptions>(Configuration.GetSection("ClientRateLimiting"));
        services.Configure<ClientRateLimitPolicies>(Configuration.GetSection("ClientRateLimitPolicies"));
        services.AddInMemoryRateLimiting();

        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        //app.UseIpRateLimiting();

        app.UseRouting();
        
        app.UseCors(MyAllowSpecificOrigins);

        app.UseAuthorization();
        app.UseMiddleware<ApiKeyMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
