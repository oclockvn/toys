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
            DateTime dt;
            var succeed = DateTime.TryParseExact(dateString, format, CultureInfo.CreateSpecificCulture(culture), DateTimeStyles.None, out dt);

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

            string result = string.Empty;
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
                result = (ts.Days > dayOfMonth) && (now.Month - dateTime.Month > 1)
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

        public static bool Intersects(this DateTime startDate, DateTime endDate, DateTime intersectingStartDate, DateTime intersectingEndDate)
        {
            return (intersectingEndDate >= startDate && intersectingStartDate <= endDate);
        }

        /// <summary>
        /// DateDiff in SQL style. 
        /// Datepart implemented: 
        ///     "year" (abbr. "yy", "yyyy"), 
        ///     "quarter" (abbr. "qq", "q"), 
        ///     "month" (abbr. "mm", "m"), 
        ///     "day" (abbr. "dd", "d"), 
        ///     "week" (abbr. "wk", "ww"), 
        ///     "hour" (abbr. "hh"), 
        ///     "minute" (abbr. "mi", "n"), 
        ///     "second" (abbr. "ss", "s"), 
        ///     "millisecond" (abbr. "ms").
        /// </summary>
        /// <param name="DatePart"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public static Int64 DateDiff(this DateTime StartDate, String DatePart, DateTime EndDate)
        {
            Int64 DateDiffVal = 0;
            Calendar cal = System.Threading.Thread.CurrentThread.CurrentCulture.Calendar;
            TimeSpan ts = new TimeSpan(EndDate.Ticks - StartDate.Ticks);
            switch (DatePart.ToLower().Trim())
            {
                #region year
                case "year":
                case "yy":
                case "yyyy":
                    DateDiffVal = (Int64)(cal.GetYear(EndDate) - cal.GetYear(StartDate));
                    break;
                #endregion

                #region quarter
                case "quarter":
                case "qq":
                case "q":
                    DateDiffVal = (Int64)((((cal.GetYear(EndDate)
                                        - cal.GetYear(StartDate)) * 4)
                                        + ((cal.GetMonth(EndDate) - 1) / 3))
                                        - ((cal.GetMonth(StartDate) - 1) / 3));
                    break;
                #endregion

                #region month
                case "month":
                case "mm":
                case "m":
                    DateDiffVal = (Int64)(((cal.GetYear(EndDate)
                                        - cal.GetYear(StartDate)) * 12
                                        + cal.GetMonth(EndDate))
                                        - cal.GetMonth(StartDate));
                    break;
                #endregion

                #region day
                case "day":
                case "d":
                case "dd":
                    DateDiffVal = (Int64)ts.TotalDays;
                    break;
                #endregion

                #region week
                case "week":
                case "wk":
                case "ww":
                    DateDiffVal = (Int64)(ts.TotalDays / 7);
                    break;
                #endregion

                #region hour
                case "hour":
                case "hh":
                    DateDiffVal = (Int64)ts.TotalHours;
                    break;
                #endregion

                #region minute
                case "minute":
                case "mi":
                case "n":
                    DateDiffVal = (Int64)ts.TotalMinutes;
                    break;
                #endregion

                #region second
                case "second":
                case "ss":
                case "s":
                    DateDiffVal = (Int64)ts.TotalSeconds;
                    break;
                #endregion

                #region millisecond
                case "millisecond":
                case "ms":
                    DateDiffVal = (Int64)ts.TotalMilliseconds;
                    break;
                #endregion

                default:
                    throw new Exception(String.Format("DatePart \"{0}\" is unknown", DatePart));
            }
            return DateDiffVal;
        }
    }
}
