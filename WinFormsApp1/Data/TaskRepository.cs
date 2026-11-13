
using System;
using System.Collections.Generic;
using MySqlConnector;
using WinFormsApp1.Models;

namespace WinFormsApp1.Data
{
    public class TaskRepository
    {
        public void AddTask(TaskItem task)
        {
            using var connection = DbConfig.GetConnection();
            connection.Open();

            string sql = @"
                INSERT INTO tasks
                (title, description, reminder_date)
                VALUES
                (@title, @description, @reminder)";

            using var command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@title", task.Title);
            command.Parameters.AddWithValue(
                "@description", task.Description);
            command.Parameters.AddWithValue(
                "@reminder",
                (object?)task.ReminderDate ?? DBNull.Value);

            command.ExecuteNonQuery();
        }

        public List<TaskItem> GetAllTasks()
        {
            var tasks = new List<TaskItem>();

            using var connection = DbConfig.GetConnection();
            connection.Open();

            string sql = "SELECT * FROM tasks ORDER BY created_at DESC, id DESC";

            using var command = new MySqlCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                tasks.Add(new TaskItem
                {
                    Id = reader.GetInt32("id"),
                    Title = reader.GetString("title"),
                    Description = reader.GetString("description"),
                    ReminderDate = reader.IsDBNull(
                        reader.GetOrdinal("reminder_date"))
                        ? null
                        : reader.GetDateTime("reminder_date"),
                    IsCompleted = reader.GetBoolean("is_completed"),
                    CreatedAt = reader.GetDateTime("created_at")
                });
            }

            return tasks;
        }

        public void CompleteTask(int id)
        {
            using var connection = DbConfig.GetConnection();
            connection.Open();

            string sql = @"
                UPDATE tasks
                SET is_completed = 1
                WHERE id = @id";

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }

        public void DeleteTask(int id)
        {
            using var connection = DbConfig.GetConnection();
            connection.Open();

            string sql = "DELETE FROM tasks WHERE id = @id";

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }

        public void SetReminder(int id, DateTime reminderDate)
        {
            using var connection = DbConfig.GetConnection();
            connection.Open();

            string sql = @"
                UPDATE tasks
                SET reminder_date = @reminder
                WHERE id = @id";

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue(
                "@reminder", reminderDate);
            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }
    }
}
