using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using STEMHub.STEMHub_Data.Data;
using STEMHub.STEMHub_Services.Interfaces;
using STEMHub.STEMHub_Services.Services.Email;
using System.Text;
using STEMHub.STEMHub_Services.Services.UserManagement;
using Microsoft.OpenApi.Models;
using System.Diagnostics.Metrics;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Services.Repository;
using STEMHub.STEMHub_Services;
using STEMHub.STEMHub_Services.Services;
using STEMHub.STEMHub_Data.DTO;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);
// Add database
builder.Services.AddDbContext<STEMHubDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("STEMHub")));
// Add access
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .WithOrigins("https://steam.codefirst.id.vn", "http://localhost:3000", "https://stem-ui.vercel.app")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowedToAllowWildcardSubdomains();
    });
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Other identity options
    options.Tokens.ProviderMap["Email"] = new TokenProviderDescriptor(typeof(EmailTokenProvider<ApplicationUser>));
}).AddEntityFrameworkStores<STEMHubDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    // Set the expiration time for the OTP
    options.TokenLifespan = TimeSpan.FromMinutes(1); // Adjust the time span as needed
});


//Add Config for Required Email
builder.Services.Configure<IdentityOptions>(
    opts => opts.SignIn.RequireConfirmedEmail = true
);

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,

        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});


//Add Email Configs
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserManagement, UserManagement>();
builder.Services.AddScoped<IPaginationService<TopicDto>, PaginationService<TopicDto>>();
builder.Services.AddScoped<IPaginationService<NewspaperArticleDto>, PaginationService<NewspaperArticleDto>>();

// Add services Mapper
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Add services repository
builder.Services.AddScoped<ICrudRepository<Banner>, CrudRepository<Banner>>();
builder.Services.AddScoped<ICrudRepository<Lesson>, CrudRepository<Lesson>>();
builder.Services.AddScoped<ICrudRepository<Comment>, CrudRepository<Comment>>();
builder.Services.AddScoped<ICrudRepository<NewspaperArticle>, CrudRepository<NewspaperArticle>>();
builder.Services.AddScoped<ICrudRepository<STEM>, CrudRepository<STEM>>();
builder.Services.AddScoped<ICrudRepository<Topic>, CrudRepository<Topic>>();
builder.Services.AddScoped<ICrudRepository<Video>, CrudRepository<Video>>();
builder.Services.AddScoped<ICrudRepository<Owner>, CrudRepository<Owner>>();
builder.Services.AddScoped<ICrudRepository<Scientist>, CrudRepository<Scientist>>();
builder.Services.AddScoped<ICrudRepository<ApplicationUser>, CrudRepository<ApplicationUser>>();
builder.Services.AddScoped<ICrudUserRepository<IdentityUser>, CrudUserRepository<IdentityUser>>();
builder.Services.AddScoped<ICrudUserRepository<Ingredients>, CrudUserRepository<Ingredients>>();
builder.Services.AddScoped<IGetAllCommentByLessonId, GetAllCommentByLessonId>();
builder.Services.AddScoped<ICrudRepository<Like>, CrudRepository<Like>>();
builder.Services.AddScoped<UnitOfWork>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });
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
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// image
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 200 * 1024 * 1024;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 200 * 1024 * 1024;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
