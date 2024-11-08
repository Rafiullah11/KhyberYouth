using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using KhyberYouth.ViewModel;

namespace KhyberYouth.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<MediaGallery> MediaGalleries { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Project> Projects { get; set; }
         public DbSet<Event> Events { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<BankAccountDetail> BankAccountDetails { get; set; }

    }
}
