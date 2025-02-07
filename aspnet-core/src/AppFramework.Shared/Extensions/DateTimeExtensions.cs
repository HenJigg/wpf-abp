using System; 

namespace AppFramework.Shared
{
    public static class DateTimeExtensions
    {
        public static DateTime GetFirstDate(this DateTime? date)
        {
            if (date == null)
                throw new ArgumentNullException(nameof(date));

            return Convert.ToDateTime(Convert.ToDateTime(date).ToString("D"));
        }

        public static DateTime GetLastDate(this DateTime? date)
        {
            if (date == null)
                throw new ArgumentNullException(nameof(date));

            return Convert.ToDateTime((Convert.ToDateTime(date).AddDays(1).ToString("D"))).AddSeconds(-1);
        }
    }
}
