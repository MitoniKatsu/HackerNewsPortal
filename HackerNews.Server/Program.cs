
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace HackerNews.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpClient("NewsClient", client =>
            {
                client.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/");
                client.DefaultRequestHeaders.Add("Accept","application/json");
            });

            builder.Services.AddCors(o =>
            {
                o.AddPolicy("open", o =>
                {
                    // normally would not do this, but do not have the origins from the hosted UI yet
                    o.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();

                });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Hacker News Portal API",
                    Description = "A .Net Web API for retrieving the latest Hacker News stories",
                    Contact = new OpenApiContact
                    {
                        Name = "Christian Lundblad",
                        Email = "ctlundblad@gmail.com"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://opensource.org/license/mit")
                    }
                });

                var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xml));
            });

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("open");

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
