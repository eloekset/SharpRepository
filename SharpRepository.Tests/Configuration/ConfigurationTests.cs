using System;
using NUnit.Framework;
#if NETSTANDARD1_6
using SharpRepository.EfCoreRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Xml;
#else
using SharpRepository.EfRepository;
#endif
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Configuration;
using SharpRepository.Tests.TestObjects;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository;

namespace SharpRepository.Tests.Configuration
{
    

    [TestFixture]
    public class ConfigurationTests
    {
#if NETSTANDARD1_6
        IConfigurationRoot Configuration;
        ISharpRepositoryConfiguration config;

        public ConfigurationTests()
        {
            var builder = new ConfigurationBuilder()
                               .AddXmlFile("App.config");
            Configuration = builder.Build();
            config = new SharpRepositoryConfiguration();
            ConfigurationBinder.Bind(Configuration, config);
        }
        
#endif
        [Test]
        public void InMemoryConfigurationNoParametersNoKeyTypes()
        {
#if NETSTANDARD1_6
            var repos = RepositoryFactory.GetInstance<Contact>(config);
#else
            var repos = RepositoryFactory.GetInstance<Contact>();
#endif

            if (!(repos is InMemoryRepository<Contact, int>))
            {
                throw new Exception("Not InMemoryRepository");
            }
        }

        [Test]
        public void InMemoryConfigurationNoParameters()
        {
#if NETSTANDARD1_6
            var repos = RepositoryFactory.GetInstance<Contact, string>(config);
#else
            var repos = RepositoryFactory.GetInstance<Contact, string>();
#endif

            if (!(repos is InMemoryRepository<Contact, string>))
            {
                throw new Exception("Not InMemoryRepository");
            }
        }

        [Test]
        public void LoadConfigurationRepositoryByName()
        {
#if NETSTANDARD1_6
            var repos = RepositoryFactory.GetInstance<Contact, string>(config, "efRepos");
#else
            var repos = RepositoryFactory.GetInstance<Contact, string>("efRepos");
#endif

#if NETSTANDARD1_6
            if (!(repos is EfCoreRepository<Contact, string>))
#else
            if (!(repos is EfRepository<Contact, string>))
#endif
            {
                throw new Exception("Not EfRepository");
            }

        }

#if !NETSTANDARD1_6
        [Test]
        public void LoadConfigurationRepositoryBySectionName()
        {
            var repos = RepositoryFactory.GetInstance<Contact, string>("sharpRepository2", null);

            if (!(repos is EfRepository<Contact, string>))
            {
                throw new Exception("Not EfRepository");
            }
        }
#endif

#if !NETSTANDARD1_6
        [Test]
        public void LoadConfigurationRepositoryBySectionAndRepositoryName()
        {
            var repos = RepositoryFactory.GetInstance<Contact, string>("sharpRepository2", "inMem");

            if (!(repos is InMemoryRepository<Contact, string>))
            {
                throw new Exception("Not InMemoryRepository");
            }
        }
#endif

        [Test]
        public void LoadRepositoryDefaultStrategyAndOverrideNone()
        {
#if NETSTANDARD1_6
            var repos = RepositoryFactory.GetInstance<Contact, string>(config);
#else
            var repos = RepositoryFactory.GetInstance<Contact, string>();
#endif

            if (!(repos.CachingStrategy is StandardCachingStrategy<Contact, string>))
            {
                throw new Exception("Not standard caching default");
            }

#if NETSTANDARD1_6
            repos = RepositoryFactory.GetInstance<Contact, string>(config, "inMemoryNoCaching");
#else
            repos = RepositoryFactory.GetInstance<Contact, string>("inMemoryNoCaching");
#endif

            if (!(repos.CachingStrategy is NoCachingStrategy<Contact, string>))
            {
                throw new Exception("Not the override of default for no caching");
            }
        }

        [Test]
        public void LoadInMemoryRepositoryFromConfigurationObject()
        {
            var config = new SharpRepositoryConfiguration();
//            config.AddRepository("default", typeof(InMemoryConfigRepositoryFactory));
            config.AddRepository(new InMemoryRepositoryConfiguration("default"));
            var repos = RepositoryFactory.GetInstance<Contact, string>(config);

            if (!(repos is InMemoryRepository<Contact, string>))
            {
                throw new Exception("Not InMemoryRepository");
            }

            if (!(repos.CachingStrategy is NoCachingStrategy<Contact, string>))
            {
                throw new Exception("not NoCachingStrategy");
            }
        }

