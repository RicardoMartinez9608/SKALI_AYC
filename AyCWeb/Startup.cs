using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NucleoAyC.Entities;
using System;
using System.IO;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace AyCWeb
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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.AddSingleton<IFileProvider>(
                new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            services.AddRazorPages();
            services.AddSession(
                //options =>
                //{
                //    options.Cookie.HttpOnly = true;
                //    options.Cookie.Name = "AyCWeb.Session";
                //    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                //    options.IdleTimeout = TimeSpan.FromSeconds(60);
                //}
            );

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Fastest);

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = new[]
                {
                    // Default
                    "text/plain",
                    "text/css",
                    "application/javascript",
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/json",
                    "text/json",
                    // Custom
                    "image/svg+xml"
                };
                options.EnableForHttps = true;
            });

            services.AddMemoryCache();

            services.Configure<IISOptions>(options=> { options.ForwardClientCertificate = false; });

            services.AddDbContext<NegocioContexto>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("NegocioContexto")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //para la localizacion
            var enUsCulture = new CultureInfo("es-SV");
            var localizationOptions = new RequestLocalizationOptions()
            {
                SupportedCultures = new List<CultureInfo>()
                {
                    enUsCulture
                },
                        SupportedUICultures = new List<CultureInfo>()
                {
                    enUsCulture
                },
                DefaultRequestCulture = new RequestCulture(enUsCulture),
                FallBackToParentCultures = false,
                FallBackToParentUICultures = false,
                RequestCultureProviders = null
            };

            app.UseRequestLocalization(localizationOptions);
            //para la localizacion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
       //         app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Views/Shared/Error.cshtml");
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Login}/{id?}");
            });
            app.UseCors("CorsPolicy");
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Xss-Protection", "1");
                await next();
            });
            /*por si funciona la rotativa*/
            Rotativa.AspNetCore.RotativaConfiguration.Setup(env);
        }
    }
}
