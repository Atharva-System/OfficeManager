namespace OfficeManager.Application.Common.Interfaces
{
    public interface IEasyCacheService
    {
        Task<object> GetAsync(string key, Type cachedData);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task RemoveAsync(string key);
        Task RemoveByPrefixAsync(string prefix);
    }
}
