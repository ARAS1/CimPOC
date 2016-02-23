using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIM.Model.Models.Booking;

namespace CIM.Model.Models.DataContexts
{

    public class ModelDbContext : DbContext
    {
        public ModelDbContext() : base("ModelDbConnection")
        {
            Database.SetInitializer<ModelDbContext>(new DropCreateDatabaseAlways<ModelDbContext>());
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 300;
        }

        public virtual DbSet<Booking.Booking>Booking { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<Company.Company> Company { get; set; }
        public virtual DbSet<General.Address> Address { get; set; }
        public virtual DbSet<General.Telephone> Telephone { get; set; }
    }

}
