using System;
using System.Globalization;

namespace RCTechs.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        public static string ToIso8601String(this DateTimeOffset dateTimeOffset)
        {
            if (dateTimeOffset == new DateTimeOffset())
                return null;
            return dateTimeOffset.ToString("O");
        }

        public static string ToIso8601String(this DateTimeOffset? dateTimeOffset)
        {
            return dateTimeOffset.HasValue ? dateTimeOffset.Value.ToIso8601String() : null;
        }

        public static string ToIso8601String(this DateTime date)
        {
            if (date == new DateTime())
                return null;
            return date.ToString("O");
        }

        public static string ToIso8601String(this DateTime? date)
        {
            return date.HasValue ? date.Value.ToIso8601String() : null;
        }

        public static DateTimeOffset ParseDateTimeOffset(this string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                return DateTimeOffset.MinValue;//TODO
            // WwwFormUrlDecoder is bugged and replaces + by space, can't fix with UrlEncode
            if (dateString.Contains(" "))
                return DateTimeOffset.Parse(dateString.Replace(" ", "+"), CultureInfo.InvariantCulture);

            DateTimeOffset dt;
            if (DateTimeOffset.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                return dt;
            else
                return DateTimeOffset.MinValue;
        }

        public static string ToUrlEncodedString(this DateTimeOffset dateTimeOffset)
        {
            return UrlEncode(dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss.FFFFFFFK"));
        }
        public static string ToUrlEncodedString(this DateTimeOffset? dateTimeOffset)
        {
            return dateTimeOffset.HasValue ? dateTimeOffset.Value.ToUrlEncodedString() : null;
        }
        public static string ToUrlEncodedString(this DateTime dateTime)
        {
            return UrlEncode(dateTime.ToString("yyyy-MM-dd HH:mm:ss.FFFFFFF"));
        }
        public static string ToUrlEncodedString(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToUrlEncodedString() : null;
        }

        private static string UrlEncode(string dt)
        {
            string result = dt.Replace(" ", "%20");
            result = result.Replace("+", "%2b");

            return result;
        }

        public static bool TryParseDateTimeOffset(this string dateString)
        {
            try
            {
                var dt = ParseDateTimeOffset(dateString);
                return dt != DateTimeOffset.MinValue;
            }
            catch { }
            return false;
        }

        public static bool TryParseDateTimeOffset(this string dateString, out DateTimeOffset offset)
        {
            try
            {
                offset = ParseDateTimeOffset(dateString);
                return true;
            }
            catch
            {
                offset = default(DateTimeOffset);
            }
            return false;
        }

        public static DateTimeOffset Max(this DateTimeOffset dt1, params DateTimeOffset[] dts)
        {
            DateTimeOffset max = dt1;
            foreach (var dt in dts)
            {
                if (dt > max)
                    max = dt;
            }
            return max;
        }
    }
}
