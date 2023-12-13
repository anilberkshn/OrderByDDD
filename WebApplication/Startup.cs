using System;
using System.Net.Http.Headers;
using Application.Common.Clients;
using Application.Common.MessageQ;
using Application.Interfaces;
using Application.Services;
using Application.Validations;
using Infrastructure.Repository;
using Infrastructure.Repository.Context;
using Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using ErrorHandlingMiddleware = Core.Middleware.ErrorHandlingMiddleware;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete("Obsolete")]
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddScoped<ICustomerHttpClient>(sp =>sp.GetRequiredService<CustomerHttpClient>());
            // services.AddHostedService<CustomerConsumer>();
            services.AddHttpClient<CustomerHttpClient>((sp, http) =>
            {
                http.BaseAddress = new Uri("http://localhost:5010/");
                http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            
            services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
            
            services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<GetAllValidation>());
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderModel", Version = "v1" });
            });
            
            var dbSettings = Configuration.GetSection("DatabaseSettings").Get<GenericDatabaseSettings>();
            var client = new MongoClient(dbSettings.ConnectionString);
            var context = new Context(client,dbSettings.DatabaseName);
            
            services.AddSingleton<IMessageProducer, RabbitMqProducer>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddSingleton<IContext, Context>(_ => context);
            services.AddSingleton<IOrderRepository, OrderRepository>();
         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {             
                // app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderModel v1"));
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}