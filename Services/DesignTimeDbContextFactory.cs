using EfPostgre.Config;
using EfPostgre.Pg;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace EfPostgre.Services
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EfTestContext>
    {
        public EfTestContext CreateDbContext(string[] args)
        {
            
            PgDbConnection conn = new PgDbConnection();


            //return new EfTestContext(conn.ConnectionString, null);
            var retContext= new EfTestContext(conn.ConnectionString, null);
            var loggerFactory = retContext.GetService<ILoggerFactory>();
            loggerFactory.AddProvider(new ConsoleLoggerProvider());
            return retContext;

        }
    }
}