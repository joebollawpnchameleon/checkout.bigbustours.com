using System;

namespace bigbus.checkout.Helpers
{
    public class DateUtil
    {
        public static DateTime NullDate { get { return DateTime.MinValue.AddDays(1); } }
    }
}