using Microsoft.AspNetCore.JsonPatch;
using Microsoft.OpenApi.Models;

namespace EmployeeSystemWebApi.Extention
{
    public static class AddSwaggerGen
    {
        public static IServiceCollection AddSwaggerGenService(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "EmployeeManagementAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
                opt.MapType<JsonPatchDocument>(() => new OpenApiSchema
                {
                    Type = "array",
                    Items = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            ["op"] = new OpenApiSchema { Type = "string" },
                            ["path"] = new OpenApiSchema { Type = "string" },
                            ["value"] = new OpenApiSchema { Type = "object" }
                        }
                    }
                });
            });
            return services;
        }
    }
}
