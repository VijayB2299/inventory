public class PaginatedResponseDto<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int NextPage { get; set; }
    public int PrevPage { get; set; }
}