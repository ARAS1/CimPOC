namespace CIM.Model
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class CIMDatabase : DbContext
    {
        // Your context has been configured to use a 'CIMDatabase' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'CIM.Model.CIMDatabase' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'CIMDatabase' 
        // connection string in the application configuration file.
        public CIMDatabase()
            : base("name=CIMDatabase")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

         public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    public class MyEntity
    {
       public int Id { get; set; }
        public string Name { get; set; }
    }
}