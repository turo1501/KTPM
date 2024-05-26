using System;

namespace SneakerStore.Tests.Controllers
{
    internal class DbContextOptionsBuilder<T>
    {
        public DbContextOptionsBuilder()
        {
        }

        internal object UseInMemoryDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }
    }
}