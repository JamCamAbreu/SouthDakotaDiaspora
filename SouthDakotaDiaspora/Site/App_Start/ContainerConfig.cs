using Autofac;
using Autofac.Integration.Mvc;
using Data.Services;
using Hangfire;
using Hangfire.SqlServer;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        }

    }
}