﻿using Microsoft.EntityFrameworkCore;
using Pusula.Training.HealthCare.Employees;
using Pusula.Training.HealthCare.Leaves;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Pusula.Training.HealthCare.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class HealthCareDbContext :
    AbpDbContext<HealthCareDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Leave> Leaves { get; set; } = null!;

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }

    public DbSet<IdentitySession> Sessions { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public HealthCareDbContext(DbContextOptions<HealthCareDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */
        if (builder.IsHostDatabase())
        {
            builder.Entity<Employee>(b =>
            {
                b.ToTable(HealthCareConsts.DbTablePrefix + "Employees", HealthCareConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.FirstName).HasColumnName(nameof(Employee.FirstName)).IsRequired()
                    .HasMaxLength(EmployeeConsts.FirstNameMaxLength);
                b.Property(x => x.LastName).HasColumnName(nameof(Employee.LastName)).IsRequired()
                    .HasMaxLength(EmployeeConsts.LastNameMaxLength);
                b.Property(x => x.BirthDate).HasColumnName(nameof(Employee.BirthDate));
                b.Property(x => x.IdentityNumber).HasColumnName(nameof(Employee.IdentityNumber)).IsRequired()
                    .HasMaxLength(EmployeeConsts.IdentityNumberMaxLength);
                b.Property(x => x.EmailAddress).HasColumnName(nameof(Employee.EmailAddress)).IsRequired()
                    .HasMaxLength(EmployeeConsts.EmailAddressMaxLength);
                b.Property(x => x.MobilePhoneNumber).HasColumnName(nameof(Employee.MobilePhoneNumber)).IsRequired()
                    .HasMaxLength(EmployeeConsts.MobilePhoneNumberMaxLength);
                b.Property(x => x.HomePhoneNumber).HasColumnName(nameof(Employee.HomePhoneNumber));
                b.Property(x => x.Gender).HasColumnName(nameof(Employee.Gender)).IsRequired()
                    .HasMaxLength(EmployeeConsts.GenderMaxLength);
                b.Property(x => x.Salary).HasColumnName(nameof(Employee.Salary)).IsRequired()
                    .HasMaxLength(EmployeeConsts.SalaryMaxLength);
            });

            builder.Entity<Leave>(b =>
            {
                b.ToTable(HealthCareConsts.DbTablePrefix + "Leaves", HealthCareConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.StartDate).HasColumnName(nameof(Leave.StartDate)).IsRequired();
                b.Property(x => x.EndDate).HasColumnName(nameof(Leave.EndDate)).IsRequired();
                b.Property(x => x.LeaveType).HasColumnName(nameof(Leave.LeaveType)).IsRequired()
                    .HasMaxLength(LeaveConsts.LeaveTypeMaxLength);
                b.Property(x => x.Status).HasColumnName(nameof(Leave.Status)).IsRequired()
                    .HasMaxLength(LeaveConsts.LeaveStatusMaxLength);

                b.HasOne<Employee>()
                    .WithMany()
                    .HasForeignKey(l => l.EmployeeId).OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}