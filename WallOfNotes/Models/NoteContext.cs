using Microsoft.EntityFrameworkCore;

namespace WallOfNotes.Models
{

    public class NoteContext : DbContext
    {
        public NoteContext(DbContextOptions<NoteContext> options)
            : base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // set up DB table(s) 
            modelBuilder.Entity<Note>().ToTable("Notes");

            // ensures id is auto-generated
            modelBuilder.Entity<Note>().Property(n => n.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("Id");
            base.OnModelCreating(modelBuilder);
        }
    }
}
