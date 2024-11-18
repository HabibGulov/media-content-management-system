public record BaseFilter
{
    public int PageNumber{get; init;}
    public int PageSize{get; init;}

    public BaseFilter()
    {
        PageNumber=1;
        PageSize=5;
    }

    public BaseFilter(int pageNumber, int pageSize)
    {
        this.PageSize = pageSize <= 0 ? 10 : pageSize;
        this.PageNumber = pageNumber <= 0 ? 1 : pageSize;
    }
}