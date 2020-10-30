using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SSD.Framework.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> lst, IEnumerable<T> addList)// where T : new()
        {
            if (addList == null) return;
            foreach (var i in addList) lst.Add(i);
        }
    }
}
