using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BlogContext>(options =>
{
    var version = new MySqlServerVersion(new Version(8, 2, 12)); // burda versiyonu xampptan alýyoruz
    options.UseMySql(builder.Configuration["ConnectionStrings:mySqlConnection"], version);
});

// burda IPostRepository çaðýrýldýðý zaman EfPostRepositoryi göndermesini saðlýyorum yani sanal versiyon ilk yazýlýyor ikinciye gerçeði yazýlarak onu çaðýrma iþlemi yapýlýyor
builder.Services.AddScoped<IPostRepository, EfPostRepository>();
builder.Services.AddScoped<ITagRepository, EfTagRepository>();
builder.Services.AddScoped<ICommentRepository, EfCommentRepository>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();

//autentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();


var app = builder.Build();

SeedData.TestVerileriniDoldur(app);


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();// routing autentication ve authorization'dan önce yazýlmalý
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "post_details",
    pattern: "posts/details/{url}",
    defaults: new {controller ="Post",action="Details"}
);
app.MapControllerRoute(
    name:"posts_by_tag",
    pattern: "posts/tag/{tag}",
    defaults: new { controller = "Post", action = "Index" }
    );


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Post}/{action=Index}/{id?}"
);


app.Run();
