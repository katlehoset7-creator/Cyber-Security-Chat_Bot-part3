# Cybersecurity Awareness Chatbot

## 1. Project Overview

The Cybersecurity Awareness Chatbot is a C# Windows Forms application developed to educate users about online safety and cybersecurity. It provides information about common online threats and gives users tips on how to protect their personal information.

The application uses a graphical user interface (GUI) with a dark theme and gold accents. It allows users to interact with the chatbot through a conversation area, buttons, and a text input field.

The chatbot also includes dynamic responses, basic memory, sentiment detection, and a WAV audio greeting.

## 2. Project Objectives

The main objectives of this project are to:

- Educate users about cybersecurity threats.
- Provide useful online safety tips.
- Recognise cybersecurity-related keywords.
- Respond dynamically to user messages.
- Remember information shared by the user during the conversation.
- Detect basic emotional language and respond supportively.
- Provide an interactive graphical user interface.
- Play a greeting using a WAV audio file.

## 3. Technologies Used

- **Programming language:** C#
- **Framework:** .NET 8
- **User interface:** Windows Forms (WinForms)
- **Development environment:** Microsoft Visual Studio
- **Data structures:** Dictionaries and lists
- **Event handling:** Button click events and keyboard events
- **Audio:** `System.Media.SoundPlayer`
- **File handling:** `System.IO`

## 4. Main Features

### 4.1 Graphical User Interface

The application uses a Windows Forms interface with a dark navy background, gold accents, and white text. It includes navigation buttons, a conversation display, a message input field, and controls for interacting with the chatbot.

### 4.2 Dynamic Responses

The chatbot recognises keywords and phrases in user messages and provides relevant responses. For example, a message about phishing can trigger information about phishing attacks and ways to identify suspicious messages.

A list of possible responses can be used to provide different answers for the same topic.

### 4.3 Cybersecurity Topics

The chatbot provides information about topics such as:

- **Phishing:** Identifying suspicious emails, messages, and links.
- **Password safety:** Creating strong passwords and protecting login details.
- **Scams and fraud:** Recognising common online scams.
- **Online privacy:** Protecting personal information online.

### 4.4 Memory

The chatbot uses a dictionary to store information shared by the user during the current session, such as their name or preferred cybersecurity topic.

This allows the chatbot to personalise responses and recall certain details when asked.

The memory is stored while the application is running and is not necessarily saved permanently after the application closes.

### 4.5 Sentiment Detection

The chatbot checks for words associated with emotions, such as being worried, scared, confused, frustrated, or overwhelmed.

When these words are recognised, the chatbot can provide a more supportive response and offer relevant cybersecurity advice.

This is basic keyword-based sentiment detection and does not use artificial intelligence to fully understand emotions.

### 4.6 Delegates

The application uses a C# delegate to reference the method responsible for generating chatbot responses.

This helps connect the user's message to the chatbot's response-processing method.

### 4.7 Audio Greeting

The application can play a recorded greeting using a WAV audio file. The recording welcomes the user to the Cybersecurity Awareness Chatbot.

The recording should be stored in the project's `Audio` folder and named `greeting.wav`, according to the configured file path.

### 4.8 Chat Controls

The interface includes controls for starting a conversation, sending messages, clearing the chat, selecting cybersecurity topics, and playing the greeting, depending on the features enabled in the application.

## 5. Project Structure

The project contains the following important files and folders:

```text
WinFormsApp1/
│
├── Form1.cs
│   └── Contains the chatbot logic, responses,
│       memory, event handlers, and GUI behaviour.
│
├── Form1.Designer.cs
│   └── Contains the Windows Forms designer-generated
│       controls and their initial properties.
│
├── Form1.resx
│   └── Stores form resources, where applicable.
│
├── Properties/
│   └── Resources.resx
│       └── Stores embedded resources such as the logo.
│
├── Audio/
│   └── greeting.wav
│       └── Contains the recorded audio greeting.
│
└── WinFormsApp1.csproj
    └── Contains the project configuration.
```

*Note: The exact files and folders may differ depending on the current Visual Studio project setup.*

## 6. Installation and Setup

To run the application:

1. Install Microsoft Visual Studio with the .NET desktop development workload.
2. Open the `WinFormsApp1` project or solution in Visual Studio.
3. Confirm that the project targets a compatible Windows version of .NET 8.
4. Place the greeting recording in the `Audio` folder.
5. Ensure the WAV file is configured to copy to the output directory when building the project.
6. Build the project to check for compilation errors.
7. Run the application using the Start button in Visual Studio.

## 7. How to Use the Chatbot

1. Launch the application.
2. Start a conversation using the available chat control.
3. Enter a message in the input field.
4. Press the Send button to receive a response.
5. Ask about cybersecurity topics such as phishing, passwords, scams, or online privacy.
6. Share your name if you want the chatbot to personalise responses.
7. Use the available navigation buttons to explore topics.
8. Use Clear Chat to clear the displayed conversation.
9. Play the greeting if you want to hear the recorded welcome message.

## 8. Error Handling

The application can use exception handling to manage problems such as missing audio files or errors when playing the greeting.

The audio feature should also check whether the expected WAV file exists before attempting to play it.

If the recording is missing, the application can display an error message indicating the expected file location.

## 9. Limitations

- The chatbot uses predefined responses and keyword matching.
- Sentiment detection is basic and depends on recognised words.
- Memory may only last for the current application session.
- The audio greeting requires a valid WAV file at the configured location.
- The chatbot does not provide a replacement for professional cybersecurity assistance.

## 10. Future Improvements

Possible improvements include:

- Adding more cybersecurity topics and responses.
- Improving keyword recognition and follow-up conversations.
- Saving user preferences between sessions.
- Adding more audio messages.
- Improving accessibility and keyboard navigation.
- Adding automated tests for chatbot responses and memory.
- Improving sentiment detection to recognise more expressions and context.

## 11. Conclusion

The Cybersecurity Awareness Chatbot demonstrates how C# and Windows Forms can be used to create an interactive educational application. It combines a graphical interface with dynamic responses, basic memory, sentiment detection, delegates, and audio playback to help users learn about online safety.

The project also demonstrates important programming concepts, including dictionaries, lists, methods, conditional statements, event handling, and exception handling.
