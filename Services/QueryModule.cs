using System.Linq;
using EfPostgre.Pg;
using EfPostgre.Pg.Model;
using Microsoft.EntityFrameworkCore;
using static System.Console;

namespace EfPostgre.Services
{
    public class QueryModule
    {
        private EfTestContext dbContext;

        public QueryModule(EfTestContext ctx)
        {
            dbContext = ctx;
        }

        public void showCategories()
        {
            WriteLine("Categories and how many products they have:");
            // запрос для всех категорий, включающих связанные товары
            //IQueryable<Category> cats = dbContext.Categories; //.Include(c => c.Products);
            IQueryable<Category> cats = dbContext.Categories.Include(c => c.Products);
            foreach (var c in cats) WriteLine($"{c.CategoryName} has {c.Products.Count} products.");
        }

        public void queryProducts()
        {
            WriteLine("Products that cost more than a price, and sorted.");
            string input;
            decimal price;
            do
            {
                Write("Enter a product price: ");
                input = ReadLine();
            } while (!decimal.TryParse(input, out price));

            IQueryable<Product> prods = dbContext.Products
                .Where(product => product.Cost > price)
                .OrderByDescending(product => product.Cost);
            foreach (var item in prods)
                WriteLine(
                    $"{item.ProductID}: {item.ProductName} costs {item.Cost:$#,##0.00} and has {item.Stock} units in stock.");
        }

        public bool AddProduct(string categoryID, string productName, decimal? price)
        {
            var newProduct = new Product
            {
                CategoryID = categoryID,
                ProductName = productName,
                Cost = price
            };
            //пометить товар как отслеживаемый на предмет изменений
            dbContext.Products.Add(newProduct);
            // сохранить отслеживаемые изменения в базе данных
            var affected = dbContext.SaveChanges();
            return affected == 1;
        }

        public void ListProducts()
        {
            {
                WriteLine("----------------------------------------------------------------------- - ");
                WriteLine("| ID | Product Name | Cost | Stock | Disc. |");
                WriteLine("----------------------------------------------------------------------- - ");
                foreach (var item in dbContext.Products.OrderByDescending(p => p.Cost))
                    WriteLine($"| {item.ProductID:000} | {item.ProductName,-35} | {item.Cost,8:$#,##0.00} | " +
                              $"{item.Stock,5} | {item.Discontinued} | ");
                WriteLine("------------------------------------------------------------------------ ");
            }
        }

        public bool IncreaseProductPrice(string name, decimal amount)
        {
            using (var ctx = new DesignTimeDbContextFactory().CreateDbContext(new[] {""}))
            {
                Product updateProduct = ctx.Products.First(
                    p => p.ProductName.StartsWith(name));
                updateProduct.Cost += amount;
                //ctx.Products.Update(updateProduct);
                ctx.Entry(updateProduct).State = EntityState.Modified;
                var affected = ctx.SaveChanges();
                return affected == 1;
            }
        }
    }
}