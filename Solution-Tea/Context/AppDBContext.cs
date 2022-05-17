using Microsoft.EntityFrameworkCore;
using Solution_Tea.Modells;
using System;
using Type = Solution_Tea.Modells.Type;

namespace Solution_Tea.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Tea> Teas { get; set; }

        public DbSet<Type> Types { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=TeaDB;Trusted_Connection=True");
            optionsBuilder.UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tea>()
                        .HasOne(x => x.Type)
                        .WithMany(x => x.Teas)
                        .HasForeignKey(x => x.TypeId);

            modelBuilder.Entity<Type>()
                        .HasMany(x => x.Teas)
                        .WithOne(x => x.Type)
                        .HasForeignKey(x => x.TypeId);

            InsertTypeDate(modelBuilder);
            InsertTeaData(modelBuilder);
        }

        private void InsertTypeDate(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Type>()
                        .HasData(new Type(id: 1, name: "White"));

            modelBuilder.Entity<Type>()
                        .HasData(new Type(id: 2, name: "Black"));

            modelBuilder.Entity<Type>()
                        .HasData(new Type(id: 3, name: "Green"));

            modelBuilder.Entity<Type>()
                        .HasData(new Type(id: 4, name: "Rooibos"));

            modelBuilder.Entity<Type>()
                        .HasData(new Type(id: 5, name: "Herbal"));
        }

        private void InsertTeaData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tea>()
                        .HasData(new Tea(id: 1, name: "Silver needles", manufacturer: "Herbária", new DateTime(2010, 05, 04), 1));
        }

    }
}
