using System.Threading.Tasks;
using InterviewApp.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
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
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Products API", Version = "v1" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Michal K: temporary for Azure deployment quick testing
            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();

                // Michal K: don't like this, I read that now should go to program.cs, however leaved here for this project.
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var repository = scope.ServiceProvider.GetService<IProductsRepository>();
                    InitializeDatabaseAsync(repository).Wait();
                }
            //}
            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products API V1");
            });
        }

        private async Task InitializeDatabaseAsync(IProductsRepository repository)
        {
            await repository.AddNewAsync(new Product { Name = "Product1", Description = "Use carefully!" });
            await repository.AddNewAsync(new Product { Name = "Product2", Description = "Throw away now!" });
        }
    }
}
