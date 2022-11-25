using la_mia_pizzeria_static.Models.Repository;

var builder = WebApplication.CreateBuilder(args);

//per far partire con db se funziona
builder.Services.AddScoped<IDbPizzaRepository, DbPizzaRepository>();

//per far partire con lista
//builder.Services.AddScoped<IDbPizzaRepository, ListPizzaRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pizza}/{action=Index}/{id?}");

app.Run();
