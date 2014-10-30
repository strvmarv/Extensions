using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
//using Newtonsoft.Json;

namespace Extensions
{
    public static class StringExtensions
    {
        //private static readonly JsonSerializerSettings _JsonDotNetSettingsDeserialize = new JsonSerializerSettings()
        //{
        //    DateFormatHandling = DateFormatHandling.IsoDateFormat, // DEFAULT
        //    DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate, // Ignore default value properties on serializer, populate on deserialize
        //    MissingMemberHandling = MissingMemberHandling.Ignore, // DEFAULT
        //    NullValueHandling = NullValueHandling.Ignore, // Ignore null values
        //    ObjectCreationHandling = ObjectCreationHandling.Auto, // DEFAULT
        //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore, // Ignore Reference Loops
        //};

        #region Conversion

        public static T ToEnum<T>(this string input)
        {
            if (typeof(T).BaseType != typeof(Enum))
            {
                throw new InvalidCastException();
            }
            if (Enum.IsDefined(typeof(T), input) == false)
            {
                throw new InvalidCastException();
            }
            return (T)Enum.Parse(typeof(T), input);
        }

        #endregion

        #region Hashing

        public static byte[] GenerateHash(this string input)
        {
            return Encoding.Unicode.GetBytes(input).GenerateHash();
        }

        public static string GenerateHashBase64String(string input)
        {
            return System.Convert.ToBase64String(GenerateHash(input));
        }

        #endregion

        #region Manipulation

        public static string SafeSubstring(this string input, int length = 0, int startIndex = 0)
        {
            // If null or empty is passed, just return input
            if (string.IsNullOrEmpty(input)) return input;

            // Length is optional, set it to input length if zero
            var _length = length > 0 ? length : input.Length;

            // Make sure startIndex plus length is possible
            if (input.Length >= (startIndex + _length)) return input.Substring(startIndex, _length);

            // Make sure startIndex does not exceed input length
            if (input.Length > startIndex) return input.Substring(startIndex);
            else throw new ApplicationException(string.Format("Start Index ({0}) cannot exceed string input Length ({1})", startIndex, input.Length));
        }

        public static string SafeTrim(this string input)
        {
            if (input == null) return input;
            return input.Trim();
        }

        public static string SafeTrimEnd(this string input)
        {
            if (input == null) return input;
            return input.TrimEnd();
        }

        public static string SafeTrimStart(this string input)
        {
            if (input == null) return input;
            return input.TrimStart();
        }

        public static string SanitizeHtmlString(this string input)
        {
            if (string.IsNullOrEmpty(input)) return null;

            // replace line breaks
            input = Regex.Replace(input, @"\r\n?|\n|\u000d|\u000a", @"<br/>");

            // replace horizontal tabs
            input = Regex.Replace(input, @"\t|\u0009", @"&emsp;");

            // replace reserved chars
            input = Regex.Replace(input, @"\|", @"");

            // remove unit separator
            input = input.Replace((char)(0x1F), ' ');

            return input;
        }

        public static string SanitizeXmlString(this string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;

            var buffer = new StringBuilder(xml.Length);

            foreach (var c in xml)
            {
                if (IsLegalXmlChar(c))
                {
                    buffer.Append(c);
                }
            }

            return buffer.ToString();
        }

        public static string ToTitleCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) return null;
            string v = input.ToLower();
            var ti = new CultureInfo("en-US", false).TextInfo;
            string r = ti.ToTitleCase(v);

            return r;
        }

        public static string Truncate(this string input, int length)
        {
            if ((string.IsNullOrEmpty(input))) return input;
            var r = input.Trim();
            if (input.Length > length) r = input.Remove(length);
            return r;
        }

        #endregion

        #region Serialization

        //public static T DeserializeFromJson<T>(this string jsonString, JsonSerializerSettings settings = null)
        //{
        //    if (settings == null) settings = _JsonDotNetSettingsDeserialize;
        //    return JsonConvert.DeserializeObject<T>(jsonString, settings);
        //}

        public static T DeserializeFromJsonNative<T>(this string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }

        #endregion

        #region Validation

        public static bool IsLegalXmlChar(int character)
        {
            return
                (
                    character == 0x9 /* == '\t' == 9   */          ||
                    character == 0xA /* == '\n' == 10  */          ||
                    character == 0xD /* == '\r' == 13  */          ||
                    (character >= 0x20 && character <= 0xD7FF) ||
                    (character >= 0xE000 && character <= 0xFFFD) ||
                    (character >= 0x10000 && character <= 0x10FFFF)
                );
        }

        public static bool IsNumber(this string value)
        {
            if (!(String.IsNullOrEmpty(value)))
            {
                var regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
                return regex.IsMatch(value);
            }

            return false;
        }

        #endregion
    }
}
