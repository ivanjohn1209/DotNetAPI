using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using CountryAPI.Models;

namespace CountryAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options){}

        public DbSet<Inspection> Inspections { get; set; } = null!;
        public DbSet<InspectionType> InspectionTypes { get; set; } = null!;
        public DbSet<Status> Statuses { get; set; } = null!;
        public DbSet<CountryItem> CountryItems { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;

    }
}
