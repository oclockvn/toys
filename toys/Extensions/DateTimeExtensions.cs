using System;
using System.Globalization;
using toys.Extensions.i18n;

namespace toys.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Try parse input datetime string into datetime using specific culture
        /// </summary>
        /// <param name="dateString">a string of datetime to parse</param>
        /// <param name="format">dateString input format</param>
        /// <param name="culture">specify culture info to parse</param>
        /// <returns>return DateTime if parse success, otherwise return null</returns>
        public static DateTime? ToDateTime(this string dateString, string format = "dd/MM/yyyy", string culture = "vi-vn")
        {
            var succeed = DateTime.TryParseExact(dateString, format, CultureInfo.CreateSpecificCulture(culture), DateTimeStyles.None, out var dt);

            if (succeed)
                return dt;

            return null;
        }

        private static string GetResource(string name, string culture = "vi") => AppResources.ResourceManager.GetString(name, new CultureInfo(culture));

        /// <summary>
        /// Return an user-friendly date time like "2 days ago"
        /// </summary>
        /// <param name="dateTime">a time in past to compare</param>
        /// <param name="culture">specific culture to display. Default is Vietnamese.
        /// </param>
        /// <returns>a string of date time formatted</returns>
        public static string ToTimeAgo(this DateTime dateTime, string culture = "vi")
        {
            var now = DateTime.Now;

            if (dateTime > now)
                return string.Empty;

            string result;
            var ts = now.Subtract(dateTime);

            if (ts <= TimeSpan.FromSeconds(60))
            {
                result = ts.Seconds > 1
                    ? string.Format(GetResource("TimeAgo_Seconds", culture), ts.Seconds)
                    : GetResource("TimeAgo_Second", culture);
            }
            else if (ts <= TimeSpan.FromMinutes(60))
            {
                result = ts.Minutes > 1
                    ? string.Format(GetResource("TimeAgo_Minutes", culture), ts.Minutes)
                    : GetResource("TimeAgo_Minute", culture);
            }
            else if (ts <= TimeSpan.FromHours(24))
            {
                result = ts.Hours > 1
                    ? string.Format(GetResource("TimeAgo_Hours", culture), ts.Hours)
                    : GetResource("TimeAgo_Hour", culture);
            }
            else if (ts <= TimeSpan.FromDays(30))
            {
                result = ts.Days > 1
                    ? string.Format(GetResource("TimeAgo_Days", culture), ts.Days)
                    : GetResource("TimeAgo_Day", culture);
            }
            else if (ts <= TimeSpan.FromDays(365))
            {
                const int dayOfMonth = 30;
                result = ts.Days > dayOfMonth && now.Month - dateTime.Month > 1
                    ? string.Format(GetResource("TimeAgo_Months", culture), ts.Days / dayOfMonth)
                    : GetResource("TimeAgo_Month", culture);
            }
            else
            {
                result = ts.Days > 365
                    ? string.Format(GetResource("TimeAgo_Years", culture), ts.Days / 365)
                    : GetResource("TimeAgo_Year", culture);
            }

            return result;
        }

        /// <summary>
        /// Intersectses the specified end date.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="intersectingStartDate">The intersecting start date.</param>
        /// <param name="intersectingEndDate">The intersecting end date.</param>
        /// <returns></returns>
        public static bool Intersects(this DateTime startDate, DateTime endDate, DateTime intersectingStartDate, DateTime intersectingEndDate)
        {
            return intersectingEndDate >= startDate && intersectingStartDate <= endDate;
        }

        /// <summary>
        /// DateDiff in SQL style.
        /// Datepart implemented:
        /// "year" (abbr. "yy", "yyyy"),
        /// "quarter" (abbr. "qq", "q"),
        /// "month" (abbr. "mm", "m"),
        /// "day" (abbr. "dd", "d"),
        /// "week" (abbr. "wk", "ww"),
        /// "hour" (abbr. "hh"),
        /// "minute" (abbr. "mi", "n"),
        /// "second" (abbr. "ss", "s"),
        /// "millisecond" (abbr. "ms").
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="datePart">The date part.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static long DateDiff(this DateTime startDate, string datePart, DateTime endDate)
        {
            long dateDiffVal;
            var cal = System.Threading.Thread.CurrentThread.CurrentCulture.Calendar;
            var ts = new TimeSpan(endDate.Ticks - startDate.Ticks);
            switch (datePart.ToLower().Trim())
            {
                #region year
                case "year":
                case "yy":
                case "yyyy":
                    dateDiffVal = cal.GetYear(endDate) - cal.GetYear(startDate);
                    break;
                #endregion

                #region quarter
                case "quarter":
                case "qq":
                case "q":
                    dateDiffVal = (cal.GetYear(endDate) - cal.GetYear(startDate)) * 4
                                  + (cal.GetMonth(endDate) - 1) / 3
                                  - (cal.GetMonth(startDate) - 1) / 3;
                    break;
                #endregion

                #region month
                case "month":
                case "mm":
                case "m":
                    dateDiffVal = (cal.GetYear(endDate) - cal.GetYear(startDate)) * 12
                                  + cal.GetMonth(endDate)
                                  - cal.GetMonth(startDate);
                    break;
                #endregion

                #region day
                case "day":
                case "d":
                case "dd":
                    dateDiffVal = (long)ts.TotalDays;
                    break;
                #endregion

                #region week
                case "week":
                case "wk":
                case "ww":
                    dateDiffVal = (long)(ts.TotalDays / 7);
                    break;
                #endregion

                #region hour
                case "hour":
                case "hh":
                    dateDiffVal = (long)ts.TotalHours;
                    break;
                #endregion

                #region minute
                case "minute":
                case "mi":
                case "n":
                    dateDiffVal = (long)ts.TotalMinutes;
                    break;
                #endregion

                #region second
                case "second":
                case "ss":
                case "s":
                    dateDiffVal = (long)ts.TotalSeconds;
                    break;
                #endregion

                #region millisecond
                case "millisecond":
                case "ms":
                    dateDiffVal = (long)ts.TotalMilliseconds;
                    break;
                #endregion

                default:
                    throw new Exception($"DatePart \"{datePart}\" is unknown");
            }
            return dateDiffVal;
        }
    }
}
