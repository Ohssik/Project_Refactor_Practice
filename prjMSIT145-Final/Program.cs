using Microsoft.EntityFrameworkCore;
using prjMSIT145_Final.Models;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

builder.Services.AddDbContext<ispanMsit145shibaContext>(
 options => options.UseSqlServer(
 builder.Configuration.GetConnectionString("ispanMsit145shibaconnection")
));


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(op =>
{
    op.IdleTimeout = TimeSpan.FromMinutes(20);
    op.Cookie.HttpOnly = true;
    op.Cookie.IsEssential = true;
});


//builder.Services.AddDbContext<ispanMsit145shibaContext>(
// options => options.UseSqlServer(
// builder.Configuration.GetConnectionString("ispanMsit145shibaconnection")
//));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();//�ҥ�Session
app.MapHub<ChatHub>("/chatHub");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",

pattern: "{controller=Home}/{action=CIndex}/{id?}");


app.Run();
