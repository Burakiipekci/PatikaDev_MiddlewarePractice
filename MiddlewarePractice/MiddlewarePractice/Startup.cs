﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MiddlewarePractice.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddlewarePractice
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MiddlewarePractice", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MiddlewarePractice v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            #region app.Run();

            //app.Run(async context => Console.WriteLine("Middleware 1."));
            //app.Run(async context => Console.WriteLine("Middleware 2."));
            #endregion

            #region app.Use();

            //app.Use(async (context, next) =>
            //{
            //    Console.WriteLine("Middleware 1 başladı.");
            //    await next.Invoke(); Console.WriteLine("Middleware 1 Sonlandırılıyor.");
            //});
            //app.Use(async (context, next) =>
            //{
            //    Console.WriteLine("Middleware 2 başladı.");
            //    await next.Invoke(); Console.WriteLine("Middleware 2 Sonlandırılıyor.");
            //});
            //app.Use(async (context, next) =>
            //{
            //    Console.WriteLine("Middleware 3 başladı.");
            //    await next.Invoke(); Console.WriteLine("Middleware 3 Sonlandırılıyor.");
            //});
            app.Use(async (context, next) =>
            {
                Console.WriteLine("Use middleware tetiklendi");
                await next.Invoke();
            });
            #endregion

            #region app.Map();

            app.Map("/example", internalApp => internalApp.Run(async context =>
            {
                Console.WriteLine("/example middleware tetiklendi");
                await context.Response.WriteAsync("/example middleware tetiklendi.");
            }));
            #endregion

            #region app.MapWhen
            app.MapWhen(x => x.Request.Method == "GET", internalApp =>
            {
                internalApp.Run(async context =>
                {
                  
                        Console.WriteLine("MapWhen middleware tetiklendi.");
                         await context.Response.WriteAsync("MapWhen middleware tetiklendi.");    
                });
            });
            #endregion

            #region oluşturduğumuz methodun startupa eklenmesi.
            app.UseMiddlewareTest();
            #endregion




            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
