#region

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

#endregion

namespace SSD.Framework.Collections
{
    public static class IEnumerableExtensions
    {
        public static SortableBindingList<T> ToSortableBindingList<T>(this IEnumerable<T> lst)
        {
            if (lst != null)
            {
                SortableBindingList<T> lst1 = new SortableBindingList<T>();
                foreach (T obj in lst)
                    lst1.Add(obj);
                return lst1;
            }
            return null;
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            if (data == null)
                return null;
            var props = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable(typeof(T).FullName);
            for (var i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            var values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                { values[i] = props[i].GetValue(item); }
                table.Rows.Add(values);
            }
            return table;
        }
    }
}
