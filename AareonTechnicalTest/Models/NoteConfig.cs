using Microsoft.EntityFrameworkCore;

namespace AareonTechnicalTest.Models
{
    public static class NoteConfig
    {

        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(
                entity =>
                {
                    entity.HasKey(e => e.Id);
                });

            modelBuilder.Entity<Ticket>(
                entity =>
                {
                    entity.HasKey(e => e.Id);
                });
        }

    }
}
