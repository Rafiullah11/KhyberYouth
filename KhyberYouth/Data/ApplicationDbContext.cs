using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using KhyberYouth.ViewModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using KhyberYouth.Identity;

namespace KhyberYouth.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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
        public DbSet<AboutSection> AboutSections { get; set; }
        public DbSet<MainIcons> MainIcons { get; set; }

        public DbSet<BlogAuthor> BlogAuthors { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: Seed initial data for MainIcons
            modelBuilder.Entity<MainIcons>().HasData(
                new MainIcons
                {
                    Id = 1,
                    Title = "MEDIA",
                    Icons = "fa-solid fa-music",
                    Description = "Explore our journey and see the impact we’re making together.",
                    ControllerName = "MediaGallery",
                    ActionName = "Media",
                },
                new MainIcons
                {
                    Id = 2,
                    Title = "BECOME VOLUNTEER",
                    Icons = "fa-solid fa-bullhorn",
                    Description = "Join us to make a difference! Volunteer today and help bring positive change.",
                    ControllerName = "Volunteers",
                    ActionName = "Create",
                },
                new MainIcons
                {
                    Id = 3,
                    Title = "SEND DONATION",
                    Icons = "fa-solid fa-hand-holding-dollar",
                    Description = "Your donation brings hope and change. Support our mission by donating now.",
                    ControllerName = "Home",
                    ActionName = "Donation",
                }
            );
        }
    }
}