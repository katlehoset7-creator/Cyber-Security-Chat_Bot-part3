# BLACK VAULT — Cybersecurity Awareness Chatbot

## PROG6221 — Portfolio of Evidence (POE)

**Project Name:** Black Vault  
**Application Type:** Windows Forms Desktop Application  
**Programming Language:** C#  
**Framework:** .NET / Windows Forms  
**Database:** MySQL  
**Development Environment:** Visual Studio 2022  
**Database Management Tool:** MySQL Workbench  
**Database Connector:** MySqlConnector  

---

## 1. Project Overview

Black Vault is an interactive cybersecurity awareness chatbot developed as part of the PROG6221 Portfolio of Evidence (POE).

The application educates users about common cybersecurity threats and encourages safe online behaviour through interactive conversations, cybersecurity tips, task management, reminders, quizzes and activity tracking.

The project was developed progressively across three parts, with each part introducing additional functionality.

The final application combines chatbot functionality, a graphical user interface, database integration and natural-language command recognition.

## 2. Project Objectives

The main objectives of Black Vault are to:

- Educate users about cybersecurity threats and online safety.
- Provide an interactive and user-friendly desktop interface.
- Recognise cybersecurity keywords and respond appropriately.
- Remember user preferences during a chatbot session.
- Detect basic emotional expressions and provide supportive responses.
- Allow users to create, view, complete and delete tasks.
- Schedule reminders for important cybersecurity activities.
- Test cybersecurity knowledge through an interactive quiz.
- Recognise natural-language commands using C# string matching.
- Maintain a timestamped record of recent chatbot activities.

## 3. Technologies Used

| Technology | Purpose |
|---|---|
| C# | Main application programming language |
| Windows Forms | Graphical user interface |
| Visual Studio 2022 | Development and debugging |
| MySQL Server | Persistent task storage |
| MySQL Workbench | Database management |
| MySqlConnector | Connection between C# and MySQL |
| Git | Version control |
| GitHub | Source code repository and releases |

## 4. Part 1 — Basic Cybersecurity Chatbot

The first part introduced the foundation of the chatbot.

### Features

**Voice Greeting**

Black Vault plays an audio greeting to welcome the user.

**Cybersecurity Awareness**

The chatbot provides information about topics including:

- Password security
- Phishing
- Online scams and fraud
- Online privacy
- General online safety

**Keyword Recognition**

The chatbot identifies cybersecurity-related keywords in user messages and generates appropriate responses.

**Interactive Conversations**

Users communicate with Black Vault by typing messages and receiving chatbot responses.

## 5. Part 2 — Enhanced Chatbot and Graphical Interface

The second part expanded the chatbot with a graphical user interface and more advanced conversation features.

### Graphical User Interface

Black Vault uses a Windows Forms dashboard with a dark-themed interface, navigation buttons and an interactive chat area.

### Session Memory

The chatbot remembers information provided by users during the current session.

For example:

`My name is Alex`

The chatbot can remember the name and use it in later responses.

### Topic Preferences

Users can specify their favourite cybersecurity topics.

For example:

`My favourite topic is privacy`

The chatbot remembers the preference and provides relevant information.

### Sentiment Recognition

Black Vault recognises basic emotional expressions such as worry, fear, confusion and frustration.

The chatbot adjusts its responses to provide more supportive explanations and cybersecurity guidance.

### Randomised Cybersecurity Tips

The chatbot provides cybersecurity tips and allows users to request additional advice.

## 6. Part 3 — Advanced Features

Part 3 introduces four major features: Task Assistant, Cybersecurity Quiz, NLP Simulation and Activity Log.

### 6.1 Task Assistant

The Task Assistant allows users to manage cybersecurity-related tasks.

**Implemented functionality:**

- Add new tasks.
- Enter task titles and descriptions.
- View saved tasks.
- Mark tasks as completed.
- Delete tasks.
- Set optional reminder dates and times.
- Store tasks in a MySQL database.
- Retrieve previously saved tasks.
- Display automatic reminder notifications while the Task Assistant window is running.

Task information is stored persistently in MySQL, meaning tasks remain available after the application closes.

### 6.2 Cybersecurity Quiz Game

Black Vault includes an interactive quiz designed to assess users' cybersecurity awareness.

