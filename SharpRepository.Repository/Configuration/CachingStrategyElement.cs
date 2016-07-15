﻿using System;
using System.Collections.Generic;
#if NET451
using System.ComponentModel;
using System.Configuration;
#endif
using SharpRepository.Repository.Caching;

namespace SharpRepository.Repository.Configuration
{
#if NET451
    public class CachingStrategyElement : ConfigurationElement, ICachingStrategyConfiguration
#elif NETSTANDARD
    public class CachingStrategyElement : ICachingStrategyConfiguration
#endif
    {
        private IDictionary<string, string> _attributes = new Dictionary<string, string>();

#if NET451
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
#endif
        public string Name
        {
#if NET451
            get { return (string)base["name"]; }
            set { base["name"] = value; }
#elif NETSTANDARD
            get;
            set;
#endif
        }

#if NET451
        [ConfigurationProperty("maxResults", IsRequired = false, IsKey = false)]
#endif
        public int? MaxResults
        {
#if NET451
            get { return base["maxResults"] as int?; }
            set { base["maxResults"] = value; }
#elif NETSTANDARD
            get;
            set;
#endif
        }

        /// <summary>
        /// Gets or sets the type of the caching strategy factory.
        /// </summary>
#if NET451
        [ConfigurationProperty("factory", IsRequired = true), TypeConverter(typeof(TypeNameConverter))]
#elif NETSTANDARD
        private Type _factory;
#endif
        public Type Factory
        {
#if NET451
            get { return (Type)base["factory"]; }
#elif NETSTANDARD
            get { return _factory; }
#endif
            set
            {
                ConfigurationHelper.CheckForInterface(value, typeof(IConfigCachingStrategyFactory));
#if NET451
                base["factory"] = value;
#elif NETSTANDARD
                _factory = value;
#endif
            }
        }

        public ICachingStrategy<T, TKey> GetInstance<T, TKey>() where T : class, new()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigCachingStrategyFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T, TKey>();
        }

        public ICompoundKeyCachingStrategy<T, TKey, TKey2> GetInstance<T, TKey, TKey2>() where T : class, new()
        {
            // load up the factory if it exists and use it
            var factory = (IConfigCachingStrategyFactory)Activator.CreateInstance(Factory, this);

            return factory.GetInstance<T, TKey, TKey2>();
        }


#if NET451
        public new string this[string key]
#elif NETSTANDARD
        public string this[string key]
#endif
        {
            get
            {
                return !_attributes.ContainsKey(key) ? null : _attributes[key];
            }
#if NETSTANDARD
            private set
            {
                _attributes[key] = value;
            }
#endif
        }

#if NET451
        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            var property = new ConfigurationProperty(name, typeof(string), value);
            base[property] = value;

            _attributes[name] = value;

            return true;
        }
#endif

        string ICachingStrategyConfiguration.Name
        {
            get { return this.Name;  }
            set { this.Name = value; }
        }

        int? ICachingStrategyConfiguration.MaxResults
        {
            get { return this.MaxResults; }
            set { this.MaxResults = value; }
        }

        Type ICachingStrategyConfiguration.Factory
        {
            get { return this.Factory; }
            set { this.Factory = value; }
        }

        IDictionary<string, string> ICachingStrategyConfiguration.Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }


        ICachingStrategy<T, TKey> ICachingStrategyConfiguration.GetInstance<T, TKey>()
        {
            return this.GetInstance<T, TKey>();
        }
    }
}