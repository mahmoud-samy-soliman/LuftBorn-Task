using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Ta3alom.Application.Interfaces;
using Takamol.Application.Clients.CreateClientCommand;
using Takamol.Domain.Entities;
using Takamol.Persistence;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EnvDTE;

namespace Takamol.web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var passwordHasher = new PasswordHasher<IdentityUser>();

            string passwordToHash = "123456";

            string hashedPassword = passwordHasher.HashPassword(null, passwordToHash);

            Console.WriteLine($"Hashed Password for '{passwordToHash}': {hashedPassword}");


            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var connectionString = builder.Configuration.GetConnectionString("ConString");
            builder.Services.AddDbContext<IContext, Context>(options =>
                options.UseSqlServer(connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
                .LogTo(Log => Debug.WriteLine(Log), LogLevel.Information).EnableSensitiveDataLogging());
            #region CORS

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", cors =>
                {
                    cors.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            #endregion
            #region Authentication

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                string keyString = builder.Configuration.GetValue<string>("SecretKey") ?? string.Empty;
                var keyInBytes = Encoding.ASCII.GetBytes(keyString);
                var key = new SymmetricSecurityKey(keyInBytes);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true, // Check token expiration
                    ClockSkew = TimeSpan.Zero // Use zero clock skew for token expiration check
                };
            });

            #endregion

            #region Identity Manager

            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;

                options.User.RequireUniqueEmail = false;
            })
                .AddEntityFrameworkStores<Context>();

            #endregion
            builder.Services.AddDbContext<Context>(options => options.EnableSensitiveDataLogging().UseSqlServer(connectionString));
            builder.Services.AddMediatR(typeof(CreateClientCommand).Assembly);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}