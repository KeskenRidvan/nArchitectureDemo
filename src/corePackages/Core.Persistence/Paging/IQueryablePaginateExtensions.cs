using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.Paging;

public static class IQueryablePaginateExtensions
{
    public static async Task<Paginate<TEntity>> ToPaginateAsync<TEntity>(
        this IQueryable<TEntity> source,
        int index,
        int size,
        CancellationToken cancellationToken = default)
    {
        int count = await source
            .CountAsync(cancellationToken)
            .ConfigureAwait(false);

        List<TEntity> items = await source
            .Skip(index * size)
            .Take(size)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        Paginate<TEntity> list = new()
        {
            Index = index,
            Count = count,
            Items = items,
            Size = size,
            Pages = (int)Math.Ceiling(count / (double)size)
        };
        return list;
    }

    public static Paginate<TEntity> ToPaginate<TEntity>(
        this IQueryable<TEntity> source,
        int index,
        int size)
    {
        int count = source.Count();
        var items = source
            .Skip(index * size)
            .Take(size)
            .ToList();

        Paginate<TEntity> list = new()
        {
            Index = index,
            Size = size,
            Count = count,
            Items = items,
            Pages = (int)Math.Ceiling(count / (double)size)
        };
        return list;
    }
}