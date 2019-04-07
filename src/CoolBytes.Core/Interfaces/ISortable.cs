namespace CoolBytes.Core.Interfaces
{
    public interface ISortable
    {
        int SortOrder { get; }
        void SetSortOrder(int sortOrder);
    }
}