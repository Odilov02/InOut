using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebUIServices();
builder.Services.AddApplicationService();
builder.Services.AddSession();
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 2;
        options.Window = TimeSpan.FromSeconds(30);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;

    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

var app = builder.Build();

app.UseRateLimiter();

app.UseExceptionHandler("/User/ErrorHandling");
    app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}");
app.Run();
