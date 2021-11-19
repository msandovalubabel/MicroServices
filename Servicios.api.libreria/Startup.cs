using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Servicios.api.libreria.Core;
using Servicios.api.libreria.Core.ContexMongoDB;
using Servicios.api.libreria.Repository;
using Servicios.RabbitMQ.Bus.BusRabbit;
using Servicios.RabbitMQ.Bus.Implement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicios.api.libreria
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
            services.Configure<MongoSettings>(

            #region mongodb
                options =>
                {
                    options.ConnectionString =Configuration.GetSection("MongoDb:ConnectionString").Value;
                    options.Database = Configuration.GetSection("MongoDb:Database").Value; 
                } );

            services.AddSingleton<MongoSettings>();
            #endregion

            #region Context
            services.AddTransient<IAutorContext, AutorContext>();
            #endregion

            #region Repository
            services.AddTransient<IAutorRepository, AutorRepository>();
            #endregion

            #region Generico
            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>)); //Para que reconozca todos los tipos
            #endregion

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Servicios.api.libreria", Version = "v1" });
            });

            #region Cors
            services.AddCors(opt => {

                opt.AddPolicy("CorsRule", rule =>
                {
                    rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
                });

             });
            #endregion
            #region Rabbit
            services.AddSingleton<IRabbitEventBus, RabbitEventBus>(sp => {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitEventBus(sp.GetService<IMediator>(), scopeFactory);
            });
            services.AddMediatR(typeof(RabbitEventBus).Assembly);
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Servicios.api.libreria v1"));
            }

            app.UseRouting();

            app.UseCors("CorsRule"); //aplica regla Cors

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
