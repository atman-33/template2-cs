using System.Collections.ObjectModel;

namespace Template2.Domain.Modules.Helpers
{
    public static class ObservableCollectionHelper
    {
        /// <summary>
        /// ObservableCollectionをソート　
        /// ※new OvervableCollectionのコンストラクタでソートした方が良いため、このメソッドは利用しなくて良い
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collections"></param>
        /// <param name="sort"></param>
        public static void Sort<T>(ref ObservableCollection<T> collections,
                                   Func<IOrderedEnumerable<T>> sort)
        {
            //// <利用例>
            ////ObservableCollectionHelper.Sort(ref _sampleTableRecords,
            ////    () =>
            ////    {
            ////        return _sampleTableRecords.OrderByDescending(record => record.SampleId);
            ////    });
            
            ObservableCollection<T> temp;
            temp = new ObservableCollection<T>(sort());

            collections.Clear();
            foreach (T item in temp)
            {
                collections.Add(item);
            }
        }
    }
}
