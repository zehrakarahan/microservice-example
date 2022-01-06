using System;
using System.Collections.Generic;

namespace My.StackExchange.Redis
{
    public interface ICacheManager
    {
        #region Item
        T Get<T>(string key) where T : class;
        void Add<T>(string key, T value);
        void AddWithExpire<T>(string key, T value, TimeSpan cacheTime);
        void Remove(string key);
        #endregion
        #region HashList
        Dictionary<string, T> GetHashList<T>(string key) where T : class;
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="hasKey"></param>
        /// <param name="isManyKeyCache">缓存对象是否为多键对应的缓存</param>
        /// <returns></returns>
        T GetFromHashList<T>(string key, string hasKey, bool isManyKeyCache = false) where T : class;
        void RemoveFromHashList(string key, string hasKey, bool isManyKeyCache = false);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashKey"></param>
        /// <param name="value"></param>
        void AddToHashList<T>(string key, string hashKey, T value) where T : class;
        #endregion
        #region List
        List<T> GetSet<T>(string key) where T : class;
        void SetAddRange<T>(string key, List<T> values) where T : class;
        void SetAdd<T>(string key, T value) where T : class;
        void AddToSetWithExpire<T>(string key, List<T> values, TimeSpan cacheTime) where T : class;


        /// <summary>
        /// 从队列中读取一条记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string BlockingDequeue(string key);
        /// <summary>
        /// 插入队列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void EnqueueItem(string key, string value);
        /// <summary>
        /// 获取集合总数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        long GetListCount(string key);
        #endregion
        void SetExpire(string key, TimeSpan cacheTime);


        /// <summary>
        /// 从list中删除指定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void RemoveList<T>(string key, T value) where T : class;
        
    }
}