using System;
using EfPostgre.Services;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace EfPostgre
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Recreate database");
            using (var ctx = new DesignTimeDbContextFactory().CreateDbContext(new []{""}))
            {
                //var loggerFactory = ctx.GetService<ILoggerFactory>();
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();
                Console.WriteLine("Recreate database - done");

                var queryModule = new QueryModule(ctx);
                queryModule.showCategories();
                //queryModule.queryProducts();
                queryModule.AddProduct("2", "Квас", 25m);
                queryModule.ListProducts();

                Console.WriteLine("Increase result: " + queryModule.IncreaseProductPrice("Квас", 35m));
                queryModule.ListProducts();
            }

            Console.ReadKey();
        }

    }
}