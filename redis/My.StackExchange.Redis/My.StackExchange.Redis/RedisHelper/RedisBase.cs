using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;

namespace My.StackExchange.Redis
{
    public class RedisBase
    {
        private static  readonly StackExchangeRedisHelper instance= StackExchangeRedisHelper.Instance();



        /// <summary> 
        /// 移除Key 
        /// </summary> 
        /// <param name="key"></param> 
        public static bool RemoveKey(string key)
        {
            return instance.Remove(key);
        }

        #region string
        /// <summary> 
        /// 设置单体 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="t"></param> 
        /// <returns></returns> 
        public static bool ItemSet<T>(string key, T t)
        {
           
           return instance.GetDatabase().StringSet(key,t.ToJson());
            
        }
      
        /// <summary> 
        /// 获取单体 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static T ItemGet<T>(string key) where T : class
        {
            return instance.Get<T>(key);
          
        }

        /// <summary> 
        /// 移除单体 
        /// </summary> 
        /// <param name="key"></param> 
        public static bool ItemRemove(string key)
        {
           return instance.Remove(key);
        }

        #endregion

        #region set
        public static void SetAdd<T>(string key, T t) where T: class
        {
            if (t == null)
            {
                return;
            }

            instance.Set<T>(key, t);
        }
        /// <summary>
        /// 新增批量数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        public static void SetAddRange<T>(string key, List<T> values) where T : class
        {
            if (values.Count > 0)
            {

                var redisValues = values.Select(p=>(RedisValue)p.ToJson()).ToList();
                instance.GetDatabase().SetAdd(key, redisValues.ToArray());
            }
        }

      
        public static void ListRemoveAll<T>(string key)
        {
             instance.Remove(key);
        }

        public static bool SetContains<T>(string key, T t) where T : class
        {
           return instance.GetDatabase().SetContains(key, t.ToJson());
          
        }

      



        public static List<T> SetGetAll<T>(string key) where T : class
        {
            var result = instance.GetDatabase().SetMembers(key);
          
            return result.Select(p=>JsonHelper.FromJson<T>(p)).ToList();
        }


        public static List<T> ListGetRange<T>(string key, int start, int count) where T : class
        {
            var result = instance.GetDatabase().StringGetRange(key, start, count);
            return JsonHelper.FromJsonList<T>(result);
        }

        public static List<T> ListGetPaged<T>(string key, int pageIndex, int pageSize) where T : class
        {
            int start = pageSize * (pageIndex - 1);
           var result= instance.GetDatabase().ListRange(key, start, pageSize);
            return result.Select(p => JsonHelper.FromJson<T>(p)).ToList();
        }

        #endregion

        #region list
        public static bool ListRemove<T>(string key, T t) where T : class
        {
            var result = instance.GetDatabase().ListRemove(key, RedisValue.Unbox(t));
            return result >= 0;
        }
        /// <summary>
        /// 阻塞从list移除一个值，并反回移除的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string BlockingDequeue(string key)
        {
            return instance.Pop(key);

        }
        /// <summary>
        /// 插入队列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void EnqueueItem(string key, string value)
        {
            instance.Push(key, value);

        }
        public static long GetListCount(string key)
        {
            return instance.GetDatabase().ListLength(key);

        }
        #endregion



        #region Hash
        /// <summary> 
        /// 判断某个数据是否已经被缓存 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static bool HashExist<T>(string key, string dataKey) where T : class
        {
            var result = instance.Get_Hash<T>(key,dataKey);
            return result!=null;
        }

        /// <summary> 
        /// 存储数据到hash表 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static bool HashSet<T>(string key, string dataKey, T t) where T : class
        {
            if (t == null)
            {
                return false;
            }
           
           return instance.Set_Hash(key, dataKey, t.ToJson());
        }

        /// <summary> 
        /// 移除hash中的某值 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static bool HashRemove(string key, string dataKey)
        {
            return instance.GetDatabase().HashDelete(key, dataKey);
        }
        /// <summary> 
        /// 移除整个hash 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static bool HashRemove(string key)
        {
            return instance.Remove(key);
        }

    
        /// <summary> 
        /// 从hash表获取数据 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <param name="dataKey"></param> 
        /// <returns></returns> 
        public static T HashGet<T>(string key, string dataKey) where T : class
        {
            return instance.Get_Hash<T>(key, dataKey);
        }
        /// <summary> 
        /// 获取整个hash的数据 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static Dictionary<string,T> HashGetAll<T>(string key) where T : class
        {
            var result= instance.GetDatabase().HashGetAll(key);
            var dict = new Dictionary<string, T>();
            foreach (var hashEntry in result)
            {
                ;
                dict.Add(hashEntry.Name,JsonHelper.FromJson<T>(hashEntry.Value));
            }
            return dict;
        }
        
    
        #endregion



        /// <summary>
        /// set expire time for spicify key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeSpan"></param>
        public static void SetExpire(string key, TimeSpan timeSpan)
        {
            instance.SetExpire(key, int.Parse(timeSpan.TotalSeconds.ToString()));
        }

      

        


    }
}