using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ChemLab.Data.DataContext;
using Microsoft.AspNetCore.Identity;
using ChemLab.Data.Entity;
using ChemLab.Data.Repository.Interfaces;
using ChemLab.Services;
using ChemLab.Services.Interfaces;
using ChemLab.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddControllersWithViews();
var configuration = builder.Configuration;

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ILabPracticeRepository, LabPracticeRepository>();
builder.Services.AddTransient<ILabPracticeService, LabPracticeService>();
builder.Services.AddTransient<ISubGroupMoleculeRepository, SubGroupMoleculeRepository>();


builder.Services.AddAuthentication(options => options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme)
    .AddGoogle(googleOptions => {
        googleOptions.SignInScheme = IdentityConstants.ExternalScheme;
        googleOptions.Events.OnCreatingTicket = context =>
        {
            var identity = (ClaimsIdentity)context.Principal.Identity;
            var picture = context.User.GetProperty("picture").GetString();
            identity.AddClaim(new Claim("picture", picture ?? "/images/user.png"));
            return Task.CompletedTask;
        };
        googleOptions.ClientId = configuration["Google:ClientId"];
        googleOptions.ClientSecret = configuration["Google:ClientSecret"];
    });

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();

app.MapControllerRoute(
    name: "chemeditorget",
    pattern: "chemeditor/get_file/{id}",
    defaults: new { controller = "ChemEditor", action = "GetFile" });

app.MapControllerRoute(
    name: "chemeditor",
    pattern: "chemeditor/save_data/{id}",
    defaults: new { controller = "ChemEditor", action = "SaveChemData" });

app.MapControllerRoute(
    name: "uploadMolecule",
    pattern: "/chemeditor/upload_molecule",
    defaults: new { controller = "ChemEditor", action = "UploadMolecule" });

app.MapControllerRoute(
    name: "uploadMolecule",
    pattern: "/chemeditor/DeleteMolecule/{name}",
    defaults: new { controller = "ChemEditor", action = "DeleteMolecule" });

app.MapControllerRoute(
    name: "GetAllMolecules",
    pattern: "/chemeditor/get_molecules",
    defaults: new { controller = "ChemEditor", action = "GetAllMolecules" });

app.MapControllerRoute(
    name: "pubchem_get_cas",
    pattern: "api/pubchem/get_cas/{smiles}",
    defaults: new { controller = "PubChem", action = "GetChemInfo" });

app.MapControllerRoute(
    name: "chemeditor",
    pattern: "chemeditor/save_text/{id}",
    defaults: new { controller = "ChemEditor", action = "SaveText" });

app.MapControllerRoute(
    name: "chemeditorget",
    pattern: "chemeditor/get_text/{id}",
    defaults: new { controller = "ChemEditor", action = "GetText" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();

