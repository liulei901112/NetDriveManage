using System.Collections.Generic;

namespace TextLocator.Util
{
    /// <summary>
    /// 简单缓存工具类
    /// </summary>
    public class CacheUtil
    {
        /// <summary>
        /// 缓存
        /// </summary>
        private static volatile Dictionary<string, object> _cache = new Dictionary<string, object>();
        /// <summary>
        /// 缓存同步锁
        /// </summary>
        private static volatile object _lock = new object();

        /// <summary>
        /// 添加缓存
        /// </summary>
        public static void Put(string key, object value)
        {
            if (_cache.TryGetValue(key, out object val))
            {
                lock (_cache)
                {
                    _cache[key] = value;
                }
            }
            else
            {
                lock (_cache)
                {
                    _cache.Add(key, value);
                }
                
            }
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            if (_cache.TryGetValue(key, out object value))
            {
                lock (_cache)
                {
                    _cache.Remove(key);
                }
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public static T Get<T>(string key)
        {
            if (!_cache.TryGetValue(key, out object value))
            {
                return default(T);
            }
            try
            {
                return (T)value;
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 判断缓存是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            if (_cache.TryGetValue(key, out object value))
            {
                return value != null;
            }
            return false;
        }
    }
}
