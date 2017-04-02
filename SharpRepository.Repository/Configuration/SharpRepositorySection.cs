﻿using System;
using System.Collections.Generic;
#if NET451
using System.Configuration;
#endif
using System.Linq;

namespace SharpRepository.Repository.Configuration
{
#if NET451
    public class SharpRepositorySection : ConfigurationSection, ISharpRepositoryConfiguration
#elif NETSTANDARD1_6
    public class SharpRepositorySection : ISharpRepositoryConfiguration
#endif
    {
#if NET451
        [ConfigurationProperty("repositories", IsRequired = true)]
#endif
        public RepositoriesCollection Repositories
        {
#if NET451
            get { return (RepositoriesCollection)this["repositories"]; }
#elif NETSTANDARD1_6
            get;
            private set;
#endif
        }

        public bool HasRepository
        {
            get { return Repositories != null && Repositories.Count != 0; }
        }

        public IRepositoryConfiguration GetRepository(string repositoryName)
        {
            if (!HasRepository)
                throw new Exception("There are no repositories configured.");

            if (String.IsNullOrEmpty(repositoryName))
            {
                repositoryName = ((ISharpRepositoryConfiguration) this).DefaultRepository;
            }

            IRepositoryConfiguration repositoryConfiguration = null;

            if (String.IsNullOrEmpty(repositoryName))
            {
                // return the first one
                foreach (IRepositoryConfiguration element in Repositories)
                {
                    repositoryConfiguration = element;
                    break;
                }

                return repositoryConfiguration;
            }
            
            // find the repository element by name
            // NOTE: i've intentionally left it as this loop instead of using LINQ because the .Cast<> slows down performance and I think this is just as readable
            foreach (IRepositoryConfiguration element in Repositories)
            {
                if (element.Name == repositoryName)
                {
                    repositoryConfiguration = element;
                    break;
                }
            }

            // if this is null then throw an error
            if (repositoryConfiguration == null)
            {
                throw new Exception(String.Format("There is no repository configured with the name '{0}'", repositoryName));
            }

            return repositoryConfiguration;
        }

#if NET451
        [ConfigurationProperty("cachingStrategies")]
#endif
        public CachingStrategyCollection CachingStrategies
        {
#if NET451
            get { return (CachingStrategyCollection)this["cachingStrategies"]; }
#elif NETSTANDARD1_6
            get;
            private set;
#endif
        }

        public bool HasCachingStrategies
        {
            get { return CachingStrategies != null && CachingStrategies.Count != 0; }
        }

        public ICachingStrategyConfiguration GetCachingStrategy(string strategyName)
        {
            if (!HasCachingStrategies) return null;

            if (String.IsNullOrEmpty(strategyName))
            {
                strategyName = ((ISharpRepositoryConfiguration) this).DefaultCachingStrategy;
            }

            if (String.IsNullOrEmpty(strategyName))
            {
                return null;
            }

            ICachingStrategyConfiguration strategyConfiguration = null;
            foreach (CachingStrategyElement element in CachingStrategies)
            {
                if (element.Name == strategyName)
                {
                    strategyConfiguration = element;
                    break;
                }
            }

            return strategyConfiguration;
        }

#if NET451
        [ConfigurationProperty("cachingProviders")]
#endif
        public CachingProviderCollection CachingProviders
        {
#if NET451
            get { return (CachingProviderCollection)this["cachingProviders"]; }
#elif NETSTANDARD1_6
            get;
            private set;
#endif
        }

        public bool HasCachingProviders
        {
            get { return CachingProviders != null && CachingProviders.Count != 0; }
        }

        public ICachingProviderConfiguration GetCachingProvider(string providerName)
        {
            if (!HasCachingProviders) return null;

            if (String.IsNullOrEmpty(providerName))
            {
                providerName = ((ISharpRepositoryConfiguration)this).DefaultCachingProvider;
            }

            if (String.IsNullOrEmpty(providerName))
            {
                return null;
            }

            ICachingProviderConfiguration providerConfiguration = null;
            foreach (CachingProviderElement element in CachingProviders)
            {
                if (element.Name == providerName)
                {
                    providerConfiguration = element;
                    break;
                }
            }

            return providerConfiguration;
        }

        public IRepository<T> GetInstance<T>(string repositoryName = null) where T : class, new()
        {
            return ConfigurationHelper.GetInstance<T>(this, repositoryName);
        }

        public IRepository<T, TKey> GetInstance<T, TKey>(string repositoryName = null) where T : class, new()
        {
            return ConfigurationHelper.GetInstance<T, TKey>(this, repositoryName);
        }

        public ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>(string repositoryName = null) where T : class, new()
        {
            return ConfigurationHelper.GetInstance<T, TKey, TKey2>(this, repositoryName);
        }

        public ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>(string repositoryName = null) where T : class, new()
        {
            return ConfigurationHelper.GetInstance<T, TKey, TKey2, TKey3>(this, repositoryName);
        }

        public ICompoundKeyRepository<T> GetCompoundKeyInstance<T>(string repositoryName = null) where T : class, new()
        {
            return ConfigurationHelper.GetCompoundKeyInstance<T>(this, repositoryName);
        }

        IList<IRepositoryConfiguration> ISharpRepositoryConfiguration.Repositories
        {
            get { return Repositories.ToRepositoryConfigurationList(); }
        }

        string ISharpRepositoryConfiguration.DefaultRepository
        {
            get { return Repositories.Default; }
            set { Repositories.Default = value; }
        }

        IList<ICachingStrategyConfiguration> ISharpRepositoryConfiguration.CachingStrategies
        {
            get { return CachingStrategies.ToCachingStrategyConfigurationList(); }
        }

        string ISharpRepositoryConfiguration.DefaultCachingStrategy
        {
            get { return CachingStrategies.Default; }
            set { CachingStrategies.Default = value; }
        }

        IList<ICachingProviderConfiguration> ISharpRepositoryConfiguration.CachingProviders
        {
            get { return CachingProviders.ToCachingProviderConfigurationList(); }
        }

        string ISharpRepositoryConfiguration.DefaultCachingProvider
        {
            get { return CachingProviders.Default; }
            set { CachingProviders.Default = value; }
        }
    }
}
