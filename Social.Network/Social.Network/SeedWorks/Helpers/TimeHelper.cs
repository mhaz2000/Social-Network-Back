using System;

namespace Social.Network.SeedWorks.Helpers
{
    public static class TimeHelper
    {
        public static string CalculateTime(this DateTime date)
        {
            if ((DateTime.Now - date).TotalMinutes <= 1)
                return "recently";
            else if ((DateTime.Now - date).TotalMinutes < 60)
                return Math.Ceiling((DateTime.Now - date).TotalMinutes) + " Minutes";
            else if ((DateTime.Now - date).TotalHours == 1)
                return "1 Hour";
            else if ((DateTime.Now - date).TotalHours < 24)
                return Math.Ceiling((DateTime.Now - date).TotalHours) + " Hours";
            else if ((DateTime.Now - date).TotalHours == 24)
                return "1 Day";
            else if ((DateTime.Now - date).TotalDays < 365)
                return Math.Ceiling((DateTime.Now - date).TotalDays) + " Days";
            else if ((DateTime.Now - date).TotalDays == 365)
                return "1 Year";
            else
                return Math.Ceiling((DateTime.Now - date).TotalDays / 365) + " Years";

        }
    }
}
