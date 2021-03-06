﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace StaticFiles
{
    public class Startup
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDirectoryBrowser();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Default to 'index.html' under 'wwwroot' directory
            app.UseDefaultFiles();

            // Default to 'home.html' under 'wwwroot/views' directory ( *** COMMENT-OUT PREVIOUS LINE OF CODE *** )
            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add(@"views/home.html");
            app.UseDefaultFiles(options);

            // Serve up static files under 'wwwroot' directory
            app.UseStaticFiles();

            // Serve up static files from images folder outside of 'root' directory
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),@"images")),
                RequestPath = new PathString("/img"),

                // Make static files publicly cacheable for 1 hr
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=3600");
                }
            });

            // Allow the user to view the directory & files for images
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "images")),
                RequestPath = new PathString("/img")
            });


            // This combines UseDefaultFiles() and UseStaticFiles()
            // app.UseFileServer();

            // Default hello page
            app.UseWelcomePage("/hello");

            loggerFactory.AddConsole();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"Hello World!");
            });
        }

    }
}
