using System.Collections.Generic;

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Class used to reverse the sorting of one list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReverseComparer<T> : IComparer<T>
    {
        public int Compare(T x, T y)
        {
            return Comparer<T>.Default.Compare(y, x);
        }
    }

    public class ReverseSortedDictionary<TKey, TValue> : SortedDictionary<TKey, TValue>
    {
        public ReverseSortedDictionary() : base(new ReverseComparer<TKey>())
        {
        }
        public ReverseSortedDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary, new ReverseComparer<TKey>())
        {
        }
    }
}