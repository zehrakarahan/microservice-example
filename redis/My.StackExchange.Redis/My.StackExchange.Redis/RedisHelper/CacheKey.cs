namespace My.StackExchange.Redis
{
    /// <summary>
    /// 缓存键值名,请按规范命名
    /// </summary>
    public class CacheKey
    {
        private const string CachePrefix = "Bailun_Pro_Cache_";
        /// <summary>
        /// 多键缓存值
        /// </summary>
        public const string ManyKeyValue = "many_key_value";
    }
}