        [Test]
        public void LoadEfRepositoryFromConfigurationObject()
        {
            var config = new SharpRepositoryConfiguration();
#if NETSTANDARD1_6
            config.AddRepository(new EfCoreRepositoryConfiguration("default", "DefaultConnection", typeof(TestObjectEntities)));
#else
            config.AddRepository(new EfRepositoryConfiguration("default", "DefaultConnection", typeof(TestObjectEntities)));
#endif
            var repos = RepositoryFactory.GetInstance<Contact, string>(config);

#if NETSTANDARD1_6
            if (!(repos is EfCoreRepository<Contact, string>))
#else
            if (!(repos is EfRepository<Contact, string>))
#endif
            {
                throw new Exception("Not InMemoryRepository");
            }

            if (!(repos.CachingStrategy is NoCachingStrategy<Contact, string>))
            {
                throw new Exception("not NoCachingStrategy");
            }
        }

        [Test]
        public void LoadEfRepositoryAndCachingFromConfigurationObject()
        {
            var config = new SharpRepositoryConfiguration();
            config.AddRepository(new InMemoryRepositoryConfiguration("inMemory", "timeout"));
#if NETSTANDARD1_6
            config.AddRepository(new EfCoreRepositoryConfiguration("ef5", "DefaultConnection", typeof(TestObjectEntities), "standard", "inMemoryProvider"));
#else
            config.AddRepository(new EfRepositoryConfiguration("ef5", "DefaultConnection", typeof(TestObjectEntities), "standard", "inMemoryProvider"));
#endif
            config.DefaultRepository = "ef5";

            config.AddCachingStrategy(new StandardCachingStrategyConfiguration("standard"));
            config.AddCachingStrategy(new TimeoutCachingStrategyConfiguration("timeout", 200));
            config.AddCachingStrategy(new NoCachingStrategyConfiguration("none"));
            
            config.AddCachingProvider(new InMemoryCachingProviderConfiguration("inMemoryProvider"));

            var repos = RepositoryFactory.GetInstance<Contact, string>(config);

#if NETSTANDARD1_6
            if (!(repos is EfCoreRepository<Contact, string>))
#else
            if (!(repos is EfRepository<Contact, string>))
#endif
            {
                throw new Exception("Not InMemoryRepository");
            }

            if (!(repos.CachingStrategy is StandardCachingStrategy<Contact, string>))
            {
                throw new Exception("not StandardCachingStrategy");
            }
        }

        [Test]
        public void TestFactoryOverloadMethod()
        {
#if NETSTANDARD1_6
            var repos = RepositoryFactory.GetInstance(typeof(Contact), typeof(string), config);
#else
            var repos = RepositoryFactory.GetInstance(typeof (Contact), typeof (string));
#endif

            if (!(repos is InMemoryRepository<Contact, string>))
            {
                throw new Exception("Not InMemoryRepository");
            }
        }

        [Test]
        public void TestFactoryOverloadMethodForCompoundKey()
        {
#if NETSTANDARD1_6
            var repos = RepositoryFactory.GetInstance(typeof(Contact), typeof(string), typeof(string), config);
#else
            var repos = RepositoryFactory.GetInstance(typeof (Contact), typeof (string), typeof(string));
#endif

            if (!(repos is InMemoryRepository<Contact, string, string>))
            {
                throw new Exception("Not InMemoryRepository");
            }
        }

        [Test]
        public void TestFactoryOverloadMethodForTripleCompoundKey()
        {
#if NETSTANDARD1_6
            var repos = RepositoryFactory.GetInstance(typeof(Contact), typeof(string), typeof(string), typeof(string), config);
#else
            var repos = RepositoryFactory.GetInstance(typeof(Contact), typeof(string), typeof(string), typeof(string));
#endif

            if (!(repos is InMemoryRepository<Contact, string, string, string>))
            {
                throw new Exception("Not InMemoryRepository");
            }
        }

        [Test]
        public void TestFactoryOverloadMethodForNoGenericsCompoundKey()
        {
#if NETSTANDARD1_6
            var repos = RepositoryFactory.GetCompoundKeyInstance(typeof(Contact), config);
#else
            var repos = RepositoryFactory.GetCompoundKeyInstance(typeof(Contact));
#endif

            if (!(repos is InMemoryCompoundKeyRepository<Contact>))
            {
                throw new Exception("Not InMemoryRepository");
            }
        }
    }
}
