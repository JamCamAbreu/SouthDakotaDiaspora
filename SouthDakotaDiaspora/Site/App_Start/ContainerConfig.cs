using Autofac;
using Autofac.Integration.Mvc;
using Data.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site
{
    public class ContainerConfig
    {
        /// Purpose: What are the services that you want injected into other pieces of software 
        /// inside this of this application?
        internal static void RegisterContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<SouthDakotaDiasporaDbContext>().InstancePerRequest();

            #region User Data
            builder.RegisterType<SqlUserData>()
                   .As<IUserData>()
                   .InstancePerRequest();
            #endregion

            #region Timeline Data
            builder.RegisterType<SqlTimelineEventData>()
                .As<ITimelineEventData>()
                .InstancePerRequest();
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

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}