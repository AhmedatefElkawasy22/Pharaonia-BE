namespace Pharaonia.API.Extentions
{
    public static class SwaggerExtension
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            // Enable API Explorer for Swagger/OpenAPI
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pharaonia API", Version = "v1" });
            });
            services.AddSwaggerGen(swagger =>
            {
                //This?is?to?generate?the?Default?UI?of?Swagger?Documentation????
                swagger.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ASP.NET?5?Web?API",
                    Description = " ITI Projrcy"
                });

                //?To?Enable?authorization?using?Swagger?(JWT)????
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter?'Bearer'?[space]?and?then?your?valid?token?in?the?text?input?below.\r\n\r\nExample:?\"Bearer?eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                    }
                    },
                    new string[] {}
                    }
                });
            });

        }
    }
}
