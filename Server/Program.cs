using CharacterViewer.Core.Providers;
using DBCD.Providers;
using Microsoft.AspNetCore.Cors.Infrastructure;
using ModelViewer.Core.Components;
using ModelViewer.Core.Providers;
using WoWFileFormats.Interfaces;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddHttpClient();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add service singletons
            builder.Services.AddSingleton<IFileDataProvider, CASCFileDataProvider>();
            //builder.Services.AddSingleton<IFileDataProvider, TACTSharpFileDataProvider>();
            builder.Services.AddSingleton<IDBCDStorageProvider, DBCDStorageProvider>();

            //Add Transients;
            builder.Services.AddTransient<IDBCProvider, FileDataDBCProvider>();
            builder.Services.AddTransient<IDBDProvider, GithubDBDProvider>();
            builder.Services.AddTransient<DBCD.DBCD>();

            // Add components
            var components= typeof(IComponent).Assembly.GetTypes()
                .Where(x => x.GetInterfaces().Contains(typeof(IComponent)));
            foreach (var component in components)
            {
                builder.Services.AddTransient(component);
            }

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddCors((opt) =>
                {
                    opt.AddDefaultPolicy((builder) =>
                    {
                        builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                    });
                });
            }

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors();
            } else
            {
                app.UseHttpsRedirection();
            }


            app.UseAuthorization();


            app.MapControllers();

            // Preload 
            app.Services.GetService<IFileDataProvider>();
            app.Run();
        }
    }
}
