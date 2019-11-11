using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RCTechs.Extensions.Helpers
{
    public static partial class CollectionHelper
    {
        public static void MergeCollections<T>(
            Collection<T> destinationCollection,
            IEnumerable<T> sourceCollection,
            Func<T, T, bool> comparator,
            Func<T, T, int> indexer,
            bool replaceExisting = false)
        {
            MergeCollections<T>(destinationCollection
                , sourceCollection
                , comparator
                , indexer
                , null
                , replaceExisting);
        }
        public static void MergeCollections<T>(
            Collection<T> destinationCollection,
            IEnumerable<T> sourceCollection,
            Func<T, T, bool> comparator,
            Func<T, T, int> indexer,
            Action<T, T> merger,
            bool replaceExisting = false,
            bool runOnUIThread = false)
        {
            if (destinationCollection == null)
                throw new ArgumentNullException("DestinationCollection must not be NULL");

            //Granulary add/remove the newer filter so we don't have a flickering screen.
            foreach (var element in sourceCollection)
            {
                var existingElementInDest = (from e in destinationCollection
                                             where comparator(e, element)
                                             select e).FirstOrDefault();
                if (existingElementInDest == null || (default(T) != null && default(T).Equals(existingElementInDest)))
                {
                    int locToAdd = FindLocationToInsertElement(destinationCollection, element, indexer);

                    //Debug.WriteLine("CollHelper-Merge: Insert " + element.ToString() + " @ " + locToAdd);
                    destinationCollection.Insert(locToAdd, element);
                }
                else if (replaceExisting)
                {
                    int loc = destinationCollection.IndexOf(existingElementInDest);
                    //Debug.WriteLine("CollHelper-Merge: Replace " + element.ToString() + " @ " + loc);
                    destinationCollection.RemoveAt(loc);
                    destinationCollection.Insert(loc, element);
                }
                else if (merger != null)
                {
                    //Only merge if this object isn't the same ref
                    if (!Object.ReferenceEquals(existingElementInDest, element))
                    {
                        //Debug.WriteLine("CollHelper-Merge: Merge " + element.ToString());
                        merger(existingElementInDest, element);
                    }

                    //Resort, just in case the sorting properties have been changed during the merger()
                    int loc = destinationCollection.IndexOf(existingElementInDest);
                    int locToAdd = FindLocationToInsertElement(destinationCollection, element, indexer);
                    if (loc != locToAdd)
                    {
                        if (locToAdd >= loc)
                            locToAdd--;

                        destinationCollection.RemoveAt(loc);
                        destinationCollection.Insert(locToAdd, existingElementInDest);
                    }
                }
            }

            //Remove excess entries:
            var toDelete = (from eleDest in destinationCollection
                                //eleDest does not exists in sourceCollection:
                            where !(from eleSrc in sourceCollection
                                    where comparator(eleDest, eleSrc)
                                    select true).Any()
                            select eleDest).ToList();

            if (runOnUIThread)
            {
                //var dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
                //dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                //{
                foreach (var eleToDelete in toDelete)
                {
                    int idx = destinationCollection.IndexOf(eleToDelete);
                    if (idx >= 0)
                        destinationCollection.RemoveAt(idx);
                }
                //});
            }
            else
            {
                foreach (var eleToDelete in toDelete)
                {
                    destinationCollection.Remove(eleToDelete);
                }
            }
        }

        public static async Task MergeCollectionsAsync<T>(
            Collection<T> destinationCollection,
            IEnumerable<T> sourceCollection,
            Func<T, T, bool> comparator,
            Func<T, T, int> indexer,
            bool replaceExisting = false)
        {
            await MergeCollectionsAsync<T>(destinationCollection
                 , sourceCollection
                 , comparator
                 , indexer
                 , null
                 , replaceExisting);
        }
        public static async Task MergeCollectionsAsync<T>(
            Collection<T> destinationCollection,
            IEnumerable<T> sourceCollection,
            Func<T, T, bool> comparator,
            Func<T, T, int> indexer,
            Action<T, T> merger,
            bool replaceExisting = false)
        {
            if (destinationCollection == null)
                throw new ArgumentNullException("DestinationCollection must not be NULL");

            //var dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;  //ServiceLocator.Current.GetInstance<IDispatcherService>().Dispatcher;
            //await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
            //    () =>
            //    {
            //Granulary add/remove the newer filter so we don't have a flickering screen.
            foreach (var element in sourceCollection)
            {
                var existingElementInDest = (from e in destinationCollection
                                             where comparator(e, element)
                                             select e).FirstOrDefault();
                if (existingElementInDest == null || (default(T) != null && default(T).Equals(existingElementInDest)))
                {
                    int locToAdd = FindLocationToInsertElement(destinationCollection, element, indexer);

                    //Debug.WriteLine("CollHelper-Merge: Insert " + element.ToString() + " @ " + locToAdd);

                    destinationCollection.Insert(locToAdd, element);
                }
                else if (replaceExisting)
                {
                    int loc = destinationCollection.IndexOf(existingElementInDest);
                    //Debug.WriteLine("CollHelper-Merge: Replace " + element.ToString() + " @ " + loc);
                    destinationCollection.RemoveAt(loc);
                    destinationCollection.Insert(loc, element);
                }
                else if (merger != null)
                {
                    //Debug.WriteLine("CollHelper-Merge: Merge " + element.ToString());
                    merger(existingElementInDest, element);

                    //Resort, just in case the sorting properties have been changed during the merger()
                    int loc = destinationCollection.IndexOf(existingElementInDest);
                    int locToAdd = FindLocationToInsertElement(destinationCollection, element, indexer);
                    if (loc != locToAdd)
                    {
                        if (locToAdd >= loc)
                            locToAdd--;

                        destinationCollection.RemoveAt(loc);
                        destinationCollection.Insert(locToAdd, element);
                    }
                }
            }

            //Remove excess entries:
            var toDelete = (from eleDest in destinationCollection
                                //eleDest does not exists in sourceCollection:
                            where !(from eleSrc in sourceCollection
                                    where comparator(eleDest, eleSrc)
                                    select true).Any()
                            select eleDest).Distinct().ToList();
            foreach (var eleToDelete in toDelete)
            {
                destinationCollection.Remove(eleToDelete);
            }
            //});
        }

        public static int FindLocationToInsertElement<T>(
            IList<T> destinationCollection
            , T element
            , Func<T, T, int> indexer)
        {
            int i = 0;
            for (; i < destinationCollection.Count; i++)
            {
                var compareResult = indexer(destinationCollection[i], element);
                if (compareResult >= 0)
                    return i;
            }
            return i; //Add to the back of the collection.
        }

        public static void BubbleSort<T>(this Collection<T> o
            , Func<T, T, int> indexer)
        {
            for (int i = o.Count - 1; i >= 0; i--)
            {
                for (int j = 1; j <= i; j++)
                {
                    T o1 = o[j - 1];
                    T o2 = o[j];
                    if (indexer(o1, o2) > 0)
                    {
                        o.Remove(o1);
                        o.Insert(j, o1);
                    }
                }
            }
        }
    }
}
