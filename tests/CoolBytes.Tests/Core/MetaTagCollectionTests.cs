using System.Linq;
using CoolBytes.Core.Collections;
using CoolBytes.Core.Domain;
using Xunit;

namespace CoolBytes.Tests.Core
{
    public class MetaTagCollectionTests
    {
        private readonly MetaTagCollection _collection;

        public MetaTagCollectionTests()
        {
            _collection = new MetaTagCollection();
        }

        [Fact]
        public void ShouldContainOneItem()
        {
            var item = new MetaTag("twitter:description", "some description");

            _collection.Add(item);

            Assert.NotEmpty(_collection);
        }

        [Fact]
        public void ShouldNotInsertDuplicateItem()
        {
            var item = new MetaTag("twitter:description", "some description");
            var itemDuplicate = new MetaTag("TWITTER:description", "some description");

            _collection.Add(item);
            _collection.Add(itemDuplicate);

            Assert.Single(_collection);
        }

        [Fact]
        public void ShouldUpdateCorrectly()
        {
            var item = new MetaTag("twitter:description", "some description");
            _collection.Add(item);
            var newItem = new[] { new MetaTag("twitter:description", "some description") };

            _collection.Update(newItem);

            Assert.Single(_collection);
            Assert.Equal("twitter:description", _collection.First().Name);
        }

        [Fact]
        public void ShouldPreserveIdsCorrectly()
        {
            var currentItem = new MetaTag("twitter:description", "some description");
            SetId(currentItem, 1);
            _collection.Add(currentItem);

            var newItem = new[] { new MetaTag("twitter:description", "some description") };
            SetId(newItem.First(), 2);

            _collection.Update(newItem);

            Assert.Equal(1, _collection.First().Id);
        }

        private static void SetId(MetaTag item, int id) =>
            item.GetType().GetProperty(nameof(MetaTag.Id))?.SetValue(item, id);
    }
}
