using AiKakeiboBackend.Data;
using AiKakeiboBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT サービスの登録
builder.Services.AddScoped<JwtService>();

// Repositories の登録
builder.Services.AddScoped<AiKakeiboBackend.Repositories.IUserRepository, AiKakeiboBackend.Repositories.UserRepository>();
builder.Services.AddScoped<AiKakeiboBackend.Repositories.ICategoryRepository, AiKakeiboBackend.Repositories.CategoryRepository>();
builder.Services.AddScoped<AiKakeiboBackend.Repositories.IIconRepository, AiKakeiboBackend.Repositories.IconRepository>();
builder.Services.AddScoped<AiKakeiboBackend.Repositories.IKakeiboRepository, AiKakeiboBackend.Repositories.KakeiboRepository>();
builder.Services.AddScoped<AiKakeiboBackend.Repositories.INewsletterRepository, AiKakeiboBackend.Repositories.NewsletterRepository>();

// Services の登録
builder.Services.AddScoped<AiKakeiboBackend.Services.IUserService, AiKakeiboBackend.Services.UserService>();
builder.Services.AddScoped<AiKakeiboBackend.Services.ICategoryService, AiKakeiboBackend.Services.CategoryService>();
builder.Services.AddScoped<AiKakeiboBackend.Services.IIconService, AiKakeiboBackend.Services.IconService>();
builder.Services.AddScoped<AiKakeiboBackend.Services.IKakeiboService, AiKakeiboBackend.Services.KakeiboService>();
builder.Services.AddScoped<AiKakeiboBackend.Services.INewsletterService, AiKakeiboBackend.Services.NewsletterService>();

// Configure DbContext
builder.Services.AddDbContext<KakeiboDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT 認証の設定
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
    };
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

var app = builder.Build();

// データベースマイグレーションを自動実行
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<KakeiboDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "データベースマイグレーション中にエラーが発生しました。");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
