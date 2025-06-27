using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Linq;

namespace Cybersecurity
{
    public partial class Form1 : Form
    {
        private readonly string connStr = "server=localhost;user=Nature;password=Bevorpodtia16@;database=cybersecurity_bot;";
        private int currentUserId;
        private string userName;

        private bool learningMode = false;
        private string lastUnknownQuestion = "";
        private readonly Dictionary<string, string> learnedQA = new Dictionary<string, string>();

        private bool quizActive = false;
        private int quizIndex = 0;
        private int quizScore = 0;
        private readonly List<Tuple<string, string[], string>> quizQuestions = new List<Tuple<string, string[], string>>
        {
            Tuple.Create("What is phishing?", new[] { "A fishing trip", "Email scam", "Strong password", "Safe software" }, "Email scam"),
            Tuple.Create("What should you do if an email asks for your password?", new[] { "Reply", "Ignore", "Report as phishing", "Click link" }, "Report as phishing"),
            Tuple.Create("True or False: You should reuse passwords.", new[] { "True", "False" }, "False"),
            Tuple.Create("Which is the safest password?", new[] { "123456", "password", "A random mix of letters, numbers, and symbols", "Your birthdate" }, "A random mix of letters, numbers, and symbols"),
            Tuple.Create("What does 2FA stand for?", new[] { "Two-factor authentication", "Two-form access", "Second file access", "Two-face authentication" }, "Two-factor authentication"),
            Tuple.Create("Is public Wi‑Fi safe for banking?", new[] { "Yes", "No" }, "No"),
            Tuple.Create("True or False: Social engineering is tricking people to give sensitive info.", new[] { "True", "False" }, "True"),
            Tuple.Create("What should you do when you get a suspicious link?", new[] { "Click it", "Delete it", "Report it", "Ignore it" }, "Report it"),
            Tuple.Create("Which software helps protect against malware?", new[] { "Antivirus", "Browser", "Media player", "Text editor" }, "Antivirus"),
            Tuple.Create("True or False: It's safe to download attachments from unknown senders.", new[] { "True", "False" }, "False")
        };

        private readonly List<string> activityLog = new List<string>();
        private Timer reminderTimer;

        // Enhanced keyword dictionaries for NLP-like functionality
        private readonly Dictionary<string, List<string>> keywordCategories = new Dictionary<string, List<string>>
        {
            ["task"] = new List<string> { "task", "todo", "reminder", "schedule", "appointment", "deadline", "due", "complete", "finish" },
            ["quiz"] = new List<string> { "quiz", "test", "question", "exam", "assessment", "challenge", "evaluate", "knowledge" },
            ["security"] = new List<string> { "password", "malware", "virus", "security", "protection", "hack", "breach", "firewall", "encryption" },
            ["help"] = new List<string> { "help", "assist", "support", "guide", "explain", "how", "what", "why", "info", "information" },
            ["log"] = new List<string> { "log", "activity", "history", "activities", "show", "display", "view", "list" },
            ["greeting"] = new List<string> { "hello", "hi", "hey", "greetings", "good morning", "good afternoon", "good evening" },
            ["add"] = new List<string> { "add", "create", "new", "make", "insert", "register", "set up", "establish" },
            ["delete"] = new List<string> { "delete", "remove", "erase", "clear", "cancel", "eliminate", "drop" },
            ["2fa"] = new List<string> { "2fa", "two factor", "two-factor", "authentication", "mfa", "multi-factor" },
            ["privacy"] = new List<string> { "privacy", "settings", "configuration", "setup", "preferences", "options" }
        };

        // Teaching state management
        private enum TeachState
        {
            None,
            WaitingForQuestion,
            WaitingForAnswer
        }

        private TeachState teachState = TeachState.None;
        private string teachingQuestion = "";

        public Form1()
        {
            InitializeComponent();
            InitializeBot();
        }

        private void InitializeBot()
        {
            PromptForUserName();
            LoadLearnedQA();

            reminderTimer = new Timer { Interval = 60000 };
            reminderTimer.Tick += ReminderTimer_Tick;
            reminderTimer.Start();
        }

