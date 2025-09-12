using Microsoft.EntityFrameworkCore;
using PlaygroundArenaApp.Core.Models;

namespace PlaygroundArenaApp.Infrastructure.Data
{
    public class PlaygroundArenaDbContext : DbContext
    {
        public PlaygroundArenaDbContext(DbContextOptions<PlaygroundArenaDbContext> options) : base(options)
        {
        }

        //Tables
        public DbSet<Arena> Arenas { get; set; }
        public DbSet<Court> Courts { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);  

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId)
                .OnDelete(DeleteBehavior.Cascade);  
        }


    }
}
