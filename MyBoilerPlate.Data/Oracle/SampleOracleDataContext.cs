using Microsoft.EntityFrameworkCore;
using MyBoilerPlate.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyBoilerPlate.Data.Oracle
{
    public class SampleOracleDataContext : DbContext
    {
        public SampleOracleDataContext(DbContextOptions<SampleOracleDataContext> options)
            : base(options)
        {
        }

        #region Tables

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeType> EmployeeTypes { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<ApplicationKey> ApplicationKeys { get; set; }

        #endregion

        #region Fluent API

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Disable the cascade delete behavior
            var cascadeFKs = modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys())
                            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            //Ignores
            modelBuilder.Ignore<ExtensionDataObject>();
        }

        #endregion
    }
}
