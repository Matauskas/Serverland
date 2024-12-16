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

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath); // Ensure the base path is correct
builder.Configuration.AddJsonFile("./Startup/Configs/appsettings.json", optional: false, reloadOnChange: true); // Explicitly load the config file

builder.Services

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
    .AddSwaggerExamplesFromAssemblyOf<ListPartDto>()
    .AddSwaggerExamplesFromAssemblyOf<ListServerDto>()
    .AddSwaggerExamplesFromAssemblyOf<ListCategoryDto>()
    .AddSwaggerExamplesFromAssemblyOf<CategoryDtoExample>()
    .AddSwaggerExamplesFromAssemblyOf<ServerDtoExample>()
    .AddSwaggerExamplesFromAssemblyOf<PartDtoExample>()
    .AddSwaggerExamplesFromAssemblyOf<CreatePartDtoExample>()
    .AddSwaggerExamplesFromAssemblyOf<CreateServerDtoExample>()
    .AddSwaggerExamplesFromAssemblyOf<CreateCategoryDtoExample>()
    .AddSwaggerExamplesFromAssemblyOf<UpdatedCategoryDtoExample>()
    .AddSwaggerExamplesFromAssemblyOf<UpdatedServerDtoExample>()
    .AddSwaggerExamplesFromAssemblyOf<UpdatedPartDtoExample>()
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
//var dbContext = scope.ServiceProvider.GetRequiredService<ServerDbContext>();
var dbSeeder = scope.ServiceProvider.GetRequiredService<AuthSeeder>();
await dbSeeder.SeedAsync();

app.AddAuthApi();
app.UseHttpsRedirection();
app.UseStaticFiles(); 
app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        //c.RouteTemplate = "api-docs/{documentName}/swagger.json";
    });
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
app.UseHttpsRedirection();

app.AddServerApi();
app.AddCategoryApi();
app.AddPartApi();
app.UseAuthentication();
app.UseAuthorization();
app.Run();