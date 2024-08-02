using INotificationShared.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace INotificationShared
{
    public static class WebExtensions
    {
        public static WebApplicationBuilder AddServiceDefaults<T>(this WebApplicationBuilder builder)
        {
            builder.Services.AddDefaultOpenApi(builder.Configuration);
            builder.Services.AddAutoAPIController<T>(builder.Configuration);
            return builder;
        }

        public static WebApplication UseServiceDefaults(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //var pathBase = app.Configuration["PATH_BASE"];

            //if (!string.IsNullOrEmpty(pathBase))
            //{
            //    app.UsePathBase(pathBase);
            //    app.UseRouting();

            //    var identitySection = app.Configuration.GetSection("Identity");

            //    if (identitySection.Exists())
            //    {
            //        // We have to add the auth middleware to the pipeline here
            //        app.UseAuthentication();
            //        app.UseAuthorization();
            //    }
            //}

            app.UseDefaultOpenApi(app.Configuration);

            //app.MapDefaultHealthChecks();

            return app;
        }

        public static IServiceCollection AddDefaultOpenApi(this IServiceCollection services, IConfiguration configuration)
        {
            var openApi = configuration.GetSection("OpenApi");

            if (!openApi.Exists())
            {
                return services;
            }

            services.AddEndpointsApiExplorer();

            //services = services.AddSwaggerGen(options =>
            //{
            //    /// {
            //    ///   "OpenApi": {
            //    ///     "Document": {
            //    ///         "Title": ..
            //    ///         "Version": ..
            //    ///         "Description": ..
            //    ///     }
            //    ///   }
            //    /// }
            //    var document = openApi.GetRequiredSection("Document");

            //    var version = document.GetRequiredValue("Version") ?? "v1";

            //    options.SwaggerDoc(version, new OpenApiInfo
            //    {
            //        Title = document.GetRequiredValue("Title"),
            //        Version = version,
            //        Description = document.GetRequiredValue("Description")
            //    });

            //    var identitySection = configuration.GetSection("Identity");

            //    if (!identitySection.Exists())
            //    {
            //        // No identity section, so no authentication open api definition
            //        return;
            //    }

            //    // {
            //    //   "Identity": {
            //    //     "ExternalUrl": "http://identity",
            //    //     "Scopes": {
            //    //         "basket": "Basket API"
            //    //      }
            //    //    }
            //    // }

            //    var identityUrlExternal = identitySection["ExternalUrl"] ?? identitySection.GetRequiredValue("Url");
            //    var scopes = identitySection.GetRequiredSection("Scopes").GetChildren().ToDictionary(p => p.Key, p => p.Value);

            //    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            //    {
            //        Type = SecuritySchemeType.OAuth2,
            //        Flows = new OpenApiOAuthFlows()
            //        {
            //            Implicit = new OpenApiOAuthFlow()
            //            {
            //                AuthorizationUrl = new Uri($"{identityUrlExternal}/connect/authorize"),
            //                TokenUrl = new Uri($"{identityUrlExternal}/connect/token"),
            //                Scopes = scopes,
            //            }
            //        }
            //    });

            //    options.OperationFilter<AuthorizeCheckOperationFilter>();
            //});

            services = services.AddSwaggerGen(
               options =>
               {
                   options.HideAbpEndpoints();
                   options.SwaggerDoc("v1", new OpenApiInfo { Title = "WanZai API", Version = "v1" });
                   options.DocInclusionPredicate((docName, description) => true);
                   //var curr = AppContext.BaseDirectory;
                   //options.IncludeXmlComments($"{curr}/WanZaiShop.{nameof(WebApi)}.xml", true);
                   options.CustomSchemaIds(type => type.FullName);
                   options.EnableAnnotations();
                   //options.SchemaFilter<CustomSchemaFilters>();
                   options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                   {
                       Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                       Name = "Authorization",
                       In = ParameterLocation.Header,
                       Type = SecuritySchemeType.ApiKey,
                       Scheme = "Bearer"
                   });
                   options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    },
                                    Scheme = "oauth2",
                                    Name = "Bearer",
                                    In = ParameterLocation.Header,

                                },
                                new List<string>()
                            }
                   });
               });

            return services;
        }
        public static IServiceCollection AddAutoAPIController<T>(this IServiceCollection services, IConfiguration configuration)
        {
            services = services.Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options
                    .ConventionalControllers
                    .Create(typeof(T).Assembly);
            });
            return services;
        }

        public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app, IConfiguration configuration)
        {
            var openApiSection = configuration.GetSection("OpenApi");

            if (!openApiSection.Exists())
            {
                return app;
            }

            //app.UseSwagger();
            //app.UseSwaggerUI(setup =>
            //{
            //    /// {
            //    ///   "OpenApi": {
            //    ///     "Endpoint: {
            //    ///         "Name": 
            //    ///     },
            //    ///     "Auth": {
            //    ///         "ClientId": ..,
            //    ///         "AppName": ..
            //    ///     }
            //    ///   }
            //    /// }

            //    var pathBase = configuration["PATH_BASE"];
            //    var authSection = openApiSection.GetSection("Auth");
            //    var endpointSection = openApiSection.GetRequiredSection("Endpoint");

            //    var swaggerUrl = endpointSection["Url"] ?? $"{(!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty)}/swagger/v1/swagger.json";

            //    setup.SwaggerEndpoint(swaggerUrl, endpointSection.GetRequiredValue("Name"));

            //    if (authSection.Exists())
            //    {
            //        setup.OAuthClientId(authSection.GetRequiredValue("ClientId"));
            //        setup.OAuthAppName(authSection.GetRequiredValue("AppName"));
            //    }
            //});

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "WanZai API");
            });

            // Add a redirect from the root of the app to the swagger endpoint
            //app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

            return app;
        }
    }
}
