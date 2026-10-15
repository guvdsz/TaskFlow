using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskFlow.Api.Data;
using TaskFlow.Api.Interfaces.Auth;
using TaskFlow.Api.Models;
using TaskFlow.Api.Services.Auth;

namespace TaskFlow.Api.Extensions;

public static class AuthExtensions
{
    public static void AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        var key = builder.Configuration["JWT:Key"];
        var issuer = builder.Configuration["JWT:Issuer"];
        var audience = builder.Configuration["JWT:Audience"];

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidOperationException("JWT:Key configuration is required.");
        }

        if (string.IsNullOrWhiteSpace(issuer))
        {
            throw new InvalidOperationException("JWT:Issuer configuration is required.");
        }

        if (string.IsNullOrWhiteSpace(audience))
        {
            throw new InvalidOperationException("JWT:Audience configuration is required.");
        }

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(jwtOptions =>
        {
            jwtOptions.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,

                ValidateAudience = true,
                ValidAudience = audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key)),

                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1)
            };
        });

        builder.Services.AddScoped<ITokenService, TokenService>();
    }

    public static void AddIdentityCore(this WebApplicationBuilder builder)
    {
        builder.Services
        .AddIdentityCore<User>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddSignInManager();
    }
}