**Implemented functionality:**

- 12 cybersecurity questions.
- Multiple-choice questions.
- True-or-false questions.
- Immediate answer feedback.
- Explanations for correct and incorrect answers.
- Automatic score calculation.
- Final performance feedback.
- Option to restart the quiz.

The quiz covers phishing, password safety, malware, two-factor authentication, public Wi-Fi, social engineering and other cybersecurity concepts.

### 6.3 Natural Language Processing Simulation

Black Vault uses rule-based natural-language processing through C# string matching.

The system recognises different phrases that express similar intentions and directs users to the appropriate feature.

**Example commands:**

| User Command | Application Response |
|---|---|
| I want to play a quiz | Opens the Cybersecurity Quiz |
| Test my cybersecurity knowledge | Opens the Cybersecurity Quiz |
| Show me my tasks | Opens the Task Assistant |
| Create a task | Opens the Task Assistant |
| Show my activity log | Displays recent activities |
| Remind me to study tomorrow at 3 PM | Creates a task and reminder in MySQL |

This implementation simulates NLP using predefined phrases and command recognition rather than an external AI service.

### 6.4 Natural-Language Reminder Creation

Users can create reminders directly through chatbot messages.

For example:

`Remind me to study cybersecurity tomorrow at 3 PM`

The application:

1. Identifies the reminder command.
2. Extracts the task description.
3. Recognises the date and time.
4. Validates that the reminder is scheduled for the future.
5. Saves the task and reminder in MySQL.
6. Displays confirmation in the chat.

The reminder parser supports relative dates such as **today** and **tomorrow**, along with supported 12-hour and 24-hour time formats.

When a time is omitted, the parser defaults to 09:00.

### 6.5 Activity Log

Black Vault includes an Activity Log that tracks recent chatbot actions.

**Implemented functionality:**

- Records activity descriptions.
- Automatically assigns timestamps.
- Displays the 10 most recent activities.
- Supports activity log requests through chatbot commands.

Example:

`Show my activity log`

The Activity Log records actions such as starting a quiz, opening the Task Assistant, requesting task creation and creating reminders.

Activity entries are currently stored in application memory and reset when the application closes.

## 7. Database Design

Black Vault uses a MySQL database named:

`cyberbot_db`

The database contains a `tasks` table used by the Task Assistant.

### Tasks Table

| Column | Data Type | Description |
|---|---|---|
| id | INT | Primary key |
| title | VARCHAR(150) | Task title |
| description | VARCHAR(500) | Task description |
| reminder_date | DATETIME | Optional reminder |
| is_completed | TINYINT(1) | Task completion status |
| created_at | DATETIME | Task creation timestamp |

### Database Setup Script

```sql
CREATE DATABASE IF NOT EXISTS cyberbot_db;

USE cyberbot_db;

CREATE TABLE IF NOT EXISTS tasks (
    id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(150) NOT NULL,
    description VARCHAR(500) NOT NULL,
    reminder_date DATETIME NULL,
    is_completed TINYINT(1) NOT NULL DEFAULT 0,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);
```

## 8. Installation and Setup

### Requirements

Before running the project, ensure the following are installed:

- Visual Studio 2022 with Windows desktop development support.
- A compatible .NET SDK and runtime for the project.
- MySQL Server.
- MySQL Workbench (recommended).
- MySqlConnector NuGet package.

### Step 1 — Obtain the Project

Clone the GitHub repository or download the source code as a ZIP file.

### Step 2 — Open the Solution

Open:

`Black Vault Bot.sln`

using Visual Studio 2022.

### Step 3 — Restore Dependencies

Restore the project's NuGet packages, including MySqlConnector.

### Step 4 — Configure MySQL

Start MySQL Server and connect through MySQL Workbench.

Run the database setup script in Section 7 to create the database and tasks table.

### Step 5 — Configure the Database Password

The application reads the MySQL password from the Windows environment variable:

`BLACKVAULT_DB_PASSWORD`

Set this variable to the password of the local MySQL account configured for the application.

The current development configuration uses:

- Server: `127.0.0.1`
- Port: `3306`
- Database: `cyberbot_db`
- User: `root`

These settings may need to be adjusted for another development environment.

