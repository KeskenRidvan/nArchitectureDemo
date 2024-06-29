using Core.Persistence.Paging;

namespace Core.Application.Responses;
public class GetListResponse<TEntity> : BasePageableModel
{
    private IList<TEntity> _items;

    public IList<TEntity> Items
    {
        get => _items ??= new List<TEntity>();
        set => _items = value;
    }
}
