using System.Text.Json.Serialization;
using Routes.Interfaces;
using Routes.Managers;
using Routes.Middlewares;

namespace Routes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            const string corsPolicyName = "Cors";

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();
            builder.Services.AddCors(
                option => option
                .AddPolicy(corsPolicyName,
                    corsPolicy => corsPolicy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()));

            builder.Services.AddScoped<IOSRMManager, OSRMManager>();
            builder.Services.AddScoped<IVRPSolutionManager, VRPSolutionManager>();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });

            var app = builder.Build();

            app.UseErrorHandling();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(corsPolicyName);
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

            app.MapControllers();

            app.Run();
        }
    }
}
