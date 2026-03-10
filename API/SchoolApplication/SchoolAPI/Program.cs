//using SchoolAPI.Extensions;
//using SchoolAPI.Middleware;

//var builder = WebApplication.CreateBuilder(args);

//// Logging Configuration
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
//builder.Logging.AddDebug();
//if (!builder.Environment.IsDevelopment())
//{
//    builder.Logging.AddEventLog();
//}

//// Dependency Injection
//builder.Services.AddSchoolAcademicsDependencies();

//// Security Configuration
//builder.Services.AddJwtAuthentication(builder.Configuration);
//builder.Services.AddCustomCors(builder.Configuration);

//// API Configuration
//builder.Services.AddControllers()
//    .ConfigureApiBehaviorOptions(options =>
//    {
//        options.SuppressMapClientErrors = false;
//        options.SuppressModelStateInvalidFilter = false;
//    });

//// Swagger Configuration
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        Description = "JWT Authorization header using the Bearer scheme."
//    });

//    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
//    {
//        {
//            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//            {
//                Reference = new Microsoft.OpenApi.Models.OpenApiReference
//                {
//                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            Array.Empty<string>()
//        }
//    });
//});

//var app = builder.Build();

//// Middleware Pipeline
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseGlobalExceptionMiddleware();

//// Disable HTTPS redirect in development
//if (!app.Environment.IsDevelopment())
//{
//    app.UseHttpsRedirection();
//}

//// Authentication and Authorization before CORS
//app.UseAuthentication();
//app.UseAuthorization();

//// CORS after authentication
//app.UseCustomCors();

//app.MapControllers();

//app.Run();


//Current Working Method code

using SchoolAPI.Extensions;
using SchoolAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Logging Configuration
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
if (!builder.Environment.IsDevelopment())
{
    builder.Logging.AddEventLog();
}

// Dependency Injection
builder.Services.AddSchoolAcademicsDependencies();

// Security Configuration
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCustomCors(builder.Configuration);

// API Configuration
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = false;
        options.SuppressModelStateInvalidFilter = false;
    });

// Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// CRITICAL: Configure Kestrel to listen on all network interfaces
// This allows the backend to be accessible from other machines on the network
app.Urls.Clear();
app.Urls.Add("http://0.0.0.0:7070");
Console.WriteLine("🚀 ASP.NET Backend listening on http://0.0.0.0:7070");

// Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionMiddleware();

// Disable HTTPS redirect in development
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}


// CORS after authentication
app.UseCustomCors();

// Authentication and Authorization before CORS
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

var sessionId = Guid.NewGuid().ToString();

