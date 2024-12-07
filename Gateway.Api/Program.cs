using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddAuthentication(BearerTokenDefaults.AuthenticationScheme).AddBearerToken();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("firstApi-Access", policy =>
    policy.RequireAuthenticatedUser().RequireClaim("firstApi", true.ToString()).Build());
});

var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.MapGet("/login", (bool firstApi) =>
    {
        var claimsPrincipal = new ClaimsPrincipal(
          new ClaimsIdentity(
            new[] {
            new Claim("sub", Guid.NewGuid().ToString()),
            new Claim("firstApi", firstApi.ToString())
            },
            BearerTokenDefaults.AuthenticationScheme));

        return Results.SignIn(claimsPrincipal);
    })
.WithName("login")
.WithOpenApi();



app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

app.Run();
