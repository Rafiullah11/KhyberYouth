﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
    }
}
