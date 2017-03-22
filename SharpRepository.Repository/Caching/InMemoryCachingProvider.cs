using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;

namespace SharpRepository.Repository.Caching
{
    /// <summary>
    /// Uses the .NET built-in MemoryCache as the caching provider.
    /// </summary>
    public class InMemoryCachingProvider : ICachingProvider
    {
        private static IMemoryCache _cache;
        private static readonly object _singletonLock = new object();
        private static IMemoryCache Cache
        {
            get
            {
                if (_cache == null)
                {
                    lock(_singletonLock)
                    {
                        if (_cache == null)
                        {
                            _cache = new MemoryCache(new MemoryCacheOptions());
                        }
                    }
                }

                return _cache;
            }
        }

        private static readonly object LockObject = new object();

        public void Set<T>(string key, T value, CacheItemPriority priority = CacheItemPriority.Normal, int? cacheTime = null)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            var policy = new MemoryCacheEntryOptions()
                             {
                                 Priority = priority
                             };
            if (cacheTime.HasValue)
            {
                policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(cacheTime.Value);
            }

            Cache.Set(key, value, policy);
        }

        public void Clear(string key)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            Cache.Remove(key);
        }

        public bool Exists(string key)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");
            object value;
            return Cache.TryGetValue(key, out value);
        }

        public bool Get<T>(string key, out T value)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            value = default(T);

            try
            {
                if (!Exists(key))
                    return false;

                value = Cache.Get<T>(key);
            }
            catch (Exception)
            {
                // ignore and use default
                return false;
            }

            return true;
        }

        public int Increment(string key, int defaultValue, int incrementValue, CacheItemPriority priority = CacheItemPriority.Normal)
        {
            if (String.IsNullOrEmpty(key)) throw new ArgumentNullException("key");

            lock (LockObject)
            {
                int current;
                if (!Get(key, out current))
                {
                    current = defaultValue;
                }

                var newValue = current + incrementValue;
                Set(key, newValue, priority);
                return newValue;
            }
        }

        public void Dispose()
        {
            // TODO: Investigate if this is desired behavior.
            // Users will have to dispose the caching provider to clear the cache,
            // like done in QueryManagerTests.Setup(). It's probably a breaking change?
            if (_cache != null)
            {
                _cache.Dispose();
                _cache = null;
            }
        }
    }
}
