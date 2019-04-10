using CoolBytes.Core.Domain;
using System.Collections.Generic;
using Xunit;

namespace CoolBytes.Tests.Core
{
    public class SorterTests
    {
        [Fact]
        public void Should_Sort_Collection_Correctly()
        {
            var sortables = new List<Category>();
            var category = new Category(id: 1, name: "Test", sortOrder: 1);
            var anotherCategory = new Category(id: 2, name: "Test 2", sortOrder: 2);
            sortables.AddRange(new[] { category, anotherCategory });
            var newSortOrder = new[] { 2, 1 };
            var sorter = new Sorter<Category>();

            sorter.Sort(sortables, newSortOrder);

            Assert.Equal(2, sortables[0].SortOrder);
            Assert.Equal(1, sortables[1].SortOrder);
        }
    }
}
