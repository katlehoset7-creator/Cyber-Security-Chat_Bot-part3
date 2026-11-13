
using System;
using System.Collections.Generic;
using System.Linq;

namespace WinFormsApp1.Services
{
    public class ActivityEntry
    {
        public DateTime Timestamp { get; set; }
        public string Description { get; set; } = "";
    }

    public class ActivityLogger
    {
        private static readonly List<ActivityEntry> activities = new();

        private static readonly object syncLock = new();

        public void Log(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return;

            lock (syncLock)
            {
                activities.Add(new ActivityEntry
                {
                    Timestamp = DateTime.Now,
                    Description = description.Trim()
                });

                // Keep only the 100 most recent activities in memory.
                if (activities.Count > 100)
                    activities.RemoveAt(0);
            }
        }

        public List<ActivityEntry> GetRecentActivities(int count = 10)
        {
            lock (syncLock)
            {
                return activities
                    .OrderByDescending(a => a.Timestamp)
                    .Take(count)
                    .ToList();
            }
        }

        public string GetActivitySummary(int count = 10)
        {
            List<ActivityEntry> recent = GetRecentActivities(count);

            if (recent.Count == 0)
                return "No activities recorded yet.";

            return string.Join(
                Environment.NewLine,
                recent.Select(a =>
                    $"[{a.Timestamp:dd MMM yyyy HH:mm:ss}] {a.Description}"));
        }

        public void ClearActivities()
        {
            lock (syncLock)
            {
                activities.Clear();
            }
        }
    }
}
