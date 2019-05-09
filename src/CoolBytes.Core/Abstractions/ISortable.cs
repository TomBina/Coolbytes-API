namespace CoolBytes.Core.Abstractions
{
    public interface ISortable : IEntity
    {
        int SortOrder { get; }
        void SetSortOrder(int sortOrder);
    }
}