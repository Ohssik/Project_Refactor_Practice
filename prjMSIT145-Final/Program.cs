using Microsoft.EntityFrameworkCore;
using prjMSIT145_Final.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddDbContext<ispanMsit145shibaContext>(
// options => options.UseSqlServer(
// builder.Configuration.GetConnectionString("ispanMsit145shibaConnection")
//));

builder.Services.AddControllersWithViews();

//開啟Session服務 start
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(op => {
    op.IdleTimeout = TimeSpan.FromMinutes(20);
    op.Cookie.HttpOnly = true;
    op.Cookie.IsEssential = true;
});
//開啟Session服務 end


builder.Services.AddDbContext<ispanMsit145shibaContext>(
 options => options.UseSqlServer(
 builder.Configuration.GetConnectionString("localconnection")
));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();//啟用Session

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AdminMembers}/{action=ALogin}/{id?}");

app.Run();
