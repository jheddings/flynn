// =============================================================================
//  Copyright © Jason Heddings, All Rights Reserved
//  $Id: ObjectCache.cs 81 2013-11-06 04:44:58Z jheddings $
// =============================================================================
using System;
using System.Collections.Generic;

namespace Flynn.Utilities {
    public sealed class ObjectCache<K, V> {

        private static readonly Logger _logger = Logger.Get("ObjectCache");

        private readonly Dictionary<K, CachedObjectInfo> _cache =
            new Dictionary<K, CachedObjectInfo>();

        ///////////////////////////////////////////////////////////////////////
        private int _capacity = int.MaxValue;
        public int Capacity {
            get { return _capacity; }

            set {
                if (value < 0) {
                    throw new ArgumentException("value cannot be negative");
                }

                _capacity = value;

                EnforceCapacity();
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private TimeSpan _expire = TimeSpan.MaxValue;
        public TimeSpan Expiration {
            get { return _expire; }

            set {
                if (value < TimeSpan.Zero) {
                    throw new ArgumentException("value cannot be negative");
                }
                _expire = value;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        public V this [K key] {
            get { return Get(key); }

            set {
                if (value == null) {
                    Remove(key);
                } else {
                    Save(key, value);
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////
        public ObjectCache(TimeSpan ttl) {
            this.Expiration = ttl;
        }
        
        ///////////////////////////////////////////////////////////////////////
        public ObjectCache(int count) {
            this.Capacity = count;
        }
        
        ///////////////////////////////////////////////////////////////////////
        public ObjectCache(int count, TimeSpan ttl) {
            this.Capacity = count;
            this.Expiration = ttl;
        }

        ///////////////////////////////////////////////////////////////////////
        private V Get(K key) {
            V data = default(V);

            lock (_cache) {
                data = UnsafeGet(key);
            }
            
            return data;
        }

        ///////////////////////////////////////////////////////////////////////
        private void Save(K key, V value) {
            lock (_cache) {
                UnsafeEnforceCap(1);
                UnsafeSave(key, value);
            }

            _logger.Debug("new data saved for {0}", key);
        }

        ///////////////////////////////////////////////////////////////////////
        private void Remove(K key) {
            lock (_cache) {
                UnsafeRemove(key);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private void EnforceCapacity(int reserve = 0) {
            lock (_cache) {
                UnsafeEnforceCap(reserve);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private V UnsafeGet(K key) {
            V data = default(V);

            if (_cache.ContainsKey(key)) {
                CachedObjectInfo info = _cache[key];
                TimeSpan age = DateTime.Now - info.LastUpdated;

                if (age < _expire) {
                    _logger.Debug("Cache hit: {0}", key);
                    data = info.UserData;
                } else {
                    _logger.Debug("Cache expired: {0}", key);
                    UnsafeRemove(key);
                }

            } else {
                _logger.Debug("Cache miss: {0}", key);
            }

            return data;
        }

        ///////////////////////////////////////////////////////////////////////
        private void UnsafeSave(K key, V value) {
            CachedObjectInfo info = new CachedObjectInfo {
                UserKey = key,
                UserData = value,
                LastUpdated = DateTime.Now,
            };

            _cache[key] = info;
        }

        ///////////////////////////////////////////////////////////////////////
        private void UnsafeRemove(K key) {
            bool removed = _cache.Remove(key);

            _logger.Debug("removed item [{1}]: {0}", key, removed);

            // XXX should we call Dispose on the removed object?
        }
        
        ///////////////////////////////////////////////////////////////////////
        private void UnsafeEnforceCap(int reserve = 0) {
            if (_cache.Count == 0) { return; }

            // FIXME may be an issue here if (_capacity - reserve) < 0

            while (_cache.Count > (_capacity - reserve)) {

                // XXX is there a LINQ expression to find the smallest value in a list?

                // start with a dummy object for comparison
                CachedObjectInfo oldest = new CachedObjectInfo {
                    LastUpdated = DateTime.MaxValue
                };

                foreach (CachedObjectInfo info in _cache.Values) {
                    if (info.LastUpdated < oldest.LastUpdated) {
                        oldest = info;
                    }
                }

                _cache.Remove(oldest.UserKey);
            }
        }

        ///////////////////////////////////////////////////////////////////////
        private struct CachedObjectInfo {
            public K UserKey;
            public V UserData;
            public DateTime LastUpdated;
        }
    }
}
