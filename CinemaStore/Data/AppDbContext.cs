using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CinemaStore.Models;

namespace CinemaStore.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        // public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        // {
        // }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Many-to-Many for Film and Genre
            modelBuilder.Entity<TicketGenre>()
                .HasKey(tg => new { tg.TicketId, tg.GenreId });

            modelBuilder.Entity<TicketGenre>()
                .HasOne(tg => tg.Ticket)
                .WithMany(t => t.TicketGenres)
                .HasForeignKey(tg => tg.TicketId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketGenre>()
                .HasOne(tg => tg.Genre)
                .WithMany(g => g.TicketGenres)
                .HasForeignKey(tg => tg.GenreId).OnDelete(DeleteBehavior.Cascade);

            // Many-to-Many for Film and Format
            modelBuilder.Entity<TicketFormat>()
                .HasKey(tf => new { tf.TicketId, tf.FormatId });

            modelBuilder.Entity<TicketFormat>()
                .HasOne(tf => tf.Ticket)
                .WithMany(t => t.TicketFormats)
                .HasForeignKey(tf => tf.TicketId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketFormat>()
                .HasOne(tf => tf.Format)
                .WithMany(fr => fr.TicketFormats)
                .HasForeignKey(tf => tf.FormatId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketFormatShowTime>()
        .HasOne(tfs => tfs.Format)
        .WithMany(tf => tf.ShowTimes)
        .HasForeignKey(tfs => new { tfs.TicketId, tfs.FormatId }) // Pravi složeni ključ
        .HasPrincipalKey(tf => new { tf.TicketId, tf.FormatId }).OnDelete(DeleteBehavior.Cascade);

            //        modelBuilder.Entity<TicketFormatShowTime>()
            //.HasKey(tfs => tfs.Id);

            //        modelBuilder.Entity<TicketFormatShowTime>()
            //.HasOne(tfs => tfs.TicketFormat)
            //.WithMany(tf => tf.ShowTimes)
            //.HasForeignKey(tfs => new { tfs.TicketFormatId }) // Koristi odgovarajući strani ključ
            //.HasPrincipalKey(tf => new { tf.FormatId });
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Format> Formats { get; set; }
        public DbSet<TicketGenre> TicketGenres { get; set; }
        public DbSet<TicketFormat> TicketFormats { get; set; }
        public DbSet<TicketFormatShowTime> TicketFormatShowTimes { get; set; }

        //public DbSet<TicketFormatShowTime> TicketFormatShowTime { get; set; }


    }
}
