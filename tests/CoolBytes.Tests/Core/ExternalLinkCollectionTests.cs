using System.Linq;
using CoolBytes.Core.Collections;
using CoolBytes.Core.Domain;
using Xunit;

namespace CoolBytes.Tests.Core
{
    public class ExternalLinkCollectionTests
    {
        private readonly ExternalLinkCollection _collection;

        public ExternalLinkCollectionTests()
        {
            _collection = new ExternalLinkCollection();
        }

        [Fact]
        public void ShouldContainOneItem()
        {
            var item = new ExternalLink("Test", "http://test.com");

            _collection.Add(item);

            Assert.Equal(1, _collection.Count);
        }

        [Fact]
        public void ShouldNotInsertDuplicateItem()
        {
            var item = new ExternalLink("TEST", "http://test.com");
            var itemDuplicate = new ExternalLink("test", "http://www.test.com");

            _collection.Add(item);
            _collection.Add(itemDuplicate);

            Assert.Equal(1, _collection.Count);
        }

        [Fact]
        public void ShouldUpdateCorrectly()
        {
            _collection.Add(new ExternalLink("Test1", "http://test.com"));
            _collection.Add(new ExternalLink("Test2", "http://test.com"));
            var newItem = new[] { new ExternalLink("Test1", "http://www.test.com") };

            _collection.Update(newItem);

            Assert.Equal(1, _collection.Count);
            Assert.Equal("Test1", _collection.First().Name);
        }

        [Fact]
        public void ShouldPreserveIdsCorrectly()
        {
            var currentItem = new ExternalLink("Test", "http://test.com");
            SetId(currentItem, 1);
            _collection.Add(currentItem);

            var newItem = new[] { new ExternalLink("Test", "http://www.test.com") };
            SetId(newItem.First(), 2);

            _collection.Update(newItem);

            Assert.Equal(1, _collection.First().Id);
        }

        private static void SetId(ExternalLink item, int id) =>
            item.GetType().GetProperty("Id").SetValue(item, id);
    }
}
