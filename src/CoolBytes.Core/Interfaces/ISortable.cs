namespace CoolBytes.Tests.Core
{
    public interface ISortable
    {
        int SortOrder { get; }
        void SetSortOrder(int sortOrder);
    }
}