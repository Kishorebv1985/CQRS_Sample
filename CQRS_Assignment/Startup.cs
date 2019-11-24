using CQRS_Assignment.Commands;
using CQRS_Assignment.Model;
using CQRS_Assignment.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CQRS_Assignment
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IQueriesService, QueriesService>(ctor => new QueriesService(Configuration["ConnectionString"]));
            services.AddScoped<ICommandService, CommandService>();

            services.AddDbContext<EmployeeContext>(optionsAction: optionsBuilder =>
                optionsBuilder.UseSqlServer(Configuration["ConnectionString"],
                optionsAction => optionsAction.MigrationsAssembly(typeof(EmployeeContext).GetTypeInfo().Assembly.GetName().Name)));
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            ConfigureOAuth(services, Configuration);
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v0";
                    document.Info.Title = "Simple CQRS API";
                    document.Info.Description = "A simple ASP.NET Core Web API";
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<EmployeeContext>();
                context.Database.Migrate();
            }
            
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseMvc();
        }

        private void ConfigureOAuth(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration.GetValue<string>("Authentication:AzureADInstance") +
                                        configuration.GetValue<string>("Authentication:TenantId");
                    options.Audience = configuration.GetValue<string>("Authentication:AuthId");
                });
        }
    }
}
