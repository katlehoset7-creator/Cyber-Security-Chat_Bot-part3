
using System;
using System.Collections.Generic;

namespace WinFormsApp1.Services
{
    public class QuizQuestion
    {
        public string Question { get; set; } = "";
        public string[] Options { get; set; } = Array.Empty<string>();
        public int CorrectAnswerIndex { get; set; }
        public string Explanation { get; set; } = "";
    }

    public class QuizService
    {
        private readonly List<QuizQuestion> questions = new();

        private int currentQuestionIndex = 0;
        private int score = 0;

        public int Score => score;
        public int TotalQuestions => questions.Count;
        public int QuestionNumber => currentQuestionIndex + 1;
        public bool IsFinished => currentQuestionIndex >= questions.Count;

        public QuizService()
        {
            LoadQuestions();
        }

        private void LoadQuestions()
        {
            questions.Add(new QuizQuestion
            {
                Question = "What is phishing?",
                Options = new[]
                {
                    "A method of protecting passwords",
                    "A scam that tricks people into revealing information",
                    "A type of antivirus software",
                    "A secure network connection"
                },
                CorrectAnswerIndex = 1,
                Explanation = "Phishing uses deceptive messages or websites to steal sensitive information."
            });

            questions.Add(new QuizQuestion
            {
                Question = "Which password is the strongest?",
                Options = new[]
                {
                    "password123",
                    "12345678",
                    "MyName2026",
                    "T!ger#Cloud92@River"
                },
                CorrectAnswerIndex = 3,
                Explanation = "Strong passwords are long, unpredictable and unique."
            });

            questions.Add(new QuizQuestion
            {
                Question = "True or False: You should share your OTP with a bank employee.",
                Options = new[] { "True", "False" },
                CorrectAnswerIndex = 1,
                Explanation = "Never share your OTP. Legitimate bank employees should not ask you for it."
            });

            questions.Add(new QuizQuestion
            {
                Question = "What does two-factor authentication provide?",
                Options = new[]
                {
                    "An additional layer of account security",
                    "Faster internet",
                    "Free antivirus protection",
                    "Automatic password sharing"
                },
                CorrectAnswerIndex = 0,
                Explanation = "Two-factor authentication requires another verification step beyond a password."
            });

            questions.Add(new QuizQuestion
            {
                Question = "What should you do when receiving a suspicious email?",
                Options = new[]
                {
                    "Click its links immediately",
                    "Reply with your personal details",
                    "Verify the sender and report the message",
                    "Forward it to everyone"
                },
                CorrectAnswerIndex = 2,
                Explanation = "Avoid suspicious links and verify requests through trusted channels."
            });

            questions.Add(new QuizQuestion
            {
                Question = "True or False: Public Wi-Fi is always safe for online banking.",
                Options = new[] { "True", "False" },
                CorrectAnswerIndex = 1,
                Explanation = "Public Wi-Fi can be risky. Prefer a trusted connection for sensitive transactions."
            });

            questions.Add(new QuizQuestion
            {
                Question = "What is malware?",
                Options = new[]
                {
                    "Software designed to cause harm",
                    "A computer keyboard",
                    "A secure website",
                    "A password manager"
                },
                CorrectAnswerIndex = 0,
                Explanation = "Malware is malicious software that can damage systems or steal data."
            });

            questions.Add(new QuizQuestion
            {
                Question = "Why should software updates be installed?",
                Options = new[]
                {
                    "To remove all passwords",
                    "To fix security vulnerabilities",
                    "To disable antivirus protection",
                    "To share personal information"
                },
                CorrectAnswerIndex = 1,
                Explanation = "Updates often contain patches that fix security weaknesses."
            });

            questions.Add(new QuizQuestion
            {
                Question = "True or False: A padlock icon guarantees a website is trustworthy.",
                Options = new[] { "True", "False" },
                CorrectAnswerIndex = 1,
                Explanation = "HTTPS encrypts the connection, but scammers can also use HTTPS websites."
            });

            questions.Add(new QuizQuestion
            {
                Question = "What is social engineering?",
                Options = new[]
                {
                    "Building social media applications",
                    "Manipulating people into revealing information",
                    "Installing computer hardware",
                    "Creating stronger Wi-Fi signals"
                },
                CorrectAnswerIndex = 1,
                Explanation = "Social engineering exploits human trust to obtain information or access."
            });

            questions.Add(new QuizQuestion
            {
                Question = "What should you do if your account is compromised?",
                Options = new[]
                {
                    "Ignore the problem",
                    "Share your password publicly",
                    "Change your password and secure your account",
                    "Delete all your contacts"
                },
                CorrectAnswerIndex = 2,
                Explanation = "Change compromised credentials, revoke suspicious sessions and enable MFA."
            });

            questions.Add(new QuizQuestion
            {
                Question = "True or False: Reusing one password across multiple accounts is recommended.",
                Options = new[] { "True", "False" },
                CorrectAnswerIndex = 1,
                Explanation = "Unique passwords prevent one breach from exposing multiple accounts."
            });
        }

        public QuizQuestion? GetCurrentQuestion()
        {
            if (IsFinished)
                return null;

            return questions[currentQuestionIndex];
        }

        public bool SubmitAnswer(int selectedAnswerIndex)
        {
            if (IsFinished)
                throw new InvalidOperationException(
                    "The quiz has already finished.");

            var question = questions[currentQuestionIndex];

            if (selectedAnswerIndex < 0 ||
                selectedAnswerIndex >= question.Options.Length)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(selectedAnswerIndex));
            }

            bool isCorrect =
                selectedAnswerIndex == question.CorrectAnswerIndex;

            if (isCorrect)
                score++;

            currentQuestionIndex++;

            return isCorrect;
        }

        public string GetFinalFeedback()
        {
            double percentage = TotalQuestions == 0
                ? 0
                : (double)score / TotalQuestions * 100;

            if (percentage >= 80)
                return "Excellent! You have strong cybersecurity awareness.";

            if (percentage >= 50)
                return "Good effort! Keep improving your cybersecurity knowledge.";

            return "Keep learning! Review cybersecurity safety practices and try again.";
        }

        public void RestartQuiz()
        {
            currentQuestionIndex = 0;
            score = 0;
        }
    }
}
