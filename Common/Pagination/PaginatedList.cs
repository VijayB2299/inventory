using Microsoft.EntityFrameworkCore;

namespace Inventory.Common.Pagination;

public sealed class PaginatedList<T>
{
    public IEnumerable<T> Items { get; }
    public int Page { get; }
    public int CurrentPage => Page;
    public int PageSize { get; }
    public int TotalCount { get; }
    public int NextPage { get; }
    public int PrevPage { get; }

    public int TotalPages { get; }
    public PaginatedList(IEnumerable<T> items, int page, int pageSize, int totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
        NextPage = (page * pageSize) >= totalCount ? 0 : page + 1;
        PrevPage = page <= 1 ? 0 : page - 1;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    public static async Task<PaginatedList<T>> FetchAsync(IQueryable<T> query, int page, int pageSize)
    {
      var totalCount = await query.CountAsync();
      var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
      return new PaginatedList<T>(items, page, pageSize, totalCount);
    }
}