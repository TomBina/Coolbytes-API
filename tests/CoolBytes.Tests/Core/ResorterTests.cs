using System.Collections.Generic;
using CoolBytes.Core.Domain;
using Xunit;

namespace CoolBytes.Tests.Core
{
    public class ReSorterTests
    {
        [Fact]
        public void Should_ReSort_Collection()
        {
            var sortables = new List<ISortable>();
            var category = new Category("Test", 1);
            var anotherCategory = new Category("Test 2", 2);
            sortables.AddRange(new[] { category, anotherCategory });
            var newSortOrder = new[] { 2, 1 };
            var reSorter = new Resorter();

            reSorter.Sort(sortables, newSortOrder);

            Assert.Equal(2, sortables[0].SortOrder);
            Assert.Equal(1, sortables[1].SortOrder);
        }
    }
}
