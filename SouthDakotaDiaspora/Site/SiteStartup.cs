using Autofac;
using Autofac.Integration.Mvc;
using Data.Services;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(Site.SiteStartup))]

namespace Site
{
    public class SiteStartup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<SouthDakotaDiasporaDbContext>().InstancePerLifetimeScope();

            #region User Data
            builder.RegisterType<SqlUserData>()
                   .As<IUserData>()
                   .InstancePerRequest();
            #endregion

            #region Timeline Data
            builder.RegisterType<SqlTimelineEventData>()
                .As<ITimelineEventData>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DiscordNotifier>()
                .As<DiscordNotifier>()
                .SingleInstance();

            builder.RegisterType<SqlActivityData>()
                .As<IActivityData>()
                .InstancePerLifetimeScope();
            #endregion

            #region Game Data
            builder.RegisterType<SqlGameData>()
                .As<IGameData>()
                .InstancePerRequest();
            #endregion

            #region Show Data
            #endregion

            #region Book Data
            #endregion

            #region Project Data
            #endregion

            // Build the container:
            var container = builder.Build();

            Hangfire.GlobalConfiguration.Configuration.UseAutofacActivator(container);
            JobActivator.Current = new AutofacJobActivator(container);

            // MVC Dependency Resolver:
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");
            app.UseHangfireAspNet(GetHangfireServers);
            app.UseHangfireDashboard();

            #region Discord Notifier
            RecurringJob.AddOrUpdate<DiscordNotifier>(j => j.SendSoonNotifications(), Cron.Minutely);
            RecurringJob.AddOrUpdate<DiscordNotifier>(j => j.SendStartingNotifications(), Cron.Minutely);
            #endregion
        }

        private IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(
                    ConfigurationManager.ConnectionStrings["SouthDakotaDiasporaDbContext"].ToString(),
                    new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    });

            yield return new BackgroundJobServer();
        }
    }
}
