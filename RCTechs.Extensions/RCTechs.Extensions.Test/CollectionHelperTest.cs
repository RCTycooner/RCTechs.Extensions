using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCTechs.Extensions.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RCTechs.Extensions.Test
{
    [TestClass]
    public class CollectionHelperTest
    {
        [TestMethod]
        public void FindLocationToInsertTest()
        {
            List<int> destination = new List<int>()
            {
                10, 20, 30, 40
            };

            var indexer = new Func<int, int, int>((d, s) => { return d.CompareTo(s); });

            Assert.AreEqual(0, CollectionHelper.FindLocationToInsertElement(destination, 5, indexer));
            Assert.AreEqual(1, CollectionHelper.FindLocationToInsertElement(destination, 11, indexer));
            Assert.AreEqual(1, CollectionHelper.FindLocationToInsertElement(destination, 20, indexer));
            Assert.AreEqual(2, CollectionHelper.FindLocationToInsertElement(destination, 25, indexer));
        }

        [TestMethod]
        public void MergeCollectionsTests_001_EmptyDestination()
        {
            List<int> source = new List<int>()
            {
                10, 20, 30, 40
            };
            ObservableCollection<int> destination = new ObservableCollection<int>();

            MergeCollections(destination, source);

            Assert.AreEqual(source.Count, destination.Count);
            Assert.AreEqual(source[0], destination[0]);
            Assert.AreEqual(source[1], destination[1]);
            Assert.AreEqual(source[2], destination[2]);
            Assert.AreEqual(source[3], destination[3]);
        }

        [TestMethod]
        public void MergeCollectionsTests_002()
        {
            List<int> source = new List<int>()
            {
                10, 20, 30, 40
            };
            ObservableCollection<int> destination = new ObservableCollection<int>()
            {
                5, 10, 15, 20, 50
            };

            MergeCollections(destination, source);

            Assert.AreEqual(source.Count, destination.Count);
            Assert.AreEqual(source[0], destination[0]);
            Assert.AreEqual(source[1], destination[1]);
            Assert.AreEqual(source[2], destination[2]);
            Assert.AreEqual(source[3], destination[3]);
        }

        [TestMethod]
        public void MergeCollectionsTests_003_IdenticalCollections()
        {
            List<Tuple<int, string>> source = new List<Tuple<int, string>>()
            {
                new Tuple<int, string>(10, "010"),
                new Tuple<int, string>(20, "020"),
                new Tuple<int, string>(30, "030"),
                new Tuple<int, string>(40, "040"),
            };
            ObservableCollection<Tuple<int, string>> destination = new ObservableCollection<Tuple<int, string>>()
            {
                new Tuple<int, string>(10, "010"),
                new Tuple<int, string>(20, "020"),
                new Tuple<int, string>(30, "030"),
                new Tuple<int, string>(40, "040"),
            };

            MergeCollections(destination, source);

            Assert.AreEqual(source.Count, destination.Count);
            Assert.AreEqual(source[0], destination[0]);
            Assert.AreEqual(source[1], destination[1]);
            Assert.AreEqual(source[2], destination[2]);
            Assert.AreEqual(source[3], destination[3]);
        }

        [TestMethod]
        public void MergeCollectionsTests_004_DifferentCollections()
        {
            List<Tuple<int, string>> source = new List<Tuple<int, string>>()
            {
                new Tuple<int, string>(10, "010"),
                new Tuple<int, string>(20, "020"),
                new Tuple<int, string>(30, "030"),
                new Tuple<int, string>(40, "040"),
            };
            ObservableCollection<Tuple<int, string>> destination = new ObservableCollection<Tuple<int, string>>()
            {
                new Tuple<int, string>(5, "005"),
                new Tuple<int, string>(15, "015"),
                new Tuple<int, string>(30, "030"),
                new Tuple<int, string>(45, "045"),
            };

            MergeCollections(destination, source);

            Assert.AreEqual(source.Count, destination.Count);
            Assert.AreEqual(source[0], destination[0]);
            Assert.AreEqual(source[1], destination[1]);
            Assert.AreEqual(source[2], destination[2]);
            Assert.AreEqual(source[3], destination[3]);
        }

        private void MergeCollections(Collection<int> destination, List<int> source)
        {
            CollectionHelper.MergeCollections<int>(destination, source
                , (d, s) =>
                {
                    return d == s;
                }
                , (d, s) =>
                {
                    return d.CompareTo(s);
                }
                , true);
        }
        private void MergeCollections(Collection<Tuple<int, string>> destination, List<Tuple<int, string>> source)
        {
            CollectionHelper.MergeCollections<Tuple<int, string>>(destination, source
                , (d, s) =>
                {
                    return d.Item1 == s.Item1;
                }
                , (d, s) =>
                {
                    return d.Item2.CompareTo(s.Item2);
                }
                , true);
        }
    }
}
