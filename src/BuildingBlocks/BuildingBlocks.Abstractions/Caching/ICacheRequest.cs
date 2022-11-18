using MediatR;

namespace BuildingBlocks.Abstractions.Caching;

// Ref: https://anderly.com/2019/12/12/cross-cutting-concerns-with-mediatr-pipeline-behaviors/
// Works like FluentValidation with defining nested or separate class for our command or query
public interface ICacheRequest<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    TimeSpan AbsoluteExpirationRelativeToNow { get; }

    // TimeSpan SlidingExpiration { get; }
    // DateTime? AbsoluteExpiration { get; }
    string Prefix { get; }
    string CacheKey(TRequest request);
}

public abstract class CacheRequest<TRequest, TResponse> : ICacheRequest<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public virtual TimeSpan AbsoluteExpirationRelativeToNow => TimeSpan.FromMinutes(5);

    // public virtual TimeSpan SlidingExpiration => TimeSpan.FromSeconds(30);
    // public virtual DateTime? AbsoluteExpiration => null;
    public virtual string Prefix => "Ch_";
    public virtual string CacheKey(TRequest request) => $"{Prefix}{typeof(TRequest).Name}";
}

public interface IStreamCacheRequest<in TRequest, TResponse>
    where TRequest : IStreamRequest<TResponse>
{
    TimeSpan AbsoluteExpirationRelativeToNow { get; }

    // TimeSpan SlidingExpiration { get; }
    // DateTime? AbsoluteExpiration { get; }
    string Prefix { get; }
    string CacheKey(TRequest request);
}

public class StreamCacheRequest<TRequest, TResponse> : IStreamCacheRequest<TRequest, TResponse>
    where TRequest : IStreamRequest<TResponse>
{
    public virtual TimeSpan AbsoluteExpirationRelativeToNow => TimeSpan.FromMinutes(5);

    // public virtual TimeSpan SlidingExpiration => TimeSpan.FromSeconds(30);
    // public virtual DateTime? AbsoluteExpiration => null;
    public virtual string Prefix => "Ch_";
    public virtual string CacheKey(TRequest request) => $"{Prefix}{typeof(TRequest).Name}";
}
