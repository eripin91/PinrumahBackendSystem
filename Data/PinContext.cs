using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PinBackendSystem.Areas.Identity;
using PinBackendSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace PinBackendSystem.Data
{
    public class PinContext : IdentityDbContext<IdentityUser>
    {
        public PinContext(DbContextOptions<PinContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Listing> Listings { get; set; }
        public DbSet<PinrumahUser> PinrumahUsers { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Listing_feature> Listing_features { get; set; }
        public DbSet<Kota> Kotas { get; set; }
        public DbSet<Kecamatan> Kecamatans { get; set; }
    }
}