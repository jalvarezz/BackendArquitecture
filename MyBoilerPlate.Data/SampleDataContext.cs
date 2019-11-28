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
//using TechAssist.Business.Entities.DTOs;

namespace TechAssist.Data
{
    public class SampleDataContext : DbContext
    {

        private readonly IHttpContextAccessor _HttpContextAccessor;

        private bool CanUseSessionContext { get; set; }

        #region Constructors

        public SampleDataContext()
        {
            CanUseSessionContext = true;
        }

        public SampleDataContext(DbContextOptions<SampleDataContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _HttpContextAccessor = httpContextAccessor;

            CanUseSessionContext = true;
        }

        #endregion

        #region Tables

        public virtual DbSet<Person> ValidationMessages { get; set; }

        #endregion

        #region Fluent API

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
                //Get the username that is saving the data
                //Guid userId = Guid.Parse(_HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                Guid userId;
                Claim userClaim = _HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

                if (userClaim != null)
                {
                    userId = Guid.Parse(_HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                else
                {
                    userId = Guid.Parse("00000000-0000-0000-0000-000000000000");
                }

                DateTime currentDate = DateTime.Now;

                // Audit set the audit information foreach record
                foreach (var auditableEntity in auditableEntitySet.Where(c => c.State == EntityState.Added || c.State == EntityState.Modified))
                {
                    if (auditableEntity.State == EntityState.Added)
                    {
                        auditableEntity.Entity.CreatedDate = currentDate;
                        auditableEntity.Entity.CreatedById = userId;
                    }

                    auditableEntity.Entity.UpdatedDate = currentDate;
                    auditableEntity.Entity.UpdatedById = userId;
                }
            }

            return base.SaveChanges();
        }

        #endregion
    }
}
