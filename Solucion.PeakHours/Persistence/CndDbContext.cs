using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SolucionPeakHours.Entities;
using SolucionPeakHours.Models;

namespace SolucionPeakHours.Persistence;

public partial class CndDbContext : IdentityDbContext
{
    public CndDbContext(DbContextOptions<CndDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FactoryStaffEntity> FactoryStaffs { get; set; }
    public virtual DbSet<ProgHourEntity> ProgHours { get; set; }
    public virtual DbSet<WorkHourEntity> WorkHours { get; set; }
    public virtual DbSet<UserIdentityEntity> UserEntity { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
