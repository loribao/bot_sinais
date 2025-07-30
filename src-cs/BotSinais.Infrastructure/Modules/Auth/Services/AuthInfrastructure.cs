using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BotSinais.Infrastructure.Modules.Auth.Services
{
    public static class AuthInfrastructure
    {
        public static IServiceCollection AddAuthKeycloak(this IServiceCollection services, IConfiguration configuration)
        {
            // Verifica se é ambiente de desenvolvimento
            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            
            // Configura autenticação JWT com Keycloak
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddKeycloakWebApi(configuration, configSectionName: "Keycloak", jwtBearerScheme: JwtBearerDefaults.AuthenticationScheme);

            // Configurações adicionais para desenvolvimento
            if (isDevelopment)
            {
                services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = false; // Permite HTTP em desenvolvimento
                    options.Authority = configuration["Keycloak:auth-server-url"] + "realms/" + configuration["Keycloak:realm"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false, // Mais flexível em desenvolvimento
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.FromMinutes(5) // Tolerância para diferenças de relógio
                    };
                });
            }

            // Configura autorização com Keycloak
            services.AddAuthorization()
                .AddKeycloakAuthorization(configuration, configSectionName: "Keycloak");

            return services;
        }

        /// <summary>
        /// Adiciona middleware de tratamento de erros de autenticação
        /// </summary>
        public static IApplicationBuilder UseAuthenticationErrorHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AuthenticationErrorMiddleware>();
        }
    }
}
