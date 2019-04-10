namespace CoolBytes.Core.Interfaces
{
    public interface ISortable : IEntity
    {
        int SortOrder { get; }
        void SetSortOrder(int sortOrder);
    }
}