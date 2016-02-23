using System;
using CIM;
using CIM.Model.Models.Company;
using CIM.Model.Models.DataContexts;
using CIM.Model.Models.Enumeration;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace CIM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
