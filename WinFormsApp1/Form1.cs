
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;
using WinFormsApp1.Data;
using WinFormsApp1.Models;
namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        // These controls are created in this file, inside the existing Form1.
        private TextBox? chatDisplay;
        private TextBox? userInput;
        private Button? sendButton;

        private Panel? sidebar;
        private SoundPlayer? greetingPlayer;

        // Generic collections used by the chatbot.
        private Dictionary<string, string> memory =
            new Dictionary<string, string>();

        private Dictionary<string, List<string>> responses =
            new Dictionary<string, List<string>>();

        private Random random = new Random();

        private string currentTopic = "phishing";

        // Delegate: connects the user's message to the response method.
        private delegate string ResponseHandler(string input);
        private ResponseHandler responseHandler;

        public Form1()
        {
            InitializeComponent();

            responseHandler = GetResponse;
        }


        private void Form1_Load_1(object sender, EventArgs e)
        {
            LoadResponses();
            BuildDashboard();

            // Test the connection to the MySQL database.
            try
            {
                using var connection = DbConfig.GetConnection();
                connection.Open();

                MessageBox.Show(
                    "Black Vault successfully connected to MySQL!",
                    "Database Connection",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Database connection failed:\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            PlayGreeting();
        }


        // Add cybersecurity responses to the dictionary.
        private void LoadResponses()
        {
            responses["password"] = new List<string>
            {
                "Use a long, unique password for every account.",
                "Never use your name or birthday as your password.",
                "Enable multi-factor authentication wherever possible.",
                "Never share your password with another person."
            };

            responses["scam"] = new List<string>
            {
                "Never share your banking PIN or OTP with strangers.",
                "Be careful of messages demanding urgent payments.",
                "Verify suspicious requests using official contact details.",
                "If an offer sounds too good to be true, investigate it."
            };

            responses["phishing"] = new List<string>
            {
                "Check the sender's email address before clicking links.",
                "Avoid unexpected attachments from unknown senders.",
                "Visit official websites directly instead of using suspicious links.",
                "Look for spelling mistakes and unusual requests in messages."
            };

            responses["privacy"] = new List<string>
            {
                "Review your privacy settings on social media.",
                "Limit the personal information you share online.",
                "Check which apps can access your camera and location.",
                "Think carefully before posting personal information publicly."
            };
        }

        // Build the original dashboard inside Form1.
        private void BuildDashboard()
        {
            // Remove the old controls from the visible layout.
            // We reuse the existing logo instead of creating another form.
            Controls.Clear();

            Text = "Black Vault | Cybersecurity Awareness";
            ClientSize = new Size(1000, 650);
            MinimumSize = new Size(1000, 650);
            MaximumSize = new Size(1000, 650);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(13, 24, 42);
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 10);

            // Sidebar.
            sidebar = new Panel();
            sidebar.Location = new Point(0, 0);
            sidebar.Size = new Size(220, 650);
            sidebar.BackColor = Color.FromArgb(20, 34, 56);
            Controls.Add(sidebar);

            // Keep the existing logo image.
            pictureBox1.Location = new Point(42, 18);
            pictureBox1.Size = new Size(128, 87);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.BackColor = Color.Transparent;
            Controls.Add(pictureBox1);
            pictureBox1.BringToFront();

            Label brandLabel = new Label();
            brandLabel.Text = "BLACK VAULT";
            brandLabel.Location = new Point(24, 112);
            brandLabel.Size = new Size(180, 30);
            brandLabel.ForeColor = Color.FromArgb(232, 184, 74);
            brandLabel.Font = new Font("Segoe UI", 15, FontStyle.Bold);
            brandLabel.TextAlign = ContentAlignment.MiddleCenter;
            sidebar.Controls.Add(brandLabel);

            Label menuLabel = new Label();
            menuLabel.Text = "CYBERSECURITY MENU";
            menuLabel.Location = new Point(18, 157);
            menuLabel.Size = new Size(185, 25);
            menuLabel.ForeColor = Color.LightGray;
            menuLabel.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            sidebar.Controls.Add(menuLabel);

            // Navigation and topic buttons.
            AddMenuButton("Home", 195, ShowHome);
            AddMenuButton("Start Chat", 242, StartChat);
            AddMenuButton("Security Tips", 289,
                () => ShowTopic("phishing"));
            AddMenuButton("Password Safety", 336,
                () => ShowTopic("password"));
            AddMenuButton("Scams & Fraud", 383,
                () => ShowTopic("scam"));
            AddMenuButton("Online Privacy", 430,
                () => ShowTopic("privacy"));
            AddMenuButton("My Profile", 477, ShowProfile);
            AddMenuButton("Play Greeting", 524, PlayGreeting);
            AddMenuButton("Exit", 571, ExitApplication);

            // Main heading.
            Label heading = new Label();
            heading.Text = "Cybersecurity Awareness Bot";
            heading.Location = new Point(250, 22);
            heading.Size = new Size(530, 36);
            heading.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            heading.ForeColor = Color.White;
            Controls.Add(heading);

            Label subtitle = new Label();
            subtitle.Text = "Your digital safety starts here.";
            subtitle.Location = new Point(252, 60);
            subtitle.Size = new Size(500, 25);
            subtitle.ForeColor = Color.FromArgb(190, 200, 215);
            Controls.Add(subtitle);

            Button clearButton = CreateStyledButton(
                "Clear Chat", 840, 28, 120, 38);

            clearButton.Click += (s, e) => ClearChat();
            Controls.Add(clearButton);

            // Chat history panel.
            Panel chatPanel = new Panel();
            chatPanel.Location = new Point(250, 100);
            chatPanel.Size = new Size(710, 390);
            chatPanel.BackColor = Color.FromArgb(24, 39, 62);
            chatPanel.Padding = new Padding(12);
            Controls.Add(chatPanel);

            chatDisplay = new TextBox();
            chatDisplay.Location = new Point(12, 12);
            chatDisplay.Size = new Size(686, 366);
            chatDisplay.Multiline = true;
            chatDisplay.ReadOnly = true;
            chatDisplay.ScrollBars = ScrollBars.Vertical;
            chatDisplay.BackColor = Color.FromArgb(24, 39, 62);
            chatDisplay.ForeColor = Color.White;
            chatDisplay.BorderStyle = BorderStyle.None;
            chatDisplay.Font = new Font("Segoe UI", 10);
            chatPanel.Controls.Add(chatDisplay);

            // Message input.
            userInput = new TextBox();
            userInput.Location = new Point(250, 505);
            userInput.Size = new Size(570, 40);
            userInput.Font = new Font("Segoe UI", 11);
            userInput.BackColor = Color.FromArgb(30, 46, 69);
            userInput.ForeColor = Color.White;
            userInput.BorderStyle = BorderStyle.FixedSingle;
            userInput.Anchor = AnchorStyles.Left | AnchorStyles.Right |
                               AnchorStyles.Bottom;
            userInput.KeyDown += UserInput_KeyDown;
            Controls.Add(userInput);

            sendButton = CreateStyledButton(
                "Send", 835, 503, 125, 43);

            sendButton.Click += sendButton_Click;
            Controls.Add(sendButton);

            // Shortcuts below the message input.
            AddQuickButton("Phishing", 250, 560, "phishing");
            AddQuickButton("Passwords", 365, 560, "password");
            AddQuickButton("Scams", 480, 560, "scam");
            AddQuickButton("Privacy", 595, 560, "privacy");

            Button taskAssistantButton = CreateStyledButton(
                "My Tasks", 720, 560, 115, 38);

            taskAssistantButton.Click += (s, e) =>
            {
                using (TaskAssistantForm taskForm = new TaskAssistantForm())
                {
                    taskForm.ShowDialog(this);
                }
            };

            Controls.Add(taskAssistantButton);

            Button quizButton = CreateStyledButton("Play Quiz", 845, 560, 115, 38);

            quizButton.Click += (s, e) =>
            {
                using (QuizForm quizForm = new QuizForm())
                {
                    quizForm.ShowDialog(this);
                }
            };

            Controls.Add(quizButton);



            Label footer = new Label();
            footer.Text = "BLACK VAULT  |  THINK BEFORE YOU CLICK";
            footer.Location = new Point(250, 610);
            footer.Size = new Size(710, 22);
            footer.ForeColor = Color.FromArgb(232, 184, 74);
            footer.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            footer.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(footer);

            AddMessage("Black Vault",
                "Welcome! I am your Cybersecurity Awareness Bot.");

            AddMessage("Black Vault",
                "Choose a topic or type a question about staying safe online.");

            userInput.Focus();
        }

        // Create a consistent style for dashboard buttons.
        private Button CreateStyledButton(
            string text, int x, int y, int width, int height)
        {
            Button button = new Button();

            button.Text = text;
            button.Location = new Point(x, y);
            button.Size = new Size(width, height);
            button.BackColor = Color.FromArgb(232, 184, 74);
            button.ForeColor = Color.FromArgb(13, 24, 42);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            button.Cursor = Cursors.Hand;

            return button;
        }

        // Create a button in the left navigation menu.
        private void AddMenuButton(
            string text, int y, Action action)
        {
            Button button = new Button();

            button.Text = text;
            button.Location = new Point(15, y);
            button.Size = new Size(190, 38);
            button.BackColor = Color.FromArgb(30, 46, 69);
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.Padding = new Padding(12, 0, 0, 0);
            button.Cursor = Cursors.Hand;

            button.Click += (s, e) => action();

            sidebar!.Controls.Add(button);
        }

        // Create a quick topic button.
        private void AddQuickButton(
            string text, int x, int y, string topic)
        {
            Button button = CreateStyledButton(text, x, y, 105, 32);

            button.Click += (s, e) => ShowTopic(topic);
            Controls.Add(button);
        }

        private void StartChat()
        {
            AddMessage("Black Vault",
                "Chat is ready. Ask me about passwords, phishing, scams or privacy.");

            userInput?.Focus();
        }

        private void ShowHome()
        {
            AddMessage("Black Vault",
                "Welcome to Black Vault. Select a security topic or ask me a question.");
        }

        private void ShowTopic(string topic)
        {
            currentTopic = topic;

            string tip = GetTip(topic);

            AddMessage("You", "Show me a tip about " + topic + ".");
            AddMessage("Black Vault", tip);

            userInput?.Focus();
        }

        private void ShowProfile()
        {
            if (memory.ContainsKey("name"))
            {
                AddMessage("Black Vault",
                    "Your saved name is " + memory["name"] + ".");
            }
            else
            {
                AddMessage("Black Vault",
                    "I haven't learned your name yet. Type: My name is [your name].");
            }

            if (memory.ContainsKey("topic"))
            {
                AddMessage("Black Vault",
                    "Your saved favourite topic is " + memory["topic"] + ".");
            }

            userInput?.Focus();
        }

        private void ClearChat()
        {
            if (chatDisplay == null)
                return;

            chatDisplay.Clear();

            AddMessage("Black Vault",
                "Chat cleared. Your saved session memory is still available.");
        }

        private void sendButton_Click(object? sender, EventArgs e)
        {
            SendMessage();
        }

        // Pressing Enter sends the message as well.
        private void UserInput_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SendMessage();
            }
        }

        private void SendMessage()
        {
            if (userInput == null)
                return;

            string input = userInput.Text.Trim();

            if (input == "")
            {
                MessageBox.Show(
                    "Please type a message first.",
                    "Black Vault",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            AddMessage("You", input);

            string answer = responseHandler(input);

            AddMessage("Black Vault", answer);

            userInput.Clear();
            userInput.Focus();
        }

        // Process the user's message and select a response.
        private string GetResponse(string input)
        {
            string message = input.ToLower();

            // Remember the user's name.
            if (message.Contains("my name is"))
            {
                int position = message.IndexOf("my name is") + 10;
                string name = input.Substring(position).Trim();

                if (name.Length == 0)
                    return "Please type your name after 'My name is'.";

                memory["name"] = name;

                return "Nice to meet you, " + name +
                       "! I will remember your name during this session.";
            }

            // Remember the user's favourite topic.
            if (message.Contains("my favourite topic is") ||
                message.Contains("my favorite topic is"))
            {
                string topic = FindTopic(message);

                if (topic == "")
                    return "Please choose password, scam, phishing or privacy.";

                memory["topic"] = topic;
                currentTopic = topic;

                return "I will remember that your favourite topic is " +
                       topic + ". " + GetTip(topic);
            }

            if (message.Contains("interested in privacy"))
            {
                memory["topic"] = "privacy";
                currentTopic = "privacy";

                return "I will remember that you are interested in privacy. " +
                       GetTip("privacy");
            }

            // Detect worried or frightened users.
            if (message.Contains("worried") ||
                message.Contains("scared") ||
                message.Contains("afraid") ||
                message.Contains("anxious"))
            {
                string topic = FindTopic(message);

                if (topic == "")
                    topic = currentTopic;

                return "It is understandable to feel worried. " +
                       "Let's take this one step at a time. " +
                       GetTip(topic);
            }

            // Detect frustration or confusion.
            if (message.Contains("frustrated") ||
                message.Contains("confused") ||
                message.Contains("overwhelmed") ||
                message.Contains("upset"))
            {
                return "That's okay. We can learn one step at a time. " +
                       "Here is one thing you can do: " + GetTip(currentTopic);
            }

            // Recall information stored in session memory.
            if (message.Contains("remember me") ||
                message.Contains("what is my name") ||
                message.Contains("what's my name"))
            {
                if (memory.ContainsKey("name"))
                    return "Your name is " + memory["name"] + ".";

                return "You haven't told me your name yet. Type: My name is [your name].";
            }

            if (message.Contains("my favourite topic") ||
                message.Contains("my favorite topic"))
            {
                if (memory.ContainsKey("topic"))
                    return "Your favourite topic is " + memory["topic"] + ".";

                return "Tell me your favourite topic, for example: My favourite topic is privacy.";
            }

            // Handle follow-up requests.
            if (message.Contains("another tip") ||
                message.Contains("more tips") ||
                message.Contains("next tip"))
            {
                return GetTip(currentTopic);
            }

            if (message.Contains("explain") ||
                message.Contains("more details"))
            {
                return "Let's look at " + currentTopic + " in simple terms. " +
                       GetTip(currentTopic);
            }

            // Recognise cybersecurity keywords.
            string foundTopic = FindTopic(message);

            if (foundTopic != "")
            {
                currentTopic = foundTopic;
                return GetTip(foundTopic);
            }

            if (message.Contains("hello") ||
                message.Contains("hi") ||
                message.Contains("hey"))
            {
                string greeting = "Hello! What would you like to learn about online safety?";

                if (memory.ContainsKey("name"))
                    greeting = "Hello, " + memory["name"] +
                               "! What would you like to learn about online safety?";

                return greeting;
            }

            if (message.Contains("thank"))
                return "You're welcome! Remember to think before you click.";

            if (message.Contains("help"))
                return "You can ask about passwords, scams, phishing and privacy. " +
                       "You can also ask for another tip or ask me to remember your name.";

            return "I'm not sure I understand. Try asking about passwords, " +
                   "scams, phishing or privacy.";
        }

        // Match keywords to one of the chatbot's topics.
        private string FindTopic(string message)
        {
            if (message.Contains("password"))
                return "password";

            if (message.Contains("scam") ||
                message.Contains("fraud") ||
                message.Contains("otp") ||
                message.Contains("banking"))
                return "scam";

            if (message.Contains("phishing") ||
                message.Contains("suspicious link") ||
                message.Contains("fake email") ||
                message.Contains("malicious link"))
                return "phishing";

            if (message.Contains("privacy") ||
                message.Contains("personal information"))
                return "privacy";

            return "";
        }

        // Select a random tip from the chosen topic.
        private string GetTip(string topic)
        {
            if (!responses.ContainsKey(topic))
                topic = "phishing";

            List<string> tips = responses[topic];

            int number = random.Next(tips.Count);

            return tips[number];
        }

        // Display a message in the chat history.

        private void AddMessage(string sender, string message)
        {
            if (chatDisplay == null)
                return;

            // Display the sender's actual name instead of "You".
            if (sender.Equals("You", StringComparison.OrdinalIgnoreCase))
            {
                if (memory.TryGetValue("name", out string? savedName)
                    && !string.IsNullOrWhiteSpace(savedName))
                {
                    sender = savedName;
                }
                else
                {
                    sender = "You";
                }
            }

            chatDisplay.AppendText(
                sender + Environment.NewLine +
                message + Environment.NewLine +
                "----------------------------------------" +
                Environment.NewLine + Environment.NewLine);
        }


        // Play the existing WAV greeting, if the file exists.

        public void PlayGreeting()
        {
            SoundPlayer player = new SoundPlayer("C:\\Users\\Student\\source\\repos\\WinFormsApp1\\WinFormsApp1\\Resources\\greeting_1.wav");
            player.Play();
        }


        private void ExitApplication()
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to close Black Vault?",
                "Exit Black Vault",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                Close();
        }

        // Keep these existing event methods for compatibility with the Designer.
        public void button2_Click(object sender, EventArgs e)
        {
            StartChat();
        }

        public void pictureBox1_Click(object sender, EventArgs e)
        {
            // The existing logo is kept as part of the dashboard.
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            // Kept in case the Designer or project still references this event.
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            greetingPlayer?.Stop();
            greetingPlayer?.Dispose();

            base.OnFormClosed(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
