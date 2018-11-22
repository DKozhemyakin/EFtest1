using System;
using System.Reflection;
using EfPostgre.Pg.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EfPostgre.Pg
{
    public class EfTestContext : DbContext
    {
        private readonly string connectionString;
        private readonly ILoggerFactory loggerFactory;

        // свойства, сопоставляемые с таблицами в базе данных
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        /// <inheritdoc />
        public EfTestContext(string connectionString, ILoggerFactory loggerFactory)
        {
            this.connectionString = connectionString;
            this.loggerFactory = loggerFactory;
            //_nspkSeedContext = new NspkSeedContext();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString);
            if (loggerFactory != null)
                optionsBuilder.UseLoggerFactory(loggerFactory);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // пример использования Fluent API вместо атрибутов для ограничения
            // имени категории 40 символами
            modelBuilder.Entity<Category>()
                .Property(category => category.CategoryName)
                .IsRequired()
                .HasMaxLength(40);

            // глобальный фильтр для удаления снятых с продажи товаров
            //modelBuilder.Entity<Product>().HasQueryFilter(p => !p.Discontinued);

            SeedDbVersion(modelBuilder);
            SeedCategory(modelBuilder);
            SeedProduct(modelBuilder);

            Console.WriteLine("OnModelCreating - done");
        }
        
        public void SeedDbVersion(ModelBuilder modelBuilder)
        {
            var assemblyVersion = Assembly.GetEntryAssembly().GetName();
            modelBuilder.Entity<DbVersion>().HasData(new {Id = "A", Version = assemblyVersion.Version.ToString()});
        }
        public void SeedCategory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new
                {
                    CategoryID = "1",
                    CategoryName = "Еда",
                    Description = "твердые продукты питания",
                    SortOrder = "0100"
                }, new
                {
                    CategoryID = "2",
                    CategoryName = "Питье",
                    Description = "жидкости",
                    SortOrder = "0100"
                }
            );
        }
        public void SeedProduct(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new
                {
                    ProductID = "10",
                    ProductName = "Хлеб",
                    Cost = (decimal)20,
                    Stock = (short)100,
                    Discontinued = true,
                    CategoryID = "1"
                }, new
                {
                    ProductID = "20",
                    ProductName = "Масло",
                    Cost = (decimal)100,
                    Stock = (short)500,
                    Discontinued = true,
                    CategoryID = "1"
                }, new
                {
                    ProductID = "30",
                    ProductName = "Пепси",
                    Cost = (decimal)50,
                    Stock = (short)150,
                    Discontinued = false,
                    CategoryID = "2"
                }, new
                {
                    ProductID = "40",
                    ProductName = "Кола",
                    Cost = (decimal)55,
                    Stock = (short)200,
                    Discontinued = true,
                    CategoryID = "2"
                }, new
                {
                    ProductID = "50",
                    ProductName = "Минералка",
                    Cost = (decimal)30,
                    Stock = (short)300,
                    Discontinued = false,
                    CategoryID = "2"
                }
            );
        }
    }
}