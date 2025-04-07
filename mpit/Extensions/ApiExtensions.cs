using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using mpit.mpit.Infastructure.Auth;

namespace mpit.Extensions
{
    public static class ApiExtensions
    {
        public static void AddAuthentication(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
            var scheme = JwtBearerDefaults.AuthenticationScheme;

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = scheme;
                    options.DefaultSignInScheme = scheme;
                    options.DefaultChallengeScheme = scheme;
                })
                .AddJwtBearer(
                    scheme,
                    options =>
                    {
                        options.RequireHttpsMetadata = true;
                        options.SaveToken = true;

                        options.TokenValidationParameters =
                            JwtParameters.GetTokenValidationParameters(jwtOptions!);

                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                context.Token = context.Request.Cookies["auth"];

                                return Task.CompletedTask;
                            },
                        };
                    }
                );

            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddSingleton<
                IAuthorizationPolicyProvider,
                PermissionAuthorizationPolicyProvider
            >();
            services.AddAuthorization();
        }
    }
}