Do not commit database passwords or other credentials to GitHub.

### Step 6 — Build and Run

1. Open the solution in Visual Studio.
2. Select **Build → Rebuild Solution**.
3. Confirm that the build completes successfully.
4. Press **F5** to run Black Vault.

**Audio note:** The voice greeting requires the audio file to be present at the path configured in the project. If running on another computer, update the greeting file path or configure the audio as a project resource.

## 9. How to Use Black Vault

### Chatbot

Launch the application and type a cybersecurity question into the chat interface.

Example:

`How can I protect myself from phishing?`

### Task Assistant

Click **My Tasks** on the dashboard to manage tasks, completion statuses and reminders.

### Cybersecurity Quiz

Click **Play Quiz** or type:

`I want to play a quiz`

Answer the questions, view explanations and review the final score.

### Natural-Language Reminders

Type a supported reminder command such as:

`Remind me to update my password tomorrow at 3 PM`

Black Vault saves the reminder to the MySQL database.

### Activity Log

Type:

`Show my activity log`

Black Vault displays the most recent recorded activities with timestamps.

## 10. Testing

The following functional tests were completed during development:

| Test | Result |
|---|---|
| Application builds without compilation errors | Passed |
| Chatbot launches successfully | Passed |
| Voice greeting plays | Passed |
| MySQL database connection | Passed |
| Task creation and retrieval | Passed |
| Automatic reminder notification | Passed |
| Cybersecurity Quiz opens | Passed |
| Quiz answer feedback and explanations | Passed |
| Quiz final score and restart | Passed |
| NLP quiz and task commands | Passed |
| Activity Log displays recent actions | Passed |
| Natural-language reminder saved to MySQL | Passed |

## 11. Project Structure

The main project includes the following components:

```text
Black Vault Bot/
│
├── Black Vault Bot.sln
│
└── WinFormsApp1/
    │
    ├── Form1.cs
    ├── Form1.Designer.cs
    ├── Program.cs
    │
    ├── TaskAssistantForm.cs
    ├── QuizForm.cs
    │
    ├── Models/
    │   └── TaskItem.cs
    │
    ├── Data/
    │   ├── DbConfig.cs
    │   └── TaskRepository.cs
    │
    └── Services/
        ├── ReminderService.cs
        ├── ReminderParser.cs
        ├── QuizService.cs
        ├── NlpService.cs
        └── ActivityLogger.cs
```

## 12. Known Limitations

The current implementation has the following limitations:

- The NLP system uses predefined rules and string matching rather than machine-learning models.
- The Activity Log is stored in memory and is not persistent between application sessions.
- Reminder notifications currently operate while the Task Assistant window is open.
- Natural-language date parsing supports a limited range of expressions.
- The voice greeting may require path configuration when running on a different computer.

These limitations provide opportunities for future development.

## 13. Future Improvements

Potential improvements include:

- More advanced natural-language understanding.
- Persistent activity logging.
- Application-wide background reminder notifications.
- Additional cybersecurity quiz categories.
- Expanded date and time recognition.
- Improved application portability and deployment.

## 14. Version Control and Releases

The project uses Git and GitHub for version control.

Development commits document changes to the chatbot, database integration, Task Assistant, quiz, NLP functionality and Activity Log.

The repository's **Releases** section contains version milestones for the project.

## 15. Video Demonstration

An unlisted YouTube demonstration provides an overview of the completed application.

The presentation demonstrates the graphical interface, chatbot functionality, voice greeting, Task Assistant, MySQL integration, Cybersecurity Quiz, NLP commands and Activity Log.

**YouTube Demonstration:** [INSERT YOUR UNLISTED YOUTUBE VIDEO LINK HERE]

## 16. Conclusion

Black Vault demonstrates the integration of C#, Windows Forms, MySQL and rule-based natural-language processing into an interactive cybersecurity awareness application.

The project combines educational chatbot functionality with task management, reminders, knowledge assessment and activity tracking.

Through its three development stages, Black Vault has evolved into a functional desktop application designed to improve users' understanding of cybersecurity and promote safer online behaviour.

---

**Project:** Black Vault — Cybersecurity Awareness Chatbot  
**Module:** PROG6221  
**Assessment:** Portfolio of Evidence (POE)
