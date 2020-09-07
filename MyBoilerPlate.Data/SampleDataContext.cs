using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Runtime.Serialization;
using Core.Common.Contracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using TechAssist.Business.Entities;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
//using TechAssist.Business.Entities.DTOs;

namespace MyBoilerPlate.Data
{
    public class SampleDataContext : DbContext
    {

        private readonly IUserProfile _UserProfile;

        private bool CanUseSessionContext { get; set; }

        #region Constructors

        public SampleDataContext()
        {
            CanUseSessionContext = true;
        }

        public SampleDataContext(DbContextOptions<SampleDataContext> options, IUserProfile userProfile)
            : base(options)
        {
            _UserProfile = userProfile;

            CanUseSessionContext = true;
        }

        #endregion

        #region Tables

        public virtual DbSet<Employee> Persons { get; set; }

        #endregion

        #region Fluent API

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //This lines map a DB funcion so we can call it inside the linq or lambda function that executes a query to the DB
            modelBuilder.HasDbFunction(typeof(SampleDataContextFunctions).GetMethod(nameof(SampleDataContextFunctions.CalcuateAgeInYearsMonths)))
            .HasTranslation(args => SqlFunctionExpression.Create("dbo",
                                                                 "f_CalcuateAgeInYearsMonths",
                                                                 args,
                                                                 typeof(decimal?),
                                                                 null));

            //Disable the cascade delete behavior
            var cascadeFKs = modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys())
                            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;


            modelBuilder.Ignore<ExtensionDataObject>();
            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region Save Changes
        public override int SaveChanges()
        {
            // Get the entries that are auditable
            var auditableEntitySet = ChangeTracker.Entries<IAuditableEntity>();

            if (auditableEntitySet != null)
            {
                DateTime currentDate = DateTime.Now;

                // Audit set the audit information foreach record
                foreach (var auditableEntity in auditableEntitySet.Where(c => c.State == EntityState.Added || c.State == EntityState.Modified))
                {
                    if (auditableEntity.State == EntityState.Added)
                    {
                        auditableEntity.Entity.CreatedDate = currentDate;
                        auditableEntity.Entity.CreatedById = _UserProfile.UserId;
                    }

                    auditableEntity.Entity.UpdatedDate = currentDate;
                    auditableEntity.Entity.UpdatedById = _UserProfile.UserId;
                }
            }

            return base.SaveChanges();
        }

        #endregion
    }
}
