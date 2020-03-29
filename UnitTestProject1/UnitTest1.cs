using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [Fact]
        public void Add_writes_to_database()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            // Run the test against one instance of the context
            using (var context = new ApplicationDbContext(options))
            {

            }

        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