        private void PromptForUserName()
        {
            using (Form prompt = new Form())
            {
                prompt.Width = 300;
                prompt.Height = 150;
                prompt.Text = "Welcome";

                Label textLabel = new Label() { Left = 20, Top = 20, Text = "Enter your name:" };
                TextBox inputBox = new TextBox() { Left = 20, Top = 50, Width = 240 };
                Button confirmation = new Button() { Text = "OK", Left = 180, Width = 80, Top = 80, DialogResult = DialogResult.OK };

                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(inputBox);
                prompt.Controls.Add(confirmation);
                prompt.AcceptButton = confirmation;

                if (prompt.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(inputBox.Text))
                    userName = inputBox.Text.Trim();
                else
                    Environment.Exit(0);
            }

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT id FROM users WHERE username=@u", conn);
                cmd.Parameters.AddWithValue("@u", userName);
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    currentUserId = Convert.ToInt32(result);
                    AppendChat("Bot", $"Welcome back, {userName}!", Color.LimeGreen);
                }
                else
                {
                    var ins = new MySqlCommand("INSERT INTO users(username) VALUES(@u); SELECT LAST_INSERT_ID();", conn);
                    ins.Parameters.AddWithValue("@u", userName);
                    currentUserId = Convert.ToInt32(ins.ExecuteScalar());
                    AppendChat("Bot", $"Hello {userName}, welcome!", Color.LimeGreen);
                }
            }
        }
        //The method that call the learn answers and questions from the databse
        private void LoadLearnedQA()
        {
            learnedQA.Clear();
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT question, answer FROM learned_qa WHERE user_id=@uid", conn);
                cmd.Parameters.AddWithValue("@uid", currentUserId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        learnedQA[reader.GetString(0).ToLower()] = reader.GetString(1);
                }
            }
        }

        // Enhanced NLP-like keyword detection using regex and fuzzy matching
        private List<string> DetectKeywords(string input)
        {
            List<string> detectedCategories = new List<string>();
            string normalizedInput = input.ToLower().Trim();

            // Remove punctuation and normalize spaces
            normalizedInput = Regex.Replace(normalizedInput, @"[^\w\s]", " ");
            normalizedInput = Regex.Replace(normalizedInput, @"\s+", " ");

            foreach (var category in keywordCategories)
            {
                foreach (var keyword in category.Value)
                {
                    // Direct match
                    if (normalizedInput.Contains(keyword))
                    {
                        if (!detectedCategories.Contains(category.Key))
                            detectedCategories.Add(category.Key);
                        continue;
                    }

                    // Fuzzy matching for common misspellings and variations
                    if (FuzzyMatch(normalizedInput, keyword))
                    {
                        if (!detectedCategories.Contains(category.Key))
                            detectedCategories.Add(category.Key);
                    }
                }
            }

            return detectedCategories;
        }

        // Simple fuzzy matching for typos and variations
        private bool FuzzyMatch(string input, string keyword)
        {
            // Check for partial matches and common variations
            if (keyword.Length <= 3) return false; // Skip short keywords for fuzzy matching

            // Check if keyword appears with small variations
            string[] words = input.Split(' ');
            foreach (string word in words)
            {
                if (word.Length >= keyword.Length - 1 && word.Length <= keyword.Length + 1)
                {
                    int differences = 0;
                    int minLength = Math.Min(word.Length, keyword.Length);
                    
                    for (int i = 0; i < minLength; i++)
                    {
                        if (word[i] != keyword[i])
                            differences++;
                    }
                    
                    differences += Math.Abs(word.Length - keyword.Length);
                    
                    if (differences <= 1) // Allow 1 character difference
                        return true;
                }
            }
            
            return false;
        }

        // Extract task details from natural language input
        private (string title, string description) ExtractTaskDetails(string input)
        {
            string title = "";
            string description = "";

            // Remove common task-related words to get the actual task
            string cleanInput = Regex.Replace(input, @"\b(add|create|new|task|todo|reminder|to|a|an|the)\b", "", RegexOptions.IgnoreCase);
            cleanInput = cleanInput.Trim();

            // Look for specific patterns
            var match = Regex.Match(input, @"(?:add|create|new|make)\s+(?:a\s+)?(?:task|reminder|todo)?\s*(?:to|for)?\s*(.+)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                string taskContent = match.Groups[1].Value.Trim();
                
                // Split on common separators for title and description
                if (taskContent.Contains(" - "))
                {
                    var parts = taskContent.Split(new[] { " - " }, 2, StringSplitOptions.None);
                    title = parts[0].Trim();
                    description = parts.Length > 1 ? parts[1].Trim() : "";
                }
                else if (taskContent.Contains(": "))
                {
                    var parts = taskContent.Split(new[] { ": " }, 2, StringSplitOptions.None);
                    title = parts[0].Trim();
                    description = parts.Length > 1 ? parts[1].Trim() : "";
                }
                else
                {
                    title = taskContent;
                    description = "Auto-generated from: " + input;
                }
            }
            else
            {
                title = cleanInput.Length > 0 ? cleanInput : "New Task";
                description = "Created from: " + input;
            }

            return (title, description);
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            string input = userInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            AppendChat(userName, input, Color.Blue);
            userInput.Clear();

            string response = ProcessInput(input);
            AppendChat("Bot", response, Color.Lime);
        }

        private string ProcessInput(string rawInput)
        {
            string input = rawInput.ToLower();

            // Handle teaching mode interactions
            if (teachState == TeachState.WaitingForQuestion)
            {
                teachingQuestion = input;
                teachState = TeachState.WaitingForAnswer;
                return $"If user says \"{rawInput}\", how must I respond?";
            }

            if (teachState == TeachState.WaitingForAnswer)
            {
                // Save the learned Q&A
                learnedQA[teachingQuestion] = rawInput;
                SaveLearnedQAToDb(teachingQuestion, rawInput);

                // Reset for next question but stay in teaching mode
                teachingQuestion = "";
                teachState = TeachState.WaitingForQuestion;
                return "Understood! Please enter another question to teach me, or press 'End Teaching' to finish.";
            }

            // Handle quiz mode
            if (quizActive)
            {
                return ProcessQuizInput(input);
            }

            // Check learned responses first
            if (learnedQA.ContainsKey(input))
                return learnedQA[input];

            // Enhanced NLP processing
            List<string> detectedKeywords = DetectKeywords(rawInput);
            
            // Process based on detected keywords
            if (detectedKeywords.Contains("log"))
            {
                ShowActivityLog();
                return "Here's your recent activity log displayed in the log panel.";
            }

            if (detectedKeywords.Contains("quiz"))
            {
                quizActive = true;
                quizIndex = 0;
                quizScore = 0;
                ShowQuizQuestion();
                return "Starting cybersecurity quiz! Answer the questions below.";
            }

            if (detectedKeywords.Contains("add") && detectedKeywords.Contains("task"))
            {
                var taskDetails = ExtractTaskDetails(rawInput);
                AddTaskFromChat(taskDetails.title, taskDetails.description);
                return $"Task added: '{taskDetails.title}' - {taskDetails.description}";
            }

            if (detectedKeywords.Contains("greeting"))
            {
                return "Hello! I'm here to help with cybersecurity questions. Ask me about passwords, malware, or type 'quiz' to test your knowledge!";
            }

            if (detectedKeywords.Contains("help"))
            {
                return "I can help with cybersecurity topics like passwords, malware prevention, and more. You can also:\n" +
                       "• Take a quiz by saying 'quiz' or 'start quiz'\n" +
                       "• Add tasks like 'Add task to enable 2FA'\n" +
                       "• View your activity by saying 'show log' or 'show activity'\n" +
                       "• Teach me new responses with the 'Teach Bot' button!";
            }

            // Cybersecurity-specific responses
            if (detectedKeywords.Contains("security"))
            {
                if (input.Contains("password"))
                    return "Strong passwords should be at least 12 characters long, include a mix of uppercase, lowercase, numbers, and symbols. Never reuse passwords across multiple accounts!";
                  
                if (input.Contains("malware") || input.Contains("virus"))
                    return "Malware is malicious software designed to harm your computer. Protect yourself with antivirus software, regular updates, and avoid downloading from untrusted sources!";
               
                return "I can help with various cybersecurity topics. What specific security question do you have?";
            }

            return "I don't know about that yet. You can teach me by clicking the 'Teach Bot' button, or try asking about cybersecurity topics, tasks, or quizzes!";
        }

        private void AddTaskFromChat(string title, string description)
        {
            string taskText = $"{title} - {description} (Added via chat on: {DateTime.Now.ToShortDateString()})";
            taskListBox.Items.Add(taskText);
            activityLog.Add($"{DateTime.Now:g}: Added task via chat: {title}");
        }

        private void SaveLearnedQAToDb(string question, string answer)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var cmd = new MySqlCommand("INSERT INTO learned_qa(user_id, question, answer) VALUES(@uid, @q, @a)", conn);
                cmd.Parameters.AddWithValue("@uid", currentUserId);
                cmd.Parameters.AddWithValue("@q", question);
                cmd.Parameters.AddWithValue("@a", answer);
                cmd.ExecuteNonQuery();
            }
        }

        private void ShowQuizQuestion()
        {
            if (quizIndex >= quizQuestions.Count)
            {
                quizActive = false;
                return;
            }

            var q = quizQuestions[quizIndex];
            questionLabel.Text = q.Item1;

            optionA.Text = q.Item2.Length > 0 ? q.Item2[0] : "";
            optionB.Text = q.Item2.Length > 1 ? q.Item2[1] : "";
            optionC.Text = q.Item2.Length > 2 ? q.Item2[2] : "";
            optionD.Text = q.Item2.Length > 3 ? q.Item2[3] : "";

            optionA.Visible = q.Item2.Length > 0;
            optionB.Visible = q.Item2.Length > 1;
            optionC.Visible = q.Item2.Length > 2;
            optionD.Visible = q.Item2.Length > 3;

            // Clear previous selections
            optionA.Checked = false;
            optionB.Checked = false;
            optionC.Checked = false;
            optionD.Checked = false;
        }
        //Method that processes the quiz
        private string ProcessQuizInput(string input)
        {
            string correct = quizQuestions[quizIndex].Item3.ToLower();
            string feedback = input == correct ? "Correct!" : $"Incorrect. The correct answer was '{quizQuestions[quizIndex].Item3}'.";

            if (input == correct) quizScore++;

            quizIndex++;
            if (quizIndex < quizQuestions.Count)
            {
                ShowQuizQuestion();
                return $"{feedback} Next question loaded below.";
            }

            quizActive = false;
            activityLog.Add($"{DateTime.Now:g}: Completed quiz with score: {quizScore}/{quizQuestions.Count}");

            string finalMessage = $"{feedback} Quiz finished! Your score: {quizScore}/{quizQuestions.Count}. ";

            if (quizScore < 5)
            {
                finalMessage += "You might want to study the basics of cybersecurity more to strengthen your foundation.";
            }
            else if (quizScore <= 7)
            {
                finalMessage += "Good job! Keep learning and practicing to become even better at cybersecurity.";
            }
            else
            {
                finalMessage += "Excellent! You are a cybersecurity guru now — there is nothing that will defeat you in the online world.";
            }

            return finalMessage;
        }

        //The method for the submit button in the quiz section.
        private void SubmitAnswerButton_Click(object sender, EventArgs e)
        {
            if (!quizActive)
            {
                MessageBox.Show("Type 'quiz' in chat to start a quiz.");
                return;
            }

            string selected = null;
            if (optionA.Checked) selected = optionA.Text.ToLower();
            else if (optionB.Checked) selected = optionB.Text.ToLower();
            else if (optionC.Checked) selected = optionC.Text.ToLower();
            else if (optionD.Checked) selected = optionD.Text.ToLower();

            if (selected == null)
            {
                MessageBox.Show("Please select an answer.");
                return;
            }

            string response = ProcessQuizInput(selected);
            AppendChat("Bot", response, Color.Lime);
        }

        // Teaching button event handlers
        private void teachButton_Click(object sender, EventArgs e)
        {
            if (teachState != TeachState.None)
            {
                AppendChat("Bot", "You are already in teaching mode. Please finish the current session or press 'End Teaching'.", Color.Orange);
                return;
            }

            teachState = TeachState.WaitingForQuestion;
            AppendChat("Bot", "Teaching mode activated! Please enter a question you want to teach me.", Color.LimeGreen);
            userInput.Focus();
        }
        //ends the teaching session
        private void endTeachingButton_Click(object sender, EventArgs e)
        {
            if (teachState == TeachState.None)
            {
                AppendChat("Bot", "You are not currently in teaching mode.", Color.Orange);
                return;
            }

            teachState = TeachState.None;
            teachingQuestion = "";
            AppendChat("Bot", "Teaching mode ended. Thank you for teaching me!", Color.LimeGreen);
            userInput.Focus();
        }

        private void userInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                sendButton_Click(sender, e);
            }
        }

        private void AppendChat(string sender, string message, Color color)
        {
            chatBox.SelectionColor = color;
            chatBox.AppendText($"{sender}: {message}\n");
            chatBox.ScrollToCaret();
            SaveConversation(sender == "Bot" ? "bot" : "user", message);
        }
        //Saves the conversation in the mySQL database.
        private void SaveConversation(string sender, string message)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var cmd = new MySqlCommand("INSERT INTO conversation(user_id,timestamp,sender,message) VALUES(@u,NOW(),@s,@m)", conn);
                cmd.Parameters.AddWithValue("@u", currentUserId);
                cmd.Parameters.AddWithValue("@s", sender);
                cmd.Parameters.AddWithValue("@m", message);
                cmd.ExecuteNonQuery();
            }
        }
        //The button that adds the task
        private void AddTaskButton_Click(object sender, EventArgs e)
        {
            string title = taskTitleTextBox.Text.Trim();
            string desc = taskDescriptionTextBox.Text.Trim();
            DateTime rem = reminderPicker.Value.Date;

            if (string.IsNullOrWhiteSpace(title) || title == "Task Title")
            {
                MessageBox.Show("Please enter a task title.");
                return;
            }

            string taskText = $"{title} - {desc} (Remind on: {rem.ToShortDateString()})";
            taskListBox.Items.Add(taskText);
            activityLog.Add($"{DateTime.Now:g}: Added task: {title}");

            taskTitleTextBox.Text = "Task Title";
            taskTitleTextBox.ForeColor = Color.Gray;
            taskDescriptionTextBox.Text = "Task Description";
            taskDescriptionTextBox.ForeColor = Color.Gray;
        }
        //The method of the button that marks the task as complete
        private void CompleteTaskButton_Click(object sender, EventArgs e)
        {
            if (taskListBox.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a task to complete.");
                return;
            }
            string task = taskListBox.SelectedItem.ToString();
            taskListBox.Items.RemoveAt(taskListBox.SelectedIndex);
            activityLog.Add($"{DateTime.Now:g}: Completed task: {task}");
        }
        //The Method of the button to delete the task
        private void DeleteTaskButton_Click(object sender, EventArgs e)
        {
            if (taskListBox.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a task to delete.");
                return;
            }
            string task = taskListBox.SelectedItem.ToString();
            taskListBox.Items.RemoveAt(taskListBox.SelectedIndex);
            activityLog.Add($"{DateTime.Now:g}: Deleted task: {task}");
        }

        private void ReminderTimer_Tick(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            for (int i = 0; i < taskListBox.Items.Count; i++)
            {
                string item = taskListBox.Items[i].ToString();
                int pos = item.LastIndexOf("Remind on:");
                if (pos < 0) continue;

                string ds = item.Substring(pos + 10).Trim(' ', ')');
                if (DateTime.TryParse(ds, out DateTime rd) && rd == today)
                {
                    MessageBox.Show($"Reminder: {item}", "Task Reminder");
                    activityLog.Add($"{DateTime.Now:g}: Reminder triggered for: {item}");
                }
            }
        }

        private void ShowActivityLog()
        {
            logListBox.Items.Clear();

            // Add recent activity log entries
            activityLog.Add($"{DateTime.Now:g}: Viewed activity log");
            
            for (int i = Math.Max(0, activityLog.Count - 15); i < activityLog.Count; i++)
                logListBox.Items.Add(activityLog[i]);

            // Add recent conversation history from database
            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    var cmd = new MySqlCommand("SELECT timestamp, sender, message FROM conversation WHERE user_id=@u ORDER BY timestamp DESC LIMIT 10", conn);
                    cmd.Parameters.AddWithValue("@u", currentUserId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        var conversations = new List<string>();
                        while (reader.Read())
                        {
                            conversations.Add($"{reader.GetDateTime(0):g} - {reader.GetString(1)}: {reader.GetString(2)}");
                        }
                        
                        // Add conversations to the beginning of the log
                        for (int i = conversations.Count - 1; i >= 0; i--)
                        {
                            logListBox.Items.Insert(0, conversations[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logListBox.Items.Add($"Error loading conversation history: {ex.Message}");
            }

            // Scroll to the bottom to show most recent activity
            if (logListBox.Items.Count > 0)
                logListBox.TopIndex = logListBox.Items.Count - 1;
        }
        //The method to clear the log
        private void ClearLogButton_Click(object sender, EventArgs e)
        {
            activityLog.Clear();
            logListBox.Items.Clear();
            activityLog.Add($"{DateTime.Now:g}: Activity log cleared");
        }
    }
}