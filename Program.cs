using System.Text;
using api.Data;
using api.Filters;
using api.Interfaces;
using api.Middleware;
using api.Models;
using api.options;
using api.Repositories;
using api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AttachmentsOptions>(builder.Configuration.GetSection("Attachments"));

// var attachementsOptions = builder.Configuration.GetSection("Attachments").Get<AttachmentsOptions>();
// builder.Services.AddSingleton(attachementsOptions);

// var attachementsOptions = new AttachmentsOptions();
// builder.Configuration.GetSection("Attachments").Bind(attachementsOptions);
// builder.Services.AddSingleton(attachementsOptions!);



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<LogActivityFilter>();
})
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;

}).AddEntityFrameworkStores<ApplicationDbContext>();

var jwtOptionsSection = builder.Configuration.GetSection("JWT");

builder.Services.Configure<SalafiJWTOptions>(jwtOptionsSection);

var jwtOptions = jwtOptionsSection.Get<SalafiJWTOptions>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtOptions?.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtOptions?.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtOptions?.SigningKey!)
        ),

        // ValidIssuer = builder.Configuration["JWT:Issuer"],
        // ValidAudience = builder.Configuration["JWT:Audience"],
        // IssuerSigningKey = new SymmetricSecurityKey(
        //     Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]!)
        // ),

    };
});

builder.Services.AddDbContext<ApplicationDbContext>(
// optionos =>
//     {
//         optionos.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//     }
);

builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RateLimitingMiddleware>();
app.UseMiddleware<ProfilingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
