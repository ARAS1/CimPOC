using System;
using CIM;
using CIM.Model.Models.DataContexts;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace CIM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ModelDbContext dbContext = new ModelDbContext();
            dbContext.Database.Create();
            ConfigureAuth(app);
        }
    }
}
