using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
//using Newtonsoft.Json;

namespace Extensions
{
    public static class ObjectExtensions
    {
        //private static readonly JsonSerializerSettings _JsonDotNetSettingsSerialize = new JsonSerializerSettings()
        //{
        //    DateFormatHandling = DateFormatHandling.IsoDateFormat, // DEFAULT
        //    DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, // Ignore default value properties on serializer, populate on deserialize
        //    MissingMemberHandling = MissingMemberHandling.Ignore, // DEFAULT
        //    NullValueHandling = NullValueHandling.Ignore, // Ignore null values
        //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore, // Ignore Reference Loops
        //};

        #region Cloning

        public static T CloneDeepBinary<T>(this T source)
        {
            if (!typeof(T).IsSerializable) throw new ArgumentException("The type must be serializable.", "source");

            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null)) return default(T);

            // Construct a serialization formatter that does all the hard work
            IFormatter formatter = new BinaryFormatter();

            // Construct a temporary memory stream
            using (var stream = new MemoryStream())
            {
                // Serialize the object graph into the memory stream
                formatter.Serialize(stream, source);
                // Seek back to the start of the memory stream before deserializing
                stream.Seek(0, SeekOrigin.Begin);
                // Deserialize the graph into a new set of objects and return the root of the graph (deep copy) to the caller
                return (T)formatter.Deserialize(stream);
            }
        }

        public static T CloneDeepJson<T>(this T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null)) return default(T);

            // Serialize object to JSON to break off references
            var serialized = SerializeToJsonNative(source);
            if (string.IsNullOrWhiteSpace(serialized)) throw new ApplicationException("Failed to serialize to JSON");

            // Deserialize from JSON to re-hydrate new object
            var deserialized = serialized.DeserializeFromJsonNative<T>();
            if (deserialized == null) throw new ApplicationException("Failed to Deserialize from JSON");

            return deserialized;
        }

        //public static string SerializeToJson<T>(this T t, JsonSerializerSettings settings = null)
        //{
        //    if (settings == null) settings = _JsonDotNetSettingsSerialize;
        //    return JsonConvert.SerializeObject(t, settings);
        //}

        public static string SerializeToJsonNative<T>(this T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        #endregion

        #region Casting

        /// <summary>
        /// Generic TryCast with support for nullable types and Guids.
        ///
        /// Credit to Eric Burcham
        /// http://stackoverflow.com/a/10839349/578859
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T TryCast<T>(this object value)
        {
            var type = typeof(T);

            // If the type is nullable and the result should be null, set a null value.
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && (value == null || value == DBNull.Value))
            {
                return default(T);
            }

            // Convert.ChangeType fails on Nullable<T> types.  We want to try to cast to the underlying type anyway.
            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

            try
            {
                // Just one edge case you might want to handle.
                if (underlyingType == typeof(Guid))
                {
                    if (value is string)
                    {
                        value = new Guid(value as string);
                    }
                    if (value is byte[])
                    {
                        value = new Guid(value as byte[]);
                    }

                    return (T)Convert.ChangeType(value, underlyingType);
                }

                return (T)Convert.ChangeType(value, underlyingType);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                if (Debugger.IsAttached) Debugger.Break();
                return default(T);
            }
        }

        #endregion
    }
}
