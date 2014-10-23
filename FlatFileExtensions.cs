using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Extensions
{
    public class FlatFileExtensions
    {
        public static EnumerableRowCollection<DataRow> ConvertFlatFileToDataRowList(string filename, char delimiter)
        {
            var reader = File.ReadLines(filename);

            var data = new DataTable();

            var headers = reader.First().Split(delimiter);
            foreach (var header in headers) data.Columns.Add(header);

            var records = reader.Skip(1);
            foreach (var record in records) data.Rows.Add(record.Split(delimiter));

            return data.AsEnumerable();
        }
    }
}
