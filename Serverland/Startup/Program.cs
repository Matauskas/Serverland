using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using Serverland.Data;
using Serverland.Data.DatabaseObjects;
using Serverland.Data.Entities;
using Serverland.Examples;
using Serverland.Factories;
using Serverland.Extensions;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serverland.Auth.Model;
using Serverland.Auth.Model;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serverland.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);
builder.Configuration.AddJsonFile("./Startup/Configs/appsettings.json", optional: false, reloadOnChange: true);

builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("AllowLocalhost3000", policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
    })
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(c =>
    {
        c.EnableAnnotations();
        c.ExampleFilters();
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "ServerLand API", Version = "v1" });
    })
    .AddSwaggerExamplesFromAssemblyOf<Program>()
    .AddDbContext<ServerDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")))
    .AddValidatorsFromAssemblyContaining<Program>()
    .AddFluentValidationAutoValidation(configuration =>
    {
        configuration.OverrideDefaultResultFactoryWith<ProblemDetails>();
    })
    .AddIdentity<ShopUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<ServerDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.MapInboundClaims = false;
    options.TokenValidationParameters.ValidAudience = builder.Configuration["Jwt:ValidAudience"];
    options.TokenValidationParameters.ValidIssuer = builder.Configuration["Jwt:ValidIssuer"];
    options.TokenValidationParameters.IssuerSigningKey =
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]));
});

builder.Services
    .AddAuthorization()
    .AddScoped<AuthSeeder>()
    .AddTransient<JwtTokenService>()
    .AddTransient<SessionService>();

var app = builder.Build();

using var scope = app.Services.CreateScope();

var dbContext = scope.ServiceProvider.GetRequiredService<ServerDbContext>();
dbContext.Database.Migrate();


var dbSeeder = scope.ServiceProvider.GetRequiredService<AuthSeeder>();
await dbSeeder.SeedAsync();

app.AddAuthApi();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowLocalhost3000");
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        c.IndexStream = () => executingAssembly.GetManifestResourceStream($"{executingAssembly.GetName().Name}.wwwroot.swagger.custom-index.html");
        c.InjectStylesheet($"./style.css");
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        c.DocumentTitle = "Forum API V1";
        c.DefaultModelExpandDepth(2);
        c.DefaultModelsExpandDepth(-1);
        c.DocExpansion(DocExpansion.List);
        c.MaxDisplayedTags(5);
        c.DisplayOperationId();
        c.DisplayRequestDuration();
        c.DefaultModelRendering(ModelRendering.Example);
        c.ShowExtensions();
    });
}

app.AddServerApi();
app.AddCategoryApi();
app.AddPartApi();
app.Run();
