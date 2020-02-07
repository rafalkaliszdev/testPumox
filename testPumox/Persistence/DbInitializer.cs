using System.Linq;

namespace testPumox.Persistence
{
    public class DbInitializer
    {
        public static void SeedData(EfDbContext context)
        {
            if (context.Company.Any())
                return;
            context.Database.EnsureCreated();
            context.SaveChanges();
        }
    }
}