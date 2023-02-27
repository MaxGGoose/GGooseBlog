using GGooseBlog;
using System.Text;
using GGooseBlog.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

const string corsPolicy = "EnableCORS";

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: corsPolicy, 
            policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            });
    });
}

var connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddLogging();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlite(connection));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuers = new []{ "https://localhost:7070", "http://127.0.0.1:5500" },
            ValidAudiences = new []{ "https://localhost:7070", "http://127.0.0.1:5500" },
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET_KEY") 
                                                                               ?? throw new InvalidOperationException()))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors(corsPolicy);
}

app.UseStatusCodePages(statusCodeContext => {
    var response = statusCodeContext.HttpContext.Response;

    switch (response.StatusCode)
    {
        case StatusCodes.Status401Unauthorized:
            response.Redirect("auth");
            break;
    }

    return Task.CompletedTask;
});
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllerRoute(name: "default", pattern: "/all");

app.Run();