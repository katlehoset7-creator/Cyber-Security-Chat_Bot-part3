
using System;
using System.Drawing;
using System.Windows.Forms;
using WinFormsApp1.Data;
using WinFormsApp1.Models;
using WinFormsApp1.Services;

namespace WinFormsApp1
{
    public partial class TaskAssistantForm : Form
    {
        private readonly TaskRepository repository = new();

        private readonly ReminderService reminderService = new();

        private readonly System.Windows.Forms.Timer reminderTimer = new();


        private TextBox txtTitle = null!;
        private TextBox txtDescription = null!;
        private DateTimePicker dtpReminder = null!;
        private CheckBox chkReminder = null!;
        private DataGridView taskGrid = null!;

        public TaskAssistantForm()
        {
            InitializeComponent();
            BuildTaskInterface();
            LoadTasks();

            // Check for due reminders every 30 seconds.
            reminderTimer.Interval = 30000;
            reminderTimer.Tick += ReminderTimer_Tick;
            reminderTimer.Start();
        }


        private void BuildTaskInterface()
        {
            Text = "Black Vault - Task Assistant";
            Size = new Size(900, 650);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(15, 23, 42);

            var heading = new Label
            {
                Text = "BLACK VAULT - TASK ASSISTANT",
                ForeColor = Color.Gold,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(30, 20),
                AutoSize = true
            };
            Controls.Add(heading);

            var lblTitle = new Label
            {
                Text = "Task Title",
                ForeColor = Color.White,
                Location = new Point(30, 80),
                AutoSize = true
            };
            Controls.Add(lblTitle);

            txtTitle = new TextBox
            {
                Location = new Point(30, 105),
                Width = 380
            };
            Controls.Add(txtTitle);

            var lblDescription = new Label
            {
                Text = "Description",
                ForeColor = Color.White,
                Location = new Point(30, 145),
                AutoSize = true
            };
            Controls.Add(lblDescription);

            txtDescription = new TextBox
            {
                Location = new Point(30, 170),
                Width = 600,
                Height = 60,
                Multiline = true
            };
            Controls.Add(txtDescription);

            chkReminder = new CheckBox
            {
                Text = "Set Reminder",
                ForeColor = Color.White,
                Location = new Point(30, 245),
                AutoSize = true
            };
            Controls.Add(chkReminder);

            dtpReminder = new DateTimePicker
            {
                Location = new Point(170, 245),
                Width = 230,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd HH:mm",
                ShowUpDown = true,
                Enabled = false
            };
            Controls.Add(dtpReminder);

            chkReminder.CheckedChanged += (s, e) =>
            {
                dtpReminder.Enabled = chkReminder.Checked;
            };

            var btnAdd = new Button
            {
                Text = "Add Task",
                Location = new Point(30, 290),
                Size = new Size(130, 40),
                BackColor = Color.Gold
            };
            btnAdd.Click += AddTask_Click;
            Controls.Add(btnAdd);

            var btnComplete = new Button
            {
                Text = "Mark Completed",
                Location = new Point(175, 290),
                Size = new Size(150, 40),
                BackColor = Color.LightGreen
            };
            btnComplete.Click += CompleteTask_Click;
            Controls.Add(btnComplete);

            var btnDelete = new Button
            {
                Text = "Delete Task",
                Location = new Point(340, 290),
                Size = new Size(130, 40),
                BackColor = Color.LightCoral
            };
            btnDelete.Click += DeleteTask_Click;
            Controls.Add(btnDelete);

            taskGrid = new DataGridView
            {
                Location = new Point(30, 350),
                Size = new Size(820, 230),
                ReadOnly = true,
                AllowUserToAddRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill
            };
            Controls.Add(taskGrid);
        }

        private void LoadTasks()
        {
            try
            {
                taskGrid.DataSource = null;
                taskGrid.DataSource = repository.GetAllTasks();

                if (taskGrid.Columns["Id"] != null)
                    taskGrid.Columns["Id"]!.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load tasks:\n" + ex.Message);
            }
        }

        private void AddTask_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a task title.");
                return;
            }

            if (txtTitle.Text.Trim().Length > 150 ||
                txtDescription.Text.Trim().Length > 500)
            {
                MessageBox.Show(
                    "Title must be at most 150 characters " +
                    "and description at most 500 characters.");
                return;
            }

            try
            {
                var task = new TaskItem
                {
                    Title = txtTitle.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    ReminderDate = chkReminder.Checked
                        ? dtpReminder.Value
                        : null
                };

                repository.AddTask(task);

                MessageBox.Show("Task added successfully!");

                txtTitle.Clear();
                txtDescription.Clear();
                chkReminder.Checked = false;

                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to add task:\n" + ex.Message);
            }
        }

        private void CompleteTask_Click(object? sender, EventArgs e)
        {
            if (taskGrid.CurrentRow?.DataBoundItem
                is not TaskItem task)
            {
                MessageBox.Show("Select a task first.");
                return;
            }

            try
            {
                repository.CompleteTask(task.Id);
                MessageBox.Show("Task marked as completed!");
                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to complete task:\n" + ex.Message);
            }
        }

        private void DeleteTask_Click(object? sender, EventArgs e)
        {
            if (taskGrid.CurrentRow?.DataBoundItem
                is not TaskItem task)
            {
                MessageBox.Show("Select a task first.");
                return;
            }

            var confirm = MessageBox.Show(
                "Are you sure you want to delete this task?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                repository.DeleteTask(task.Id);
                MessageBox.Show("Task deleted successfully!");
                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to delete task:\n" + ex.Message);
            }
        }

        private void ReminderTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                var dueReminders = reminderService.GetDueReminders();

                foreach (var task in dueReminders)
                {
                    MessageBox.Show(
                        $"Reminder: {task.Title}\n\n{task.Description}",
                        "Black Vault - Task Reminder",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                reminderTimer.Stop();

                MessageBox.Show(
                    "Unable to check reminders:\n" + ex.Message,
                    "Reminder Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            reminderTimer.Stop();
            reminderTimer.Dispose();

            base.OnFormClosed(e);
        }

    }
}
