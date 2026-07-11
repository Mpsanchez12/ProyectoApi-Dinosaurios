using DinoArgentoApi.Repositories;
using DinoArgentoApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Resend;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "DinoArgento API", Version = "v1" });
    options.AddSecurityDefinition("token", new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Name = "Authorization",
        Scheme = "bearer"
    });
    options.OperationFilter<DinoArgentoApi.Utils.AuthOperationFilter>();
});

// 2. Registro de Repositorios y Servicios
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDinosaurioRepository, DinosaurioRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<EncoderService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<DinosaurioService>();
builder.Services.AddScoped<DietaService>();
builder.Services.AddScoped<PeriodoService>();

// 3. Resend y Otros
builder.Services.AddHttpClient<IResend, ResendClient>();
builder.Services.AddOptions();
builder.Services.Configure<ResendClientOptions>(options => { options.ApiToken = builder.Configuration["Resend:ApiKey"]; });
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<DinoArgentoApi.Config.AppDbContext>(opt => { opt.UseSqlServer(builder.Configuration.GetConnectionString("devConnection")); });

// 4. Autenticación
var jwtKey = builder.Configuration["Secrets:jwt"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
        new BadRequestObjectResult(new { Errors = context.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
});

var app = builder.Build();

// 5. Middlewares
app.UseCors(options => options.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod().AllowCredentials());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();