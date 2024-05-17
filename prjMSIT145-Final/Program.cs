using Mapster;
using prjMSIT145Final.DI;
using prjMSIT145Final.Infrastructure.Models;
using prjMSIT145Final.MappingProfile;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddMemoryCache();
//builder.Services.AddMvc();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(op =>
{
    op.IdleTimeout = TimeSpan.FromMinutes(20);
    op.Cookie.HttpOnly = true;
    op.Cookie.IsEssential = true;
});

builder.Services.AddDependencyInjection(builder.Configuration);
var config = TypeAdapterConfig.GlobalSettings;
config.Scan(typeof(MapperConfig).Assembly);
builder.Services.AddSingleton(config);


//Session
builder.Services.AddSession(op =>
{
    op.IdleTimeout = TimeSpan.FromMinutes(20);
    op.Cookie.HttpOnly = true;
    op.Cookie.IsEssential = true;
});

//builder.Services.AddHttpContextAccessor();

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

app.UseSession();
//app.MapHub<ChatHub>("/chatHub");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();



app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",

pattern: "{controller=Admin}/{action=ALogin}/{id?}");


app.Run();
