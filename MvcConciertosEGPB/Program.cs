using Amazon.S3;
using MvcConciertosEGPB.Helpers;
using MvcConciertosEGPB.Models;
using MvcConciertosEGPB.Services;
using Newtonsoft.Json;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string secreto = await HelperSecretManager.GetSecretAsync();
KeyModel model = JsonConvert.DeserializeObject<KeyModel>(secreto);
builder.Services.AddSingleton(x => model);
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddTransient<ServiceStorageS3>();
builder.Services.AddTransient<ServiceApiConciertos>();
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
