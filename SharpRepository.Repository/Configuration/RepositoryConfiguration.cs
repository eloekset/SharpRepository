using System;
using System.Collections.Generic;
#if NETSTANDARD
using System.Linq;
using System.Reflection;
#endif

namespace SharpRepository.Repository.Configuration
{
    public class RepositoryConfiguration : IRepositoryConfiguration
    {
        public RepositoryConfiguration()
        {
            Attributes = new Dictionary<string, string>();
        }

        public RepositoryConfiguration(string name)
        {
            Name = name;
            Attributes = new Dictionary<string, string>();
        }

        public string Name { get; set; }

        private Type _factory;
        public Type Factory
        {
            get { return _factory; }
            set
            {
                ConfigurationHelper.CheckForInterface(value, typeof(IConfigRepositoryFactory));

                _factory = value;
            }
        }
#if NETSTANDARD
        public string FactoryType
        {
            get
            {
                return _factory?.AssemblyQualifiedName;
            }
            set
            {
                string[] typeNameAndAssemblyName = value?.Split(',').Select(s => s.Trim()).ToArray();

                if (typeNameAndAssemblyName.Length != 2)
                    throw new Exception("The type name must specify full type name and assembly name");

                var assembly = Assembly.Load(new AssemblyName(typeNameAndAssemblyName[1]));
                Factory = assembly.GetType(typeNameAndAssemblyName[0]);
            }
        }
#endif
        public string CachingStrategy { get; set; }
        public string CachingProvider { get; set; }

        public IDictionary<string, string> Attributes { get; set; }

        public IRepository<T> GetInstance<T>() where T : class, new()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigRepositoryFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T>();
        }

        public IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigRepositoryFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T, TKey>();
        }

        public ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>() where T : class, new()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigRepositoryFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T, TKey, TKey2>();
        }

        public string this[string key]
        {
            get { return Attributes.ContainsKey(key) ? Attributes[key] : null; }
        }
    }
}
