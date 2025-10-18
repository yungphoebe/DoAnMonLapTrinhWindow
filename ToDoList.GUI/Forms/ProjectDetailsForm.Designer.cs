namespace ToDoList.GUI.Forms
{
    partial class ProjectDetailsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;



        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblProjectName = new Label();
            lblProjectDescription = new Label();
            pnlProjectColor = new Panel();
            btnAddTask = new Button();
            pnlTasksContainer = new Panel();
            btnClose = new Button();
            lblTasksTitle = new Label();
            SuspendLayout();
            // 
            // lblProjectName
            // 
            lblProjectName.AutoSize = true;
            lblProjectName.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblProjectName.ForeColor = Color.White;
            lblProjectName.Location = new Point(27, 31);
            lblProjectName.Margin = new Padding(4, 0, 4, 0);
            lblProjectName.Name = "lblProjectName";
            lblProjectName.Size = new Size(208, 41);
            lblProjectName.TabIndex = 0;
            lblProjectName.Text = "Project Name";
            // 
            // lblProjectDescription
            // 
            lblProjectDescription.AutoSize = true;
            lblProjectDescription.ForeColor = Color.Gray;
            lblProjectDescription.Location = new Point(27, 92);
            lblProjectDescription.Margin = new Padding(4, 0, 4, 0);
            lblProjectDescription.Name = "lblProjectDescription";
            lblProjectDescription.Size = new Size(135, 20);
            lblProjectDescription.TabIndex = 1;
            lblProjectDescription.Text = "Project Description";
            // 
            // pnlProjectColor
            // 
            pnlProjectColor.Location = new Point(27, 131);
            pnlProjectColor.Margin = new Padding(4, 5, 4, 5);
            pnlProjectColor.Name = "pnlProjectColor";
            pnlProjectColor.Size = new Size(40, 46);
            pnlProjectColor.TabIndex = 2;
            // 
            // btnAddTask
            // 
            btnAddTask.BackColor = Color.FromArgb(100, 200, 150);
            btnAddTask.FlatAppearance.BorderSize = 0;
            btnAddTask.FlatStyle = FlatStyle.Flat;
            btnAddTask.ForeColor = Color.Black;
            btnAddTask.Location = new Point(27, 200);
            btnAddTask.Margin = new Padding(4, 5, 4, 5);
            btnAddTask.Name = "btnAddTask";
            btnAddTask.Size = new Size(160, 54);
            btnAddTask.TabIndex = 3;
            btnAddTask.Text = "Thêm công việc";
            btnAddTask.UseVisualStyleBackColor = false;
            btnAddTask.Click += BtnAddTask_Click;
            // 
            // pnlTasksContainer
            // 
            pnlTasksContainer.AutoScroll = true;
            pnlTasksContainer.Location = new Point(27, 277);
            pnlTasksContainer.Margin = new Padding(4, 5, 4, 5);
            pnlTasksContainer.Name = "pnlTasksContainer";
            pnlTasksContainer.Size = new Size(960, 615);
            pnlTasksContainer.TabIndex = 4;
            // 
            // btnClose
            // 
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(960, 15);
            btnClose.Margin = new Padding(4, 5, 4, 5);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(27, 31);
            btnClose.TabIndex = 5;
            btnClose.Text = "X";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += BtnClose_Click;
            // 
            // lblTasksTitle
            // 
            lblTasksTitle.AutoSize = true;
            lblTasksTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTasksTitle.ForeColor = Color.White;
            lblTasksTitle.Location = new Point(27, 246);
            lblTasksTitle.Margin = new Padding(4, 0, 4, 0);
            lblTasksTitle.Name = "lblTasksTitle";
            lblTasksTitle.Size = new Size(206, 28);
            lblTasksTitle.TabIndex = 6;
            lblTasksTitle.Text = "Danh sách công việc";
            // 
            // ProjectDetailsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 24, 24);
            ClientSize = new Size(1013, 923);
            Controls.Add(lblTasksTitle);
            Controls.Add(btnClose);
            Controls.Add(pnlTasksContainer);
            Controls.Add(btnAddTask);
            Controls.Add(pnlProjectColor);
            Controls.Add(lblProjectDescription);
            Controls.Add(lblProjectName);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 5, 4, 5);
            Name = "ProjectDetailsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ProjectDetailsForm";
            Load += ProjectDetailsForm_Load;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.Label lblProjectDescription;
        private System.Windows.Forms.Panel pnlProjectColor;
        private System.Windows.Forms.Button btnAddTask;
        private System.Windows.Forms.Panel pnlTasksContainer;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTasksTitle;
    }
}
