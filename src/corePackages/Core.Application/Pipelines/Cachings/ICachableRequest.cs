namespace Core.Application.Pipelines.Cachings;
public interface ICachableRequest
{
    string CacheKey { get; }
    bool BypassCache { get; }
    TimeSpan? SlidingExpiration { get; }
}