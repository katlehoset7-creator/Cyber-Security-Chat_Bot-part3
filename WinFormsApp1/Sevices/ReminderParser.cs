
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace WinFormsApp1.Services
{
    public class ParsedReminder
    {
        public string TaskTitle { get; set; } = "";
        public DateTime? ReminderDate { get; set; }
        public bool IsValid => !string.IsNullOrWhiteSpace(TaskTitle)
                               && ReminderDate.HasValue;
    }

    public class ReminderParser
    {
        public ParsedReminder Parse(string input)
        {
            ParsedReminder result = new ParsedReminder();

            if (string.IsNullOrWhiteSpace(input))
                return result;

            string text = input.Trim();

            // Remove the reminder command.
            text = Regex.Replace(
                text,
                @"^(please\s+)?(remind me to|remind me|set a reminder to|create a reminder to)\s+",
                "",
                RegexOptions.IgnoreCase);

            DateTime now = DateTime.Now;
            DateTime? date = null;

            // Recognise "tomorrow" or "today".
            Match dayMatch = Regex.Match(
                text,
                @"\b(today|tomorrow)\b",
                RegexOptions.IgnoreCase);

            if (dayMatch.Success)
            {
                date = dayMatch.Value.Equals(
                    "tomorrow",
                    StringComparison.OrdinalIgnoreCase)
                    ? now.Date.AddDays(1)
                    : now.Date;
            }

            // Recognise times such as 3 PM, 3:30 PM or 15:30.
            Match timeMatch = Regex.Match(
                text,
                @"\b(?:at\s+)?(\d{1,2})(?::(\d{2}))?\s*(am|pm)?\b",
                RegexOptions.IgnoreCase);

            int hour = 9;
            int minute = 0;

            if (timeMatch.Success)
            {
                int parsedHour = int.Parse(timeMatch.Groups[1].Value);

                int parsedMinute = timeMatch.Groups[2].Success
                    ? int.Parse(timeMatch.Groups[2].Value)
                    : 0;

                string meridiem = timeMatch.Groups[3].Value.ToLowerInvariant();

                bool validTime = parsedMinute <= 59;

                if (meridiem == "am" || meridiem == "pm")
                {
                    validTime = validTime &&
                                parsedHour >= 1 &&
                                parsedHour <= 12;

                    if (validTime)
                    {
                        hour = parsedHour % 12;

                        if (meridiem == "pm")
                            hour += 12;
                    }
                }
                else
                {
                    validTime = validTime &&
                                parsedHour >= 0 &&
                                parsedHour <= 23;

                    if (validTime)
                        hour = parsedHour;
                }

                if (!validTime)
                    return result;

                minute = parsedMinute;
            }

            if (date.HasValue)
            {
                date = date.Value.AddHours(hour).AddMinutes(minute);
            }

            // Remove date and time phrases from the task title.
            string taskTitle = text;

            if (dayMatch.Success)
            {
                taskTitle = Regex.Replace(
                    taskTitle,
                    @"\b(today|tomorrow)\b",
                    "",
                    RegexOptions.IgnoreCase);
            }

            if (timeMatch.Success)
            {
                taskTitle = Regex.Replace(
                    taskTitle,
                    @"\b(at\s+)?\d{1,2}(?::\d{2})?\s*(am|pm)?\b",
                    "",
                    RegexOptions.IgnoreCase);
            }

            taskTitle = Regex.Replace(
                taskTitle,
                @"\s+",
                " ").Trim(' ', '.', ',');

            result.TaskTitle = taskTitle;

            if (date.HasValue && date.Value > now)
                result.ReminderDate = date;

            return result;
        }
    }
}
