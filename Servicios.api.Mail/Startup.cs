using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Servicios.api.Mail.ManejadorRabbit;
using Servicios.RabbitMQ.Bus.BusRabbit;
using Servicios.RabbitMQ.Bus.EventoQueue;
using Servicios.RabbitMQ.Bus.Implement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicios.api.Mail
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

            services.AddSingleton<IRabbitEventBus, RabbitEventBus>(sp => {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitEventBus(sp.GetService<IMediator>(), scopeFactory);
            });

            services.AddTransient<EmailEventoManejador>();
            services.AddTransient<IEventoManejador<EmailEventoQueue>, EmailEventoManejador>();

            services.AddMediatR(typeof(RabbitEventBus).Assembly);

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var eventBus = app.ApplicationServices.GetRequiredService<IRabbitEventBus>();
            eventBus.Suscribe<EmailEventoQueue, EmailEventoManejador>();
        }
    }
}
