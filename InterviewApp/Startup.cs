﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterviewApp.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using InterviewApp.Core;
using InterviewApp.Models;

namespace InterviewApp
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
            services.AddDbContext<ProductsContext>(opt => opt.UseInMemoryDatabase("ProductsList"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped<IProductsRepository, EntityFrameworkProductsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Michal K: don't like this, I read that now should go to program.cs, however leaved here for this project.
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var repository = scope.ServiceProvider.GetService<IProductsRepository>();
                    InitializeDatabaseAsync(repository).Wait();
                }
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private async Task InitializeDatabaseAsync(IProductsRepository repository)
        {
            await repository.AddNewAsync(new Product { Name = "Product1", Description = "Use carefully!" });
            await repository.AddNewAsync(new Product { Name = "Product2", Description = "Throw away now!" });
        }
    }
}