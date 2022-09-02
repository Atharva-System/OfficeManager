namespace OfficeManager.Infrastructure.Settings
{
    public class CacheSettings
    {
        public bool PreferRedis { get; set; }
        public string RedisURL { get; set; } = string.Empty;
        public int RedisPort { get; set; }
    }
}
