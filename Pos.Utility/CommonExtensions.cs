using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Pos.Utility
{
    public static class CommonExtensions
    {
        public static int ToInt(this object obj)
        {
            return Convert.ToInt32(obj);
        }

        public static long ToLong(this object obj)
        {
            return Convert.ToInt64(obj);
        }

        public static short ToShort(this object obj)
        {
            return Convert.ToInt16(obj);
        }

        public static decimal ToDecimal(this object obj)
        {
            return Convert.ToDecimal(obj);
        }

        public static DateTime ToDateTime(this object obj)
        {
            return Convert.ToDateTime(obj);
        }

        public static bool ToBool(this object obj)
        {
            return Convert.ToBoolean(obj);
        }

        public static double ToDouble(this object obj)
        {
            return Convert.ToDouble(obj);
        }

        /// <summary>
        /// Sets the time of the datetime to 11:59:59 PM for the filters. Used for end date filters
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime SetTimeToEndOfTheDay(this object obj)
        {
            var generatedDate = Convert.ToDateTime(obj);
            var generatedDateToEndOfTheDay =
                new DateTime(generatedDate.Year, generatedDate.Month, generatedDate.Day, 23, 59, 59);
            return generatedDateToEndOfTheDay;
        }

        /// <summary>
        /// Sets the time of the datetime to 12:00:00 AM for the filters. Used for start date filters
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime SetTimeToStartOfTheDay(this object obj)
        {
            var generatedDate = Convert.ToDateTime(obj);
            var generatedDateToEndOfTheDay =
                new DateTime(generatedDate.Year, generatedDate.Month, generatedDate.Day, 00, 00, 00);
            return generatedDateToEndOfTheDay;
        }

        /// <summary>
        /// Checks if the given date is between tow other given date
        /// </summary>
        /// <param name="dateToCheck"></param>
        /// <param name="dateRangeStart"></param>
        /// <param name="dateRangeEnd"></param>
        /// <returns></returns>
        public static bool IsBetween(this DateTime? dateToCheck, DateTime? dateRangeStart, DateTime? dateRangeEnd)
        {
            if (dateToCheck == null)
            {
                return false;
            }

            dateRangeStart = dateRangeStart ?? DateTime.MinValue;
            dateRangeEnd = dateRangeEnd ?? DateTime.MinValue;

            return (dateToCheck >= dateRangeStart && dateToCheck <= dateRangeEnd);
        }

        public static string ConvertToSafeJson(object obj)
        {
            var json = string.Empty;
            try
            {
                json = JsonConvert.SerializeObject(obj);
                return json;
            }
            catch (Exception)
            {
                // Ignored
                return json;
            }
        }

        /// <summary>
        /// returns the description value from the display attribute
        /// of given enum.
        /// keywords is comma separated and holds the replacing variable such as @entity.
        /// keywordValues is comma separated and holds the replacing value such as Username.
        /// </summary>
        /// <param name="enumValue"></param>
        /// <param name="keywords"></param>
        /// <param name="keywordValues"></param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum enumValue, string keywords = "", string keywordValues = "")
        {
            var description = enumValue.GetType().GetMember(enumValue.ToString())
                .First()
                ?.GetCustomAttribute<DisplayAttribute>()?.Description;
            if (string.IsNullOrEmpty(keywords) || string.IsNullOrEmpty(keywordValues) ||
                string.IsNullOrEmpty(description)) return description ?? "";
            var keywordsList = keywords.Split(',');
            var keywordValuesList = keywordValues.Split(',');
            var index = 0;

            description = keywordsList.Aggregate(description,
                (current, keyword) => current.Replace(keyword, keywordValuesList[index++]));

            return description ?? "";
        }

        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> list)
        {
            return list == null || !list.Any();
        }

        public static bool ContainsAll<T>(this IEnumerable<T> containingList, IEnumerable<T> lookupList)
        {
            return !lookupList.Except(containingList).Any();
        }

        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(T));
                dcs.WriteObject(stream, a);
                stream.Position = 0;
                return (T)dcs.ReadObject(stream);
            }
        }

        public static List<T> GetAllEnums<T>(this T enumType)
        {
            return ((T[])Enum.GetValues(typeof(T))).ToList();
        }

        public static bool ValidateDateTime(this DateTime datetime)
        {
            return (datetime < SqlDateTime.MinValue.Value || datetime > SqlDateTime.MaxValue.Value);
        }
    }
}
