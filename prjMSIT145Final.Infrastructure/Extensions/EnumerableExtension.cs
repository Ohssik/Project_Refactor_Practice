using System.Collections;
using System.Data;
using System.Reflection;

namespace iProcurementWebApi.Infrastructure.Extensions
{
    public static class EnumerableExtension
    {
        public static bool IsAny<T>(this IEnumerable<T> list)
        {
            if(list == null)
            {
                return false;
            }

            return list.Any();
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            // 獲取所有的屬性
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                // 設置 DataTable 的列的名稱和類型
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    // 插入屬性值到 DataTable
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}
