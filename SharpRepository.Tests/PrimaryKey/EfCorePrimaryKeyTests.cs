#if NETCOREAPP1_1
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using NUnit.Framework;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.TestObjects.PrimaryKeys;
using Should;

namespace SharpRepository.Tests.PrimaryKey
{
    [TestFixture]
    public class EfCorePrimaryKeyTests
    {
        [Test]
        public void Should_Return_KeyInt2_Property()
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            var repos = new TestEfRepository<ObjectKeys, int>(new DbContext(optionsBuilder.Options));
            var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

            propInfo.PropertyType.ShouldEqual(typeof(int));
            propInfo.Name.ShouldEqual("KeyInt2");
        }

        [Test]
        public void Should_Return_KeyInt1_2_3_Property()
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            var repos = new TestTripleKeyEfRepository<TripleObjectKeys, int, int, int>(new DbContext(optionsBuilder.Options));
            var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

            propInfo[0].PropertyType.ShouldEqual(typeof(int));
            propInfo[0].Name.ShouldEqual("KeyInt1");
            propInfo[1].PropertyType.ShouldEqual(typeof(int));
            propInfo[1].Name.ShouldEqual("KeyInt2");
            propInfo[2].PropertyType.ShouldEqual(typeof(int));
            propInfo[2].Name.ShouldEqual("KeyInt3");
        }
    }

    internal class TestEfRepository<T, TKey> : EfCoreRepository.EfCoreRepository<T, TKey> where T : class, new()
    {
        public TestEfRepository(DbContext dbContext, ICachingStrategy<T, TKey> cachingStrategy = null) : base(dbContext, cachingStrategy)
        {
        }

        public PropertyInfo TestGetPrimaryKeyPropertyInfo()
        {
            return GetPrimaryKeyPropertyInfo();
        }
    }

    internal class TestTripleKeyEfRepository<T, TKey, TKey2, TKey3> : EfCoreRepository.EfCoreRepository<T, TKey, TKey2, TKey3> where T : class, new()
    {
        public TestTripleKeyEfRepository(DbContext dbContext, ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> cachingStrategy = null) : base(dbContext, cachingStrategy)
        {
        }

        public PropertyInfo[] TestGetPrimaryKeyPropertyInfo()
        {
            return GetPrimaryKeyPropertyInfo();
        }
    }
}
#endif