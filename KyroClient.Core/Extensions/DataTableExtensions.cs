using System.Data;
using System.Dynamic;

namespace KyroClient.Core.Extensions;

public static class DataTableExtensions
{
    public static IEnumerable<dynamic> ToDynamicEnumerable(this DataTable table)
    {
        foreach (DataRow row in table.Rows)
        {
            var dict = new ExpandoObject() as IDictionary<string, object>;
            foreach (DataColumn col in table.Columns)
                dict[col.ColumnName] = row[col];
            yield return dict;
        }
    }
}