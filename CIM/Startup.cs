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
            ModelDbContext dbContext = new ModelDbContext();
            dbContext.Database.Create();
            Company company = new Company();
            company.AreasOfOfOperation = AreasOfOperation.Australia;
            company.CompanyName = "CompanyName";
            company.CompanyRegistrationNumber = "45341745";
            dbContext.Company.Add(company);
            ConfigureAuth(app);
        }
    }
}
