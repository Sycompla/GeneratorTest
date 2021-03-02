using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileProviders;
using System.IO;
using CSUsers;

namespace FBClassesODataService
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc();
            services.AddControllers(mvcOptions =>
                mvcOptions.EnableEndpointRouting = false);

            services.AddOData();

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("*");
                                      builder.AllowAnyHeader();
                                      builder.AllowAnyMethod();
                                  });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"Document")),
                RequestPath = new Microsoft.AspNetCore.Http.PathString("/document")
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();
            var builder = new ODataConventionModelBuilder(app.ApplicationServices);

            builder.EntitySet<User>("User");
builder.EntitySet<UserToken>("UserToken");
builder.EntitySet<AuthenticationRequest>("AuthenticationRequest");

            
            
            app.Use((context, next) =>
            {
                context.Response.Headers["Access-Control-Expose-Headers"] = "odata-version";
                context.Response.Headers["Access-Control-Allow-Headers"] = "odata-version, X-CSRF-TOKEN";
                context.Response.Headers["OData-Version"] = "4.0";
                return next.Invoke();
            });

            app.UseODataBatching();

            app.UseMvc(routeBuilder =>
            {
                // and this line to enable OData query option, for example $filter

                routeBuilder.Expand().Select().OrderBy().Filter().MaxTop(null).Count();
                routeBuilder.EnableDependencyInjection();

                routeBuilder.MapODataServiceRoute("ODataRoute", "odata", builder.GetEdmModel(), new DefaultODataBatchHandler());

            });
        }    } //Startup
}