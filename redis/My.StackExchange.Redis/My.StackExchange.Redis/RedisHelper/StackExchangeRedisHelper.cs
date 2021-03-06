using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace My.StackExchange.Redis
{
    /// <summary>
    /// StackExchangeRedisHelper
    /// 
    /// 在StackExchange.Redis中最重要的对象是ConnectionMultiplexer类， 它存在于StackExchange.Redis命名空间中。
    /// 这个类隐藏了Redis服务的操作细节，ConnectionMultiplexer类做了很多东西， 在所有调用之间它被设计为共享和重用的。
    /// 不应该为每一个操作都创建一个ConnectionMultiplexer 。 ConnectionMultiplexer是线程安全的 ， 推荐使用下面的方法。
    /// 在所有后续示例中 ， 都假定你已经实例化好了一个ConnectionMultiplexer类，它将会一直被重用 ，
    /// 现在我们来创建一个ConnectionMultiplexer实例。它是通过ConnectionMultiplexer.Connect 或者 ConnectionMultiplexer.ConnectAsync，
    /// 传递一个连接字符串或者一个ConfigurationOptions 对象来创建的。
    /// 连接字符串可以是以逗号分割的多个服务的节点.
    /// 
    /// 
    /// 注意 : 
    /// ConnectionMultiplexer 实现了IDisposable接口当我们不再需要是可以将其释放的 , 这里我故意不使用 using 来释放他。 
    /// 简单来讲创建一个ConnectionMultiplexer是十分昂贵的 ， 一个好的主意是我们一直重用一个ConnectionMultiplexer对象。
    /// 一个复杂的的场景中可能包含有主从复制 ， 对于这种情况，只需要指定所有地址在连接字符串中（它将会自动识别出主服务器）
    ///  ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("server1:6379,server2:6379");
    /// 假设这里找到了两台主服务器，将会对两台服务进行裁决选出一台作为主服务器来解决这个问题 ， 这种情况是非常罕见的 ，我们也应该避免这种情况的发生。
    /// 
    /// 
    /// 这里有个和 ServiceStack.Redis 大的区别是没有默认的连接池管理了。没有连接池自然有其利弊,最大的好处在于等待获取连接的等待时间没有了,
    /// 也不会因为连接池里面的连接由于没有正确释放等原因导致无限等待而处于死锁状态。缺点在于一些低质量的代码可能导致服务器资源耗尽。不过提供连接池等阻塞和等待的手段是和作者的设计理念相违背的。StackExchange.Redis这里使用管道和多路复用的技术来实现减少连接
    /// 
    /// 参考：http://www.cnblogs.com/Leo_wl/p/4968537.html
    /// 
    /// 修改记录
    /// 
    ///        2016.04.07 版本：1.0 SongBiao    主键创建。
    ///        
    /// <author>
    ///        <name>SongBiao</name>
    ///        <date>2016.04.07</date>
    /// </author>
    /// </summary>
    public class StackExchangeRedisHelper
    {
        private static object _locker = new Object();
        private static StackExchangeRedisHelper _instance = null;
        public static StackExchangeRedisHelper Instance()
        {
            if (_instance == null)
            {
                lock (_locker)
                {
                    if (_instance == null || _instance._conn.IsConnected == false)
                    {
                        _instance = new StackExchangeRedisHelper();
                    }
                }
            }
            return _instance;
        }

        private ConnectionMultiplexer _conn;

        private string connStr = string.Format("{0}:{1},allowAdmin=true,password={2},defaultdatabase={3}",
      ConfigManagerConf.GetValue("redis:HostName"),
      ConfigManagerConf.GetValue("redis:Port"),
      ConfigManagerConf.GetValue("redis:Password"),
      ConfigManagerConf.GetValue("redis:Defaultdatabase")
      );
        /// <summary>
        /// 使用一个静态属性来返回已连接的实例，如下列中所示。这样，一旦 ConnectionMultiplexer 断开连接，便可以初始化新的连接实例。
        /// </summary>
        public StackExchangeRedisHelper()
        {
            //注册如下事件
            //_instance.ConnectionFailed += MuxerConnectionFailed;
            //_instance.ConnectionRestored += MuxerConnectionRestored;
            //_instance.ErrorMessage += MuxerErrorMessage;
            //_instance.ConfigurationChanged += MuxerConfigurationChanged;
            //_instance.HashSlotMoved += MuxerHashSlotMoved;
            //_instance.InternalError += MuxerInternalError;
            Console.WriteLine(connStr);
            _conn = ConnectionMultiplexer.Connect(connStr);
        }


        public IDatabase GetDatabase()
        {
            try
            {
                return _conn.GetDatabase();
            }
            catch
            {
                _conn = ConnectionMultiplexer.Connect(connStr);
                return _conn.GetDatabase();
            }
        }

        public bool SetExpire(string key, int seconds)
        {
            return GetDatabase().KeyExpire(key, DateTime.Now.AddSeconds(seconds));
        }

        private string MergeKey(string key)
        {
            return key;
        }

        public  void Push(string stackName, RedisValue value)
        {
          var  redisKey = MergeKey(stackName);
            GetDatabase().ListRightPush(redisKey, value);
        }

        public  RedisValue Pop(string stackName)
        {
            var redisKey = MergeKey(stackName);
            return GetDatabase().ListRightPop(redisKey);
        }



        public T Get<T>(string key) where T : class
        {
            key = MergeKey(key);
            var result= GetDatabase().StringGet(key);
          
          return  JsonHelper.FromJson<T>(result);
        }

        public string Get(string key)
        {
            key = MergeKey(key);
          
            return GetDatabase().StringGet(key);
        }

        public bool Set(string key, string value)
        {
            key = MergeKey(key);
            return GetDatabase().StringSet(key, value);
        }
        public bool Set<T>(string key, T value)
        {
            key = MergeKey(key);
            return GetDatabase().SetAdd(key,value.ToJson());
        }

        public string Get_Hash(string redisKey, string hashField)
        {
            redisKey = MergeKey(redisKey);
            string hashvalue = GetDatabase().HashGet(redisKey, hashField);
            return hashvalue;
        }

        public T Get_Hash<T>(string redisKey, string hashField) where T:class
        {
            redisKey = MergeKey(redisKey);
            var hashvalue = GetDatabase().HashGet(redisKey, hashField);
            return JsonHelper.FromJson<T>(hashvalue);
        }
     
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public bool Set_Hash(string redisKey, string hashField, string value)
        {
            redisKey = MergeKey(redisKey);
            //return GetDatabase().HashSet(redisKey, hashField, Serialize(value));
            return GetDatabase().HashSet(redisKey, hashField, value);
        }



        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public bool Set_Hash<T>(string redisKey, string hashField, T value)
        {
            redisKey = MergeKey(redisKey);
            var result = RedisValue.EmptyString;
            return GetDatabase().HashSet(redisKey, hashField, value.ToJson());
        }
        

        /// <summary>
        /// 判断在缓存中是否存在该key的缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            key = MergeKey(key);
            return GetDatabase().KeyExists(key);  //可直接调用
        }

        /// <summary>
        /// 移除指定key的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            key = MergeKey(key);
            return GetDatabase().KeyDelete(key);
        }

        /// <summary>
        /// 异步设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task SetAsync(string key, string value)
        {
            key = MergeKey(key);
            await GetDatabase().StringSetAsync(key, value);
        }

        /// <summary>
        /// 根据key获取缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<object> GetAsync(string key)
        {
            key = MergeKey(key);
            object value = await GetDatabase().StringGetAsync(key);
            return value;
        }

        /// <summary>
        /// 实现递增
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long Increment(string key)
        {
            key = MergeKey(key);
            //三种命令模式
            //Sync,同步模式会直接阻塞调用者，但是显然不会阻塞其他线程。
            //Async,异步模式直接走的是Task模型。
            //Fire - and - Forget,就是发送命令，然后完全不关心最终什么时候完成命令操作。
            //即发即弃：通过配置 CommandFlags 来实现即发即弃功能，在该实例中该方法会立即返回，如果是string则返回null 如果是int则返回0.这个操作将会继续在后台运行，一个典型的用法页面计数器的实现：
            return GetDatabase().StringIncrement(key, flags: CommandFlags.FireAndForget);
        }

        /// <summary>
        /// 实现递减
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long Decrement(string key, string value)
        {
            key = MergeKey(key);
            return GetDatabase().HashDecrement(key, value, flags: CommandFlags.FireAndForget);
        }




        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
            //LogHelper.WriteInfoLog("Configuration changed: " + e.EndPoint);
        }
        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
            //LogHelper.WriteInfoLog("ErrorMessage: " + e.Message);
        }
        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            //LogHelper.WriteInfoLog("ConnectionRestored: " + e.EndPoint);
        }
        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            //LogHelper.WriteInfoLog("重新连接：Endpoint failed: " + e.EndPoint + ", " + e.FailureType + (e.Exception == null ? "" : (", " + e.Exception.Message)));
        }
        /// <summary>
        /// 更改集群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            //LogHelper.WriteInfoLog("HashSlotMoved:NewEndPoint" + e.NewEndPoint + ", OldEndPoint" + e.OldEndPoint);
        }
        /// <summary>
        /// redis类库错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
            //LogHelper.WriteInfoLog("InternalError:Message" + e.Exception.Message);
        }
    }
}
