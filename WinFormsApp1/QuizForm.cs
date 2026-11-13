
using System;
using System.Drawing;
using System.Windows.Forms;
using WinFormsApp1.Services;

namespace WinFormsApp1
{
    public partial class QuizForm : Form
    {
        private readonly QuizService quizService = new QuizService();

        private Label lblProgress = null!;
        private Label lblQuestion = null!;
        private Label lblScore = null!;
        private Label lblFeedback = null!;

        private readonly RadioButton[] answerButtons = new RadioButton[4];

        private Button btnSubmit = null!;
        private Button btnNext = null!;
        private Button btnRestart = null!;

        private bool answerSubmitted = false;

        public QuizForm()
        {
            InitializeComponent();
            BuildQuizInterface();
            DisplayQuestion();
        }

        private void BuildQuizInterface()
        {
            Text = "Black Vault - Cybersecurity Quiz";
            Size = new Size(850, 650);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(12, 22, 40);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Controls.Clear();

            Label heading = new Label
            {
                Text = "BLACK VAULT | CYBERSECURITY QUIZ",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.Gold,
                Location = new Point(35, 25),
                Size = new Size(780, 45)
            };
            Controls.Add(heading);

            lblProgress = new Label
            {
                Location = new Point(40, 90),
                Size = new Size(350, 30),
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            Controls.Add(lblProgress);

            lblScore = new Label
            {
                Location = new Point(630, 90),
                Size = new Size(170, 30),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.Gold,
                TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lblScore);

            lblQuestion = new Label
            {
                Location = new Point(40, 145),
                Size = new Size(750, 75),
                Font = new Font("Segoe UI", 15, FontStyle.Bold)
            };
            Controls.Add(lblQuestion);

            for (int i = 0; i < answerButtons.Length; i++)
            {
                RadioButton radio = new RadioButton
                {
                    Location = new Point(55, 240 + (i * 55)),
                    Size = new Size(730, 45),
                    Font = new Font("Segoe UI", 12),
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(25, 40, 65),
                    Padding = new Padding(12),
                    Tag = i
                };

                answerButtons[i] = radio;
                Controls.Add(radio);
            }

            lblFeedback = new Label
            {
                Location = new Point(40, 465),
                Size = new Size(750, 65),
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.Gold
            };
            Controls.Add(lblFeedback);

            btnSubmit = CreateButton("Submit Answer", 40, 545, 170);
            btnSubmit.Click += SubmitAnswer;
            Controls.Add(btnSubmit);

            btnNext = CreateButton("Next Question", 230, 545, 170);
            btnNext.Click += NextQuestion;
            btnNext.Visible = false;
            Controls.Add(btnNext);

            btnRestart = CreateButton("Restart Quiz", 420, 545, 170);
            btnRestart.Click += RestartQuiz;
            btnRestart.Visible = false;
            Controls.Add(btnRestart);
        }

        private Button CreateButton(string text, int x, int y, int width)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, 42),
                BackColor = Color.Gold,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        private void DisplayQuestion()
        {
            QuizQuestion? question = quizService.GetCurrentQuestion();

            if (question == null)
            {
                ShowFinalResults();
                return;
            }

            answerSubmitted = false;

            lblProgress.Text =
                $"Question {quizService.QuestionNumber} of {quizService.TotalQuestions}";

            lblScore.Text = $"Score: {quizService.Score}";

            lblQuestion.Text = question.Question;
            lblFeedback.Text = "";

            for (int i = 0; i < answerButtons.Length; i++)
            {
                answerButtons[i].Checked = false;
                answerButtons[i].Enabled = true;

                if (i < question.Options.Length)
                {
                    answerButtons[i].Text = question.Options[i];
                    answerButtons[i].Visible = true;
                }
                else
                {
                    answerButtons[i].Visible = false;
                }
            }

            btnSubmit.Visible = true;
            btnNext.Visible = false;
            btnRestart.Visible = false;
        }

        private void SubmitAnswer(object? sender, EventArgs e)
        {
            if (answerSubmitted)
                return;

            int selectedIndex = -1;

            for (int i = 0; i < answerButtons.Length; i++)
            {
                if (answerButtons[i].Visible && answerButtons[i].Checked)
                {
                    selectedIndex = i;
                    break;
                }
            }

            if (selectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select an answer first.",
                    "Black Vault",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            QuizQuestion? question = quizService.GetCurrentQuestion();

            if (question == null)
                return;

            bool correct = quizService.SubmitAnswer(selectedIndex);

            answerSubmitted = true;

            string correctAnswer = question.Options[question.CorrectAnswerIndex];

            lblFeedback.Text = correct
                ? $"Correct! {question.Explanation}"
                : $"Incorrect. Correct answer: {correctAnswer}. {question.Explanation}";

            lblFeedback.ForeColor = correct
                ? Color.LightGreen
                : Color.Orange;

            lblScore.Text = $"Score: {quizService.Score}";

            foreach (RadioButton radio in answerButtons)
                radio.Enabled = false;

            btnSubmit.Visible = false;
            btnNext.Visible = true;

            btnNext.Text = quizService.IsFinished
                ? "View Results"
                : "Next Question";
        }

        private void NextQuestion(object? sender, EventArgs e)
        {
            DisplayQuestion();
        }

        private void ShowFinalResults()
        {
            lblProgress.Text = "Quiz Completed!";
            lblQuestion.Text =
                $"Final Score: {quizService.Score} / {quizService.TotalQuestions}";

            lblScore.Text = "";

            lblFeedback.Text = quizService.GetFinalFeedback();
            lblFeedback.ForeColor = Color.Gold;

            foreach (RadioButton radio in answerButtons)
                radio.Visible = false;

            btnSubmit.Visible = false;
            btnNext.Visible = false;
            btnRestart.Visible = true;
        }

        private void RestartQuiz(object? sender, EventArgs e)
        {
            quizService.RestartQuiz();
            DisplayQuestion();
        }
    }
}
