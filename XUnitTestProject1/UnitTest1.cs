using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using WanderlustRealms.Data;
using WanderlustRealms.Services;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var service = new ShopService(context);

                
            }
        }
    }
}
