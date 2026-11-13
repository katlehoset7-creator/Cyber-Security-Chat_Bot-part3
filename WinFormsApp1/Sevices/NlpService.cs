
using System;
using System.Text.RegularExpressions;

namespace WinFormsApp1.Services
{
    public enum NlpIntent
    {
        Unknown,
        StartQuiz,
        ViewTasks,
        AddTask,
        SetReminder,
        ViewActivityLog,
        CybersecurityTopic
    }

    public class NlpResult
    {
        public NlpIntent Intent { get; set; } = NlpIntent.Unknown;
        public string OriginalText { get; set; } = "";
        public string Topic { get; set; } = "";
        public string TaskTitle { get; set; } = "";
    }

    public class NlpService
    {
        public NlpResult Understand(string? userMessage)
        {
            string original = userMessage?.Trim() ?? "";

            string text = Regex.Replace(
                original.ToLowerInvariant(),
                @"[^\w\s]",
                " ");

            text = Regex.Replace(text, @"\s+", " ").Trim();

            NlpResult result = new NlpResult
            {
                OriginalText = original
            };

            if (string.IsNullOrWhiteSpace(text))
                return result;

            // Activity log commands
            if (ContainsAny(text,
                "activity log",
                "recent activity",
                "what have you done for me",
                "show my history",
                "show your history"))
            {
                result.Intent = NlpIntent.ViewActivityLog;
                return result;
            }

            // Reminder commands
            if (ContainsAny(text,
                "remind me",
                "set a reminder",
                "create a reminder",
                "schedule a reminder"))
            {
                result.Intent = NlpIntent.SetReminder;

                result.TaskTitle = ExtractAfterPhrase(
                    original,
                    "remind me to",
                    "remind me",
                    "set a reminder to",
                    "create a reminder to");

                return result;
            }

            // Quiz commands
            if (ContainsAny(text,
                "quiz",
                "test my knowledge",
                "test my cybersecurity",
                "play a game",
                "ask me questions"))
            {
                result.Intent = NlpIntent.StartQuiz;
                return result;
            }

            // View tasks commands
            if (ContainsAny(text,
                "show my tasks",
                "view my tasks",
                "list my tasks",
                "open my tasks",
                "task list",
                "what are my tasks"))
            {
                result.Intent = NlpIntent.ViewTasks;
                return result;
            }

            // Add task commands
            if (ContainsAny(text,
                "add a task",
                "create a task",
                "new task",
                "add task",
                "create task"))
            {
                result.Intent = NlpIntent.AddTask;

                result.TaskTitle = ExtractAfterPhrase(
                    original,
                    "add a task to",
                    "create a task to",
                    "add task",
                    "create task",
                    "new task");

                return result;
            }

            // Cybersecurity topic recognition
            if (ContainsAny(text, "phishing", "suspicious email"))
            {
                result.Intent = NlpIntent.CybersecurityTopic;
                result.Topic = "phishing";
                return result;
            }

            if (ContainsAny(text, "password", "passphrase"))
            {
                result.Intent = NlpIntent.CybersecurityTopic;
                result.Topic = "password";
                return result;
            }

            if (ContainsAny(text, "scam", "fraud"))
            {
                result.Intent = NlpIntent.CybersecurityTopic;
                result.Topic = "scam";
                return result;
            }

            if (ContainsAny(text, "privacy", "personal data"))
            {
                result.Intent = NlpIntent.CybersecurityTopic;
                result.Topic = "privacy";
                return result;
            }

            return result;
        }

        private static bool ContainsAny(
            string text,
            params string[] phrases)
        {
            foreach (string phrase in phrases)
            {
                if (text.Contains(phrase))
                    return true;
            }

            return false;
        }

        private static string ExtractAfterPhrase(
            string original,
            params string[] phrases)
        {
            foreach (string phrase in phrases)
            {
                int index = original.IndexOf(
                    phrase,
                    StringComparison.OrdinalIgnoreCase);

                if (index >= 0)
                {
                    return original.Substring(
                        index + phrase.Length).Trim();
                }
            }

            return "";
        }
    }
}
