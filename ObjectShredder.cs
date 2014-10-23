using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Extensions
{
    public class ObjectShredder<T>
    {
        private FieldInfo[] _fi;
        private Dictionary<string, int> _ordinalMap;
        private PropertyInfo[] _pi;
        private Type _type;

        public ObjectShredder()
        {
            this._type = typeof(T);
            this._fi = this._type.GetFields();
            this._pi = this._type.GetProperties();
            this._ordinalMap = new Dictionary<string, int>();
        }

        public DataTable ExtendTable(DataTable table, Type type)
        {
            // value is type derived from T, may need to extend table.
            foreach (var f in type.GetFields())
            {
                if (this._ordinalMap.ContainsKey(f.Name)) continue;

                var dc = table.Columns.Contains(f.Name) ? table.Columns[f.Name] : table.Columns.Add(f.Name, f.FieldType);
                this._ordinalMap.Add(f.Name, dc.Ordinal);
            }
            foreach (var p in type.GetProperties())
            {
                if (this._ordinalMap.ContainsKey(p.Name)) continue;

                var colType = p.PropertyType;

                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    colType = colType.GetGenericArguments()[0];
                }

                var dc = table.Columns.Contains(p.Name) ? table.Columns[p.Name] : table.Columns.Add(p.Name, colType);
                this._ordinalMap.Add(p.Name, dc.Ordinal);
            }
            return table;
        }

        public DataTable Shred(IEnumerable<T> source, DataTable table, LoadOption? options)
        {
            if (typeof(T).IsPrimitive) return this.ShredPrimitive(source, table, options);
            if (table == null) table = new DataTable(typeof(T).Name);

            // now see if need to extend data-table base on the type T + build ordinal map
            table = this.ExtendTable(table, typeof(T));

            table.BeginLoadData();
            using (IEnumerator<T> e = source.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    if (options != null)
                    {
                        table.LoadDataRow(this.ShredObject(table, e.Current), (LoadOption)options);
                    }
                    else
                    {
                        table.LoadDataRow(this.ShredObject(table, e.Current), true);
                    }
                }
            }
            table.EndLoadData();
            return table;
        }

        public object[] ShredObject(DataTable table, T instance)
        {
            var fi = this._fi;
            var pi = this._pi;

            if (instance.GetType() != typeof(T))
            {
                this.ExtendTable(table, instance.GetType());
                fi = instance.GetType().GetFields();
                pi = instance.GetType().GetProperties();
            }

            var values = new object[table.Columns.Count];
            foreach (var f in fi)
            {
                values[this._ordinalMap[f.Name]] = f.GetValue(instance);
            }

            foreach (var p in pi)
            {
                values[this._ordinalMap[p.Name]] = p.GetValue(instance, null);
            }
            return values;
        }

        public DataTable ShredPrimitive(IEnumerable<T> source, DataTable table, LoadOption? options)
        {
            if (table == null) table = new DataTable(typeof(T).Name);
            if (!table.Columns.Contains("Value")) table.Columns.Add("Value", typeof(T));

            table.BeginLoadData();
            using (var e = source.GetEnumerator())
            {
                var values = new object[table.Columns.Count];
                while (e.MoveNext())
                {
                    values[table.Columns["Value"].Ordinal] = e.Current;

                    if (options != null) table.LoadDataRow(values, (LoadOption)options);
                    else table.LoadDataRow(values, true);
                }
            }
            table.EndLoadData();
            return table;
        }
    }
}
