using ChatTest.App.Hubs;
using ChatTest.App.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChatTest.App
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true );
            services.AddSpaStaticFiles(configuration => configuration.RootPath = "ClientApp/dist");

            services.AddMemoryCache();

            services.AddSignalR();

            services.AddSingleton<IUserNameGenerator, UserNameGenerator>();
            services.AddSingleton<ITokenGenerator, TokenGenerator>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IConversationService, ConversationService>();
            services.AddSingleton<IMessangesService, MessangesService>();
            services.AddSingleton<ISeeder, ApplicationSeeder>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISeeder seeder)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller}/{action=Index}/{id?}");
                endpoints.MapHub<ChatHub>("/hub");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                    spa.UseAngularCliServer(npmScript: "start");
            });

            seeder.Seed();
        }
    }
}
