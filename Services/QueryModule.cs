using System.Linq;
using EfPostgre.Pg;
using EfPostgre.Pg.Model;
using Microsoft.EntityFrameworkCore;
using static System.Console;
using static EfPostgre.Utils.StringUtils;

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

        public bool AddProduct(string categoryID, string productName, decimal? price, short? stock, string productID = null)
        {
            var newProduct = new Product
            {
                //ProductID = ! productID.IsEmpty() ?  productID : null,
                ProductID = productID,
                CategoryID = categoryID,
                ProductName = productName,
                Cost = price,
                Stock = stock
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

        public void JoinGroups()
        {
            // создаем две последовательности, которые хотим присоединить
            var categories = dbContext.Categories.Select( c => new { c.CategoryID, c.CategoryName }).ToArray();
            var products = dbContext.Products.Select(
                p => new {
                    p.ProductID,
                    p.ProductName,
                    p.CategoryID
                }).ToArray();

            // присоединяем каждый товар к своей категории
            var queryJoin = categories.Join(products,
                category => category.CategoryID,
                product => product.CategoryID,
                (c, p) => new {
                    c.CategoryName,
                    p.ProductName,
                    p.ProductID
                }).OrderBy(cp => cp.ProductID);

            foreach (var item in queryJoin)
            {
                WriteLine($"{item.ProductID}: {item.ProductName} is in { item.CategoryName}.");
            }

            // группируем все товары по соответствующим категориям,
            // чтобы вернуть восемь совпадений
            var queryGroup = categories.GroupJoin(products,
                category => category.CategoryID,
                product => product.CategoryID,
                (c, Products) => new {
                    c.CategoryName,
                    Products = Products.OrderBy(prod => prod.ProductName)
                });
            foreach (var item in queryGroup)
            {
                WriteLine($"{item.CategoryName} has { item.Products.Count()} products.");
                foreach (var product in item.Products)
                {
                    WriteLine($" {product.ProductName}");
                }
            }

            WriteLine("Products");
            WriteLine($" Count:{ dbContext.Products.Count()}");
            WriteLine($" Sum of units in stock:{ dbContext.Products.Sum(p => p.Stock):N0}");
            WriteLine($" Average unit price:{ dbContext.Products.Average(p => p.Cost):$#,##0.00}");
            WriteLine($" Value of units in stock:{dbContext.Products.Sum(p => p.Cost * p.Stock):$#,##0.00}");

        }

        public void querySugar()
        {
            var names = new string[] { "Michael", "Pam", "Jim", "Dwight", "Angela", "Klark", "Kevins", "Toby", "Creed" };
            var query = names.Where(name => name.Length > 4).OrderBy(name => name.Length).ThenBy(name => name);
            var query2 = from name in names where name.Length > 4 orderby name.Length, name select name;
            var query3 = (from name in names where name.Length > 4 orderby name.Length, name select name).Skip(2).Take(1);


            PrintToConsole(query);
            PrintToConsole(query2);
            PrintToConsole(query3);
        }
    }
}