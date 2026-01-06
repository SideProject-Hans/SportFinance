using FinanceCenter.Components;
using FinanceCenter.Data;
using FinanceCenter.Repositories;
using FinanceCenter.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

namespace FinanceCenter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 強制啟用 Static Web Assets（解決非 Development 環境的問題）
            if (!builder.Environment.IsDevelopment())
            {
                builder.WebHost.UseStaticWebAssets();
            }

            builder.Services.AddMudServices();

            // 設定 Entity Framework Core with MySQL
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            var serverVersion = ServerVersion.AutoDetect(connectionString);

            builder.Services.AddDbContext<FinanceCenterDbContext>(options =>
                options.UseMySql(connectionString, serverVersion)
                    .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
                    .EnableDetailedErrors(builder.Environment.IsDevelopment()));

            // 註冊 Repository 和 Service
            builder.Services.AddScoped<FinanceRepository>();
            builder.Services.AddScoped<FinanceService>();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
