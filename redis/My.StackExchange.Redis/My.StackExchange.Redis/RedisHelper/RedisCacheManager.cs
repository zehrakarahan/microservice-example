using System;
using System.Collections.Generic;

namespace My.StackExchange.Redis
{
    public class RedisCacheManager : ICacheManager
    {
        private static readonly object Lockobj = new object();
        private static readonly object RedisAsyncLock = new object();
        private RedisCacheManager() { }

        private static RedisCacheManager _instance;
        public static RedisCacheManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (RedisAsyncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new RedisCacheManager();
                        }
                       
                    }
                }
                return _instance;
            }
        }

        #region string


        /// <summary> 
        /// 设置单体 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="t"></param> 
        /// <returns></returns> 
        public void Add<T>(string key, T value)
        {
            RedisBase.ItemSet<T>(key, value);
        }

        /// <summary>
        /// 设置单体 ，带过期时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime"></param>
        public void AddWithExpire<T>(string key, T value, TimeSpan cacheTime)
        {
            Add(key, value);
            SetExpire(key, cacheTime);
        }

        /// <summary>
        /// 获取单体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            return RedisBase.ItemGet<T>(key);
        }

        #endregion

        #region hash



        /// <summary>
        /// 存储数据到hash表 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashKey"></param>
        /// <param name="value"></param>
        public void AddToHashList<T>(string key, string hashKey, T value) where T : class
        {
         
           RedisBase.HashSet(key, hashKey, value);
            
        }

        public void RemoveFromHashList(string key, string hasKey, bool isManyKeyCache = false)
        {
            if (isManyKeyCache)
            {
                var manyKeyValue = RedisBase.HashGet<string>(key, hasKey);
                if (!string.IsNullOrEmpty(manyKeyValue))
                {
                    RedisBase.HashRemove(key, manyKeyValue);
                }
            }
            RedisBase.HashRemove(key, hasKey);
        }

        public T GetFromHashList<T>(string key, string hasKey, bool isManyKeyCache = false) where T : class
        {
            if (isManyKeyCache)
            {
                var manyKeyValue = RedisBase.HashGet<string>(key, hasKey);
                if (!string.IsNullOrEmpty(manyKeyValue) && manyKeyValue.Contains(CacheKey.ManyKeyValue))
                {
                    return RedisBase.HashGet<T>(key, manyKeyValue);
                }
            }
            return RedisBase.HashGet<T>(key, hasKey);
        }


        public Dictionary<string, T> GetHashList<T>(string key) where T : class
        {
            var result = RedisBase.HashGetAll<T>(key);
            return result;
        }
        #endregion

        #region set
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> GetSet<T>(string key) where T : class
        {
            return RedisBase.SetGetAll<T>(key);
        }

        public void SetAdd<T>(string key, T value) where T : class
        {
            RedisBase.SetAdd<T>(key, value);
        }

        /// <summary>
        /// 存储数据到set,不追加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        public void SetSet<T>(string key, List<T> values) where T : class
        {
            lock (Lockobj)
            {
                RedisBase.RemoveKey(key);
                RedisBase.SetAddRange(key, values);
            }
        }

        /// <summary>
        /// 存储数据到set,带过期时间，不追加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        public void SetSetWithExpire<T>(string key, List<T> values, TimeSpan cacheTime) where T : class
        {
            lock (Lockobj)
            {
                RedisBase.RemoveKey(key);
                RedisBase.SetAddRange(key, values);
                SetExpire(key, cacheTime);
            }
           
        }


        /// <summary>
        /// 存储数据到set,追加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        public void SetAddRange<T>(string key, List<T> values) where T : class
        {
            if (values.Count > 0)
            {
                RedisBase.SetAddRange(key, values);
            }
            
        }

        /// <summary>
        /// 存储数据到set,带过期时间，追加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        public void AddToSetWithExpire<T>(string key, List<T> values, TimeSpan cacheTime) where T : class
        {
            if (values.Count > 0)
            {
                RedisBase.SetAddRange(key, values);
                SetExpire(key, cacheTime);
            }
        }
        #endregion


        #region list


        /// <summary>
        /// 阻塞从list移除一个值，并反回移除的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string BlockingDequeue(string key)
        {
            return RedisBase.BlockingDequeue(key);

        }

        /// <summary>
        /// 插入队列，list中插入值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void EnqueueItem(string key, string value)
        {
            RedisBase.EnqueueItem(key, value);

        }

        /// <summary>
        /// 从list中删除指定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void RemoveList<T>(string key, T value) where T : class
        {
            RedisBase.ListRemove(key, value);
        }

        /// <summary>
        /// 获取list长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long GetListCount(string key)
        {

            return RedisBase.GetListCount(key);

        }
        #endregion



       

        public void Remove(string key)
        {
            RedisBase.ItemRemove(key);
        }

      

        public void RemoveAllList<T>(string key) where T : class
        {
            RedisBase.ListRemoveAll<T>(key);
        }
       

        public void SetExpire(string key, TimeSpan cacheTime)
        {
            RedisBase.SetExpire(key, cacheTime);
        }

      

    



    }
}
