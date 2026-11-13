
using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsApp1.Data;
using WinFormsApp1.Models;

namespace WinFormsApp1.Services
{
    public class ReminderService
    {
        private readonly TaskRepository repository = new();

        private readonly HashSet<int> notifiedTasks = new();

        public List<TaskItem> GetDueReminders()
        {
            var dueTasks = new List<TaskItem>();

            foreach (var task in repository.GetAllTasks())
            {
                if (!task.IsCompleted &&
                    task.ReminderDate.HasValue &&
                    task.ReminderDate.Value <= DateTime.Now &&
                    !notifiedTasks.Contains(task.Id))
                {
                    dueTasks.Add(task);
                    notifiedTasks.Add(task.Id);
                }
            }

            return dueTasks;
        }
    }
}
