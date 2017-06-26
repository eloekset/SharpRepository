﻿using NUnit.Framework;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.TestObjects;
using Shouldly;

namespace SharpRepository.Tests.Caching
{
    [TestFixture]
    public class DisabledCacheTests
    {
        [Test]
        public void Using_DisableCaching_Should_Disable_Cache_Inside_Using_Block()
        {
            var repos = new InMemoryRepository<Contact>(new StandardCachingStrategy<Contact>());

            repos.CachingEnabled.ShouldBeTrue();

            using (repos.DisableCaching())
            {
                repos.CachingEnabled.ShouldBeFalse();
            }

            repos.CachingEnabled.ShouldBeTrue();
        }
    }
}
