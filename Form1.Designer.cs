namespace Cybersecurity
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.chatBox = new System.Windows.Forms.RichTextBox();
            this.userInput = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.teachButton = new System.Windows.Forms.Button();
            this.endTeachingButton = new System.Windows.Forms.Button();
            this.taskTitleTextBox = new System.Windows.Forms.TextBox();
            this.taskDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.reminderPicker = new System.Windows.Forms.DateTimePicker();
            this.taskListBox = new System.Windows.Forms.ListBox();
            this.AddTaskButton = new System.Windows.Forms.Button();
            this.CompleteTaskButton = new System.Windows.Forms.Button();
            this.DeleteTaskButton = new System.Windows.Forms.Button();
            this.logListBox = new System.Windows.Forms.ListBox();
            this.ClearLogButton = new System.Windows.Forms.Button();
            this.questionLabel = new System.Windows.Forms.Label();
            this.optionA = new System.Windows.Forms.RadioButton();
            this.optionB = new System.Windows.Forms.RadioButton();
            this.optionC = new System.Windows.Forms.RadioButton();
            this.optionD = new System.Windows.Forms.RadioButton();
            this.SubmitAnswerButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chatBox
            // 
            this.chatBox.BackColor = System.Drawing.Color.White;
            this.chatBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chatBox.Location = new System.Drawing.Point(20, 20);
            this.chatBox.Name = "chatBox";
            this.chatBox.ReadOnly = true;
            this.chatBox.Size = new System.Drawing.Size(480, 250);
            this.chatBox.TabIndex = 0;
            this.chatBox.Text = "";
            // 
            // userInput
            // 
            this.userInput.Location = new System.Drawing.Point(20, 280);
            this.userInput.Name = "userInput";
            this.userInput.Size = new System.Drawing.Size(380, 20);
            this.userInput.TabIndex = 1;
            this.userInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.userInput_KeyDown);
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(410, 280);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(90, 25);
            this.sendButton.TabIndex = 2;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // teachButton
            // 
            this.teachButton.BackColor = System.Drawing.Color.LightGreen;
            this.teachButton.Location = new System.Drawing.Point(20, 315);
            this.teachButton.Name = "teachButton";
            this.teachButton.Size = new System.Drawing.Size(100, 30);
            this.teachButton.TabIndex = 3;
            this.teachButton.Text = "Teach Bot";
            this.teachButton.UseVisualStyleBackColor = false;
            this.teachButton.Click += new System.EventHandler(this.teachButton_Click);
            // 
            // endTeachingButton
            // 
            this.endTeachingButton.BackColor = System.Drawing.Color.LightCoral;
            this.endTeachingButton.Location = new System.Drawing.Point(130, 315);
            this.endTeachingButton.Name = "endTeachingButton";
            this.endTeachingButton.Size = new System.Drawing.Size(100, 30);
            this.endTeachingButton.TabIndex = 4;
            this.endTeachingButton.Text = "End Teaching";
            this.endTeachingButton.UseVisualStyleBackColor = false;
            this.endTeachingButton.Click += new System.EventHandler(this.endTeachingButton_Click);
            // 
            // taskTitleTextBox
            // 
            this.taskTitleTextBox.ForeColor = System.Drawing.Color.Gray;
            this.taskTitleTextBox.Location = new System.Drawing.Point(520, 20);
            this.taskTitleTextBox.Name = "taskTitleTextBox";
            this.taskTitleTextBox.Size = new System.Drawing.Size(348, 20);
            this.taskTitleTextBox.TabIndex = 5;
            this.taskTitleTextBox.Text = "Task Title";
            // 
            // taskDescriptionTextBox
            // 
            this.taskDescriptionTextBox.ForeColor = System.Drawing.Color.Gray;
            this.taskDescriptionTextBox.Location = new System.Drawing.Point(520, 50);
            this.taskDescriptionTextBox.Name = "taskDescriptionTextBox";
            this.taskDescriptionTextBox.Size = new System.Drawing.Size(348, 20);
            this.taskDescriptionTextBox.TabIndex = 6;
            this.taskDescriptionTextBox.Text = "Task Description";
            // 
            // reminderPicker
            // 
            this.reminderPicker.Location = new System.Drawing.Point(520, 80);
            this.reminderPicker.Name = "reminderPicker";
            this.reminderPicker.Size = new System.Drawing.Size(348, 20);
            this.reminderPicker.TabIndex = 7;
            // 
            // taskListBox
            // 
            this.taskListBox.Location = new System.Drawing.Point(520, 110);
            this.taskListBox.Name = "taskListBox";
            this.taskListBox.Size = new System.Drawing.Size(348, 95);
            this.taskListBox.TabIndex = 8;
            // 
            // AddTaskButton
            // 
            this.AddTaskButton.BackColor = System.Drawing.SystemColors.Control;
            this.AddTaskButton.Location = new System.Drawing.Point(520, 220);
            this.AddTaskButton.Name = "AddTaskButton";
            this.AddTaskButton.Size = new System.Drawing.Size(60, 25);
            this.AddTaskButton.TabIndex = 9;
            this.AddTaskButton.Text = "Add";
            this.AddTaskButton.UseVisualStyleBackColor = false;
            this.AddTaskButton.Click += new System.EventHandler(this.AddTaskButton_Click);
            // 
            // CompleteTaskButton
            // 
            this.CompleteTaskButton.BackColor = System.Drawing.SystemColors.Control;
            this.CompleteTaskButton.Location = new System.Drawing.Point(609, 220);
            this.CompleteTaskButton.Name = "CompleteTaskButton";
            this.CompleteTaskButton.Size = new System.Drawing.Size(60, 25);
            this.CompleteTaskButton.TabIndex = 10;
            this.CompleteTaskButton.Text = "Done";
            this.CompleteTaskButton.UseVisualStyleBackColor = false;
            this.CompleteTaskButton.Click += new System.EventHandler(this.CompleteTaskButton_Click);
            // 
            // DeleteTaskButton
            // 
            this.DeleteTaskButton.Location = new System.Drawing.Point(703, 220);
            this.DeleteTaskButton.Name = "DeleteTaskButton";
            this.DeleteTaskButton.Size = new System.Drawing.Size(60, 25);
            this.DeleteTaskButton.TabIndex = 11;
            this.DeleteTaskButton.Text = "Delete";
            this.DeleteTaskButton.UseVisualStyleBackColor = true;
            this.DeleteTaskButton.Click += new System.EventHandler(this.DeleteTaskButton_Click);
            // 
            // logListBox
            // 
            this.logListBox.Location = new System.Drawing.Point(520, 260);
            this.logListBox.Name = "logListBox";
            this.logListBox.Size = new System.Drawing.Size(348, 95);
            this.logListBox.TabIndex = 12;
            // 
            // ClearLogButton
            // 
            this.ClearLogButton.Location = new System.Drawing.Point(520, 370);
            this.ClearLogButton.Name = "ClearLogButton";
            this.ClearLogButton.Size = new System.Drawing.Size(348, 25);
            this.ClearLogButton.TabIndex = 13;
            this.ClearLogButton.Text = "Clear Log";
            this.ClearLogButton.UseVisualStyleBackColor = true;
            this.ClearLogButton.Click += new System.EventHandler(this.ClearLogButton_Click);
            // 
            // questionLabel
            // 
            this.questionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.questionLabel.Location = new System.Drawing.Point(20, 360);
            this.questionLabel.Name = "questionLabel";
            this.questionLabel.Size = new System.Drawing.Size(480, 25);
            this.questionLabel.TabIndex = 14;
            this.questionLabel.Text = "Question appears here during quiz";
            // 
            // optionA
            // 
            this.optionA.Location = new System.Drawing.Point(20, 390);
            this.optionA.Name = "optionA";
            this.optionA.Size = new System.Drawing.Size(200, 20);
            this.optionA.TabIndex = 15;
            this.optionA.UseVisualStyleBackColor = true;
            // 
            // optionB
            // 
            this.optionB.Location = new System.Drawing.Point(20, 410);
            this.optionB.Name = "optionB";
            this.optionB.Size = new System.Drawing.Size(200, 20);
            this.optionB.TabIndex = 16;
            this.optionB.UseVisualStyleBackColor = true;
            // 
            // optionC
            // 
            this.optionC.Location = new System.Drawing.Point(250, 390);
            this.optionC.Name = "optionC";
            this.optionC.Size = new System.Drawing.Size(200, 20);
            this.optionC.TabIndex = 17;
            this.optionC.UseVisualStyleBackColor = true;
            // 
            // optionD
            // 
            this.optionD.Location = new System.Drawing.Point(250, 410);
            this.optionD.Name = "optionD";
            this.optionD.Size = new System.Drawing.Size(200, 20);
            this.optionD.TabIndex = 18;
            this.optionD.UseVisualStyleBackColor = true;
            // 
            // SubmitAnswerButton
            // 
            this.SubmitAnswerButton.Location = new System.Drawing.Point(20, 440);
            this.SubmitAnswerButton.Name = "SubmitAnswerButton";
            this.SubmitAnswerButton.Size = new System.Drawing.Size(100, 30);
            this.SubmitAnswerButton.TabIndex = 19;
            this.SubmitAnswerButton.Text = "Submit Answer";
            this.SubmitAnswerButton.UseVisualStyleBackColor = true;
            this.SubmitAnswerButton.Click += new System.EventHandler(this.SubmitAnswerButton_Click);
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(880, 500);
            this.Controls.Add(this.chatBox);
            this.Controls.Add(this.userInput);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.teachButton);
            this.Controls.Add(this.endTeachingButton);
            this.Controls.Add(this.taskTitleTextBox);
            this.Controls.Add(this.taskDescriptionTextBox);
            this.Controls.Add(this.reminderPicker);
            this.Controls.Add(this.taskListBox);
            this.Controls.Add(this.AddTaskButton);
            this.Controls.Add(this.CompleteTaskButton);
            this.Controls.Add(this.DeleteTaskButton);
            this.Controls.Add(this.logListBox);
            this.Controls.Add(this.ClearLogButton);
            this.Controls.Add(this.questionLabel);
            this.Controls.Add(this.optionA);
            this.Controls.Add(this.optionB);
            this.Controls.Add(this.optionC);
            this.Controls.Add(this.optionD);
            this.Controls.Add(this.SubmitAnswerButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cybersecurity Awareness Chatbot";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.RichTextBox chatBox;
        private System.Windows.Forms.TextBox userInput;
        private System.Windows.Forms.Button sendButton;

        // Teaching buttons
        private System.Windows.Forms.Button teachButton;
        private System.Windows.Forms.Button endTeachingButton;

        private System.Windows.Forms.TextBox taskTitleTextBox;
        private System.Windows.Forms.TextBox taskDescriptionTextBox;
        private System.Windows.Forms.DateTimePicker reminderPicker;
        private System.Windows.Forms.ListBox taskListBox;
        private System.Windows.Forms.Button AddTaskButton;
        private System.Windows.Forms.Button CompleteTaskButton;
        private System.Windows.Forms.Button DeleteTaskButton;

        private System.Windows.Forms.ListBox logListBox;
        private System.Windows.Forms.Button ClearLogButton;

        private System.Windows.Forms.Label questionLabel;
        private System.Windows.Forms.RadioButton optionA;
        private System.Windows.Forms.RadioButton optionB;
        private System.Windows.Forms.RadioButton optionC;
        private System.Windows.Forms.RadioButton optionD;
        private System.Windows.Forms.Button SubmitAnswerButton;
    }
}