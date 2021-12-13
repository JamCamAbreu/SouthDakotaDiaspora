using Autofac;
using Autofac.Integration.Mvc;
using Data.Services;
using System;
using System.Collections.Generic;
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

            #region User Data
            builder.RegisterType<TestUserData>()
                   .As<IUserData>()
                   .SingleInstance();
            #endregion

            #region Timeline Data
            builder.RegisterType<TestTimelineEventData>()
                .As<ITimelineEventData>()
                .SingleInstance();
            #endregion

            #region Game Data
            builder.RegisterType<TestGameData>()
                .As<IGameData>()
                .SingleInstance();
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