using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using NamesApi.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace NamesApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        readonly string MyAllowSpecificOrigins= "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => 
            {
                options.AddPolicy(MyAllowSpecificOrigins, builder => 
                {
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins("http://localhost:3000");
                });
            });
            services.AddDbContext<NamesDbContext>(options => options.UseMySql(Configuration["DBInfo:ConnectionString"]));
            services.AddScoped<INamesRepository, EFNamesRepository>();
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options => 
                {
                    options.SuppressModelStateInvalidFilter = true;
                })
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            SeedData.EnsurePopulated(app);
        }
    }
}
