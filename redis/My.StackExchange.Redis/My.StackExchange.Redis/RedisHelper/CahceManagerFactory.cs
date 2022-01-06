using System;

namespace My.StackExchange.Redis
{
    public class CacheManagerFactory
    {
        public static ICacheManager GetCacheManager()
        {
            return RedisCacheManager.Instance;
            // return new MemoryCacheManager();
        }

        /// <summary>
        /// 查询或添加到Hash缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="hashKey"></param>
        /// <param name="getModel"></param>
        /// <returns></returns>
        public static T GetOrAddToHash<T>(string cacheKey, string hashKey, Func<T> getModel) where T : class
        {
            var cacheManager = GetCacheManager();
            T model = cacheManager.GetFromHashList<T>(cacheKey, hashKey);
            if (model != null)
            {
                return model;
            }
            model = getModel();
            cacheManager.AddToHashList(cacheKey, hashKey, model);
            return model;
        }
      

    }
}
