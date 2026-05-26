using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using WorkloadPlanner.Data;
using WorkloadPlanner.Hubs;
using WorkloadPlanner.Models;
using WorkloadPlanner.Repositories.ScrumBoards;
using WorkloadPlanner.Repositories.Tickets;
using WorkloadPlanner.Services.Auth;
using WorkloadPlanner.Services.ScrumBoards;
using WorkloadPlanner.Services.Tickets;
using WorkloadPlanner.Services.Users;

var builder = WebApplication.CreateBuilder(args);

//Setup database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Database connection string was not found");
builder.Services.AddDbContext<WorkloadPlannerDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

//Configuration ASP.NET Core Identity 
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<WorkloadPlannerDbContext>()
    .AddDefaultTokenProviders();

//Add cookie authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/auth/login";
    options.LogoutPath = "/auth/logout";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    options.SlidingExpiration = true;

    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
});

//Configure request limiter
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("ApiPolicy",
    limiterOptions =>
    {
        limiterOptions.PermitLimit = 50;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 0;
    });

    options.AddFixedWindowLimiter("AuthPolicy",
    limiterOptions =>
    {
        limiterOptions.PermitLimit = 5;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
    });
});


//Configure CORS
var frontendUrl = builder.Configuration["Frontend:Url"];
Console.WriteLine($"Frontend URL: {frontendUrl}");
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(frontendUrl!)
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

//Configure SignalR
builder.Services.AddSignalR();

//Register custom services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IScrumBoardService, ScrumBoardService>();
builder.Services.AddScoped<IScrumBoardRepository, ScrumBoardRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
//Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
app.UseRateLimiter();
app.MapControllers();

app.MapHub<ScrumboardHub>("/scrumboardHub");

using (var scope =
    app.Services.CreateScope())
{
    var db =
        scope.ServiceProvider
            .GetRequiredService<
                WorkloadPlannerDbContext
            >();

    await db.Database.MigrateAsync();
}

app.Run();
