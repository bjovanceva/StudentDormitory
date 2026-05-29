using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentDormitoryApp.Domain.DomainModels;
using StudentDormitoryApp.Domain.Email;
using StudentDormitoryApp.Domain.Identity;
using StudentDormitoryApp.Repository;
using StudentDormitoryApp.Repository.Implementations;
using StudentDormitoryApp.Repository.Interfaces;
using StudentDormitoryApp.Service.Implementations;
using StudentDormitoryApp.Service.Interfaces;
using StudentDormitoryApp.Web.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    //options.UseSqlServer(connectionString));
    options.UseNpgsql(connectionString));


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity<StudentDormitoryAppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));

builder.Services.AddTransient<IStudentDormitoryService, StudentDormitoryService>();
builder.Services.AddTransient<IRoomService, RoomService>();
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IRoomImageService, RoomImageService>();
builder.Services.AddTransient<IDocumentService, DocumentService>();
builder.Services.AddTransient<IApplicationService, ApplicationService>();

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));


//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.LoginPath = "/Account/Login";          // redirect here if not logged in
//    options.AccessDeniedPath = "/Account/AccessDenied"; // redirect here if user lacks role
//});



builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    await DbInitializer.SeedRolesAndAdmin(scope.ServiceProvider);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
