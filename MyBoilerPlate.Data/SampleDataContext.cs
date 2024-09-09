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
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using MyBoilerPlate.Business.Entities;
using System.Threading.Tasks;
using System.Threading;

namespace MyBoilerPlate.Data
{
    public class SampleDataContext : DbContext
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;

        private bool CanUseSessionContext { get; set; }

        #region Constructors

        public SampleDataContext(DbContextOptions<SampleDataContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _HttpContextAccessor = httpContextAccessor;

            CanUseSessionContext = true;
        }

        #endregion

        #region Tables

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeType> EmployeeTypes { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<ApplicationKey> ApplicationKeys { get; set; }

        #endregion

        #region Fluent API

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TODO: Map Composite Keys Entities Here
            //TODO: Set the .HasNoKey() to views and stored procedures

            //Disable the cascade delete behavior
            var cascadeFKs = modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys())
                            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            //Ignores
            modelBuilder.Ignore<ExtensionDataObject>();
        }

        #endregion

        #region Save Changes
        public override int SaveChanges()
        {
            // Get the entries that are auditable
            var auditableEntitySet = ChangeTracker.Entries<IAuditableEntity>();

            if (auditableEntitySet != null)
            {
                Guid? userId = null;

                if (_HttpContextAccessor.HttpContext.User.Claims.Any(x => x.Type == "sub"))
                {
                    userId = Guid.Parse(_HttpContextAccessor.HttpContext.User.Claims.First(x => x.Type == "sub").Value);
                }

                DateTime currentDate = DateTime.Now;

                // Audit set the audit information foreach record
                foreach (var auditableEntity in auditableEntitySet.Where(c => c.State == EntityState.Added || c.State == EntityState.Modified))
                {
                    if (auditableEntity.State == EntityState.Added)
                    {
                        auditableEntity.Entity.CreatedDate = currentDate;
                        auditableEntity.Entity.CreatedById = userId.Value;
                    }

                    auditableEntity.Entity.UpdatedDate = currentDate;
                    auditableEntity.Entity.UpdatedById = userId;
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Get the entries that are auditable
            var auditableEntitySet = ChangeTracker.Entries<IAuditableEntity>();

            if (auditableEntitySet != null)
            {
                Guid? userId = null;

                if (_HttpContextAccessor.HttpContext.User.Claims.Any(x => x.Type == "sub"))
                {
                    userId = Guid.Parse(_HttpContextAccessor.HttpContext.User.Claims.First(x => x.Type == "sub").Value);
                }

                DateTime currentDate = DateTime.Now;

                // Audit set the audit information foreach record
                foreach (var auditableEntity in auditableEntitySet.Where(c => c.State == EntityState.Added || c.State == EntityState.Modified))
                {
                    if (auditableEntity.State == EntityState.Added)
                    {
                        auditableEntity.Entity.CreatedDate = currentDate;
                        auditableEntity.Entity.CreatedById = userId.Value;
                    }

                    auditableEntity.Entity.UpdatedDate = currentDate;
                    auditableEntity.Entity.UpdatedById = userId;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        #endregion
    }
}
