using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebHangfireDemoSql
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
            services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_110)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            // Add framework services.
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseMvc();
            // BackgroundJob.Enqueue(() => MeuPrimeiroJobFireAndForget());
            //  RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring Job......"), Cron.MinuteInterval(1));

            RecurringJob.AddOrUpdate(() => MeuPrimeiroJobFireAndForget1(), Cron.Minutely());
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring Job1......"), Cron.Hourly());


        }

        public async Task MeuPrimeiroJobFireAndForget ()
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Bem-Vindo ao Hangfire !");
            });

        }

        public async Task MeuPrimeiroJobFireAndForget1()
        {
            await Task.Run(() =>
            {

                Thread.Sleep(120000);
                Console.WriteLine("Bem-Vindo ao Hangfire !");

            });
            //Thread.Sleep(60000);


        }



    }
}
