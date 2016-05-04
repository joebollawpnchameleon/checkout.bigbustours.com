using System;
using System.Collections;

namespace Common.Model
{
    public class TimeZoneComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            TimeZoneInformation tzx, tzy;

            tzx = x as TimeZoneInformation;
            tzy = y as TimeZoneInformation;

            if (tzx == null || tzy == null)
            {
                throw new ArgumentException("Parameter null or wrong type");
            }

            int biasDifference = tzx.Bias - tzy.Bias;

            if (biasDifference == 0)
            {
                return tzx.DisplayName.CompareTo(tzy.DisplayName);
            }
            else
            {
                return biasDifference;
            }
        }
    }
}
