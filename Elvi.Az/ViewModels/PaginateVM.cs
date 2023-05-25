namespace Elvi.Az.ViewModels
{
    public class PaginateVM<T>
    {
        public List<T> Items { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
    }
}
