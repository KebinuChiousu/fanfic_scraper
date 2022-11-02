using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.VisualBasic;

namespace HtmlGrabber
{

    [HideModuleName()]
    public static class ExtensionModule
    {

        public static DataTable ToDataTable<T>(this IList<T> data)
        {

            var properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                var row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }

            table.AcceptChanges();

            return table;
        }
    }
}