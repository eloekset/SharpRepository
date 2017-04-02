﻿using System.Collections.Generic;
#if !NETSTANDARD1_6
using System.Runtime.Caching;
#endif
using NUnit.Framework;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using SharpRepository.Tests.TestObjects;
using Should;

namespace SharpRepository.Tests.Caching
{
    [TestFixture]
    public class QueryManagerTests : TestBase
    {
        protected QueryManager<Contact, int> QueryManager;

        [SetUp]
        public void Setup()
        {
#if !NETSTANDARD1_6
            // need to clear out the InMemory cache before each test is run so that each is independent and won't effect the next one
            var cache = MemoryCache.Default;
            foreach (var item in cache)
            {
                cache.Remove(item.Key);
            }
#endif

            QueryManager = new QueryManager<Contact, int>(new StandardCachingStrategy<Contact, int>()
                                                   {
                                                       CachePrefix =
                                                           "#RepoStandardCache"
                                                   });
        }

        [TearDown]
        public void Teardown()
        {
            //Repository = null;
        }

        [Test]
        public void ExecuteGet_Should_Not_Use_Cache()
        {
            QueryManager.ExecuteGet(FakeGet, 1);
            QueryManager.CacheUsed.ShouldBeFalse();
        }

        [Test]
        public void ExecuteGet_Should_Use_Cache_After_First_Call()
        {
            // first time no cache yet
            QueryManager.ExecuteGet(FakeGet, 1);
            QueryManager.CacheUsed.ShouldBeFalse();

            // second time the cache has been populated from the last call
            QueryManager.ExecuteGet(FakeGet, 1);
            QueryManager.CacheUsed.ShouldBeTrue();
        }

        [Test]
        public void ExecuteGet_Cache_Disabled_Should_Not_Use_Cache_After_First_Call()
        {
            // first time no cache yet
            QueryManager.ExecuteGet(FakeGet, 1);
            QueryManager.CacheUsed.ShouldBeFalse();

            // second time the cache has been populated from the last call
            QueryManager.CacheEnabled = false;
            QueryManager.ExecuteGet(FakeGet, 1);
            QueryManager.CacheUsed.ShouldBeFalse();
        }

        [Test]
        public void ExecuteGetAll_Should_Not_Use_Cache()
        {
            QueryManager.ExecuteGetAll(FakeGetAll, null, null);
            QueryManager.CacheUsed.ShouldBeFalse();
        }

        [Test]
        public void ExecuteGetAll_Should_Use_Cache_After_First_Call()
        {
            // first time should not find anything
            QueryManager.ExecuteGetAll(FakeGetAll, null, null);
            QueryManager.CacheUsed.ShouldBeFalse();

            // second time it should be from cache
            QueryManager.ExecuteGetAll(FakeGetAll, null, null);
            QueryManager.CacheUsed.ShouldBeTrue();
        }

        [Test]
        public void ExecuteGetAll_Cache_Disabled_Should_Not_Use_Cache_After_First_Call()
        {
            // first time should not find anything
            QueryManager.ExecuteGetAll(FakeGetAll, null, null);
            QueryManager.CacheUsed.ShouldBeFalse();

            // second time it should be from cache
            QueryManager.CacheEnabled = false;
            QueryManager.ExecuteGetAll(FakeGetAll, null, null);
            QueryManager.CacheUsed.ShouldBeFalse();
        }

        [Test]
        public void ExecuteFindAll_Should_Not_Use_Cache()
        {
            QueryManager.ExecuteFindAll(FakeGetAll, new Specification<Contact>(c => c.ContactId < 10), null, null);
            QueryManager.CacheUsed.ShouldBeFalse();
        }

        [Test]
        public void ExecuteFindAll_Should_Use_Cache_After_First_Call()
        {
            // first time should not find anything
            QueryManager.ExecuteFindAll(FakeGetAll, new Specification<Contact>(c => c.ContactId < 10), null, null);
            QueryManager.CacheUsed.ShouldBeFalse();

            // second time it should be from cache
            QueryManager.ExecuteFindAll(FakeGetAll, new Specification<Contact>(c => c.ContactId < 10), null, null);
            QueryManager.CacheUsed.ShouldBeTrue();
        }

        [Test]
        public void ExecuteFindAll_Cache_Disabled_Should_Not_Use_Cache_After_First_Call()
        {
            // first time should not find anything
            QueryManager.ExecuteFindAll(FakeGetAll, new Specification<Contact>(c => c.ContactId < 10), null, null);
            QueryManager.CacheUsed.ShouldBeFalse();

            // second time it should be from cache
            QueryManager.CacheEnabled = false;
            QueryManager.ExecuteFindAll(FakeGetAll, new Specification<Contact>(c => c.ContactId < 10), null, null);
            QueryManager.CacheUsed.ShouldBeFalse();
        }

        [Test]
        public void ExecuteFind_Should_Not_Use_Cache()
        {
            QueryManager.ExecuteFind(FakeGet, new Specification<Contact>(c => c.ContactId < 10), null, null);
            QueryManager.CacheUsed.ShouldBeFalse();
        }

        [Test]
        public void ExecuteFind_Should_Use_Cache_After_First_Call()
        {
            // first time should not find anything
            QueryManager.ExecuteFind(FakeGet, new Specification<Contact>(c => c.ContactId < 10), null, null);
            QueryManager.CacheUsed.ShouldBeFalse();

            // second time it should be from cache
            QueryManager.ExecuteFind(FakeGet, new Specification<Contact>(c => c.ContactId < 10), null, null);
            QueryManager.CacheUsed.ShouldBeTrue();
        }

        [Test]
        public void ExecuteFind_Cache_Disabled_Should_Not_Use_Cache_After_First_Call()
        {
            // first time should not find anything
            QueryManager.ExecuteFind(FakeGet, new Specification<Contact>(c => c.ContactId < 10), null, null);
            QueryManager.CacheUsed.ShouldBeFalse();

            // second time it should be from cache
            QueryManager.CacheEnabled = false;
            QueryManager.ExecuteFind(FakeGet, new Specification<Contact>(c => c.ContactId < 10), null, null);
            QueryManager.CacheUsed.ShouldBeFalse();
        }


#region fake calls

        public Contact FakeGet()
        {
            return new Contact();
        }

        public IEnumerable<Contact> FakeGetAll()
        {
            return new List<Contact>();
        }

#endregion
    }
}
