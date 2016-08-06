using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashForReddit
{
    static class Helpers
    {
        public static void Sort<T>(this ObservableCollection<T> collection) where T : IComparable
        {
            List<T> sorted = collection.OrderBy(x => x).ToList();
            for (int i = 0; i < sorted.Count(); i++)
                collection.Move(collection.IndexOf(sorted[i]), i);
        }

        public static string GetElapsedTime(long created_utc)
        {
            string result = "";
            var offset = DateTime.UtcNow.Subtract(DateTimeOffset.FromUnixTimeSeconds(created_utc).UtcDateTime);
            if (offset.TotalSeconds < 60)
                result = $"{(int)offset.TotalSeconds}s ago";
            else if (offset.TotalMinutes < 60)
                result = $"{(int)offset.TotalMinutes}m ago";
            else if (offset.TotalHours < 24)
                result = $"{(int)offset.TotalHours}h ago";
            else if (offset.TotalDays < 365)
                result = $"{(int)offset.TotalDays}d ago";
            else
                result = $"{(int)(offset.TotalDays / 365)}y ago";

            return result;
        }
    }
}
