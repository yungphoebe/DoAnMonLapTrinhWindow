namespace ToDoList.GUI.Tests
{
    partial class ProjectManagementTestForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnCreateProject = new System.Windows.Forms.Button();
            this.btnGetProjects = new System.Windows.Forms.Button();
            this.btnRunAllTests = new System.Windows.Forms.Button();
            this.btnCleanup = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(300, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Test Quản lý Project";
            // 
            // btnCreateProject
            // 
            this.btnCreateProject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(200)))), ((int)(((byte)(150)))));
            this.btnCreateProject.FlatAppearance.BorderSize = 0;
            this.btnCreateProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateProject.ForeColor = System.Drawing.Color.Black;
            this.btnCreateProject.Location = new System.Drawing.Point(20, 80);
            this.btnCreateProject.Name = "btnCreateProject";
            this.btnCreateProject.Size = new System.Drawing.Size(150, 40);
            this.btnCreateProject.TabIndex = 1;
            this.btnCreateProject.Text = "Test Tạo Project";
            this.btnCreateProject.UseVisualStyleBackColor = false;
            this.btnCreateProject.Click += new System.EventHandler(this.BtnCreateProject_Click);
            // 
            // btnGetProjects
            // 
            this.btnGetProjects.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(200)))), ((int)(((byte)(150)))));
            this.btnGetProjects.FlatAppearance.BorderSize = 0;
            this.btnGetProjects.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGetProjects.ForeColor = System.Drawing.Color.Black;
            this.btnGetProjects.Location = new System.Drawing.Point(190, 80);
            this.btnGetProjects.Name = "btnGetProjects";
            this.btnGetProjects.Size = new System.Drawing.Size(150, 40);
            this.btnGetProjects.TabIndex = 2;
            this.btnGetProjects.Text = "Test Lấy Projects";
            this.btnGetProjects.UseVisualStyleBackColor = false;
            this.btnGetProjects.Click += new System.EventHandler(this.BtnGetProjects_Click);
            // 
            // btnRunAllTests
            // 
            this.btnRunAllTests.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(100)))), ((int)(((byte)(150)))));
            this.btnRunAllTests.FlatAppearance.BorderSize = 0;
            this.btnRunAllTests.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunAllTests.ForeColor = System.Drawing.Color.White;
            this.btnRunAllTests.Location = new System.Drawing.Point(20, 140);
            this.btnRunAllTests.Name = "btnRunAllTests";
            this.btnRunAllTests.Size = new System.Drawing.Size(150, 40);
            this.btnRunAllTests.TabIndex = 3;
            this.btnRunAllTests.Text = "Chạy Tất Cả Test";
            this.btnRunAllTests.UseVisualStyleBackColor = false;
            this.btnRunAllTests.Click += new System.EventHandler(this.BtnRunAllTests_Click);
            // 
            // btnCleanup
            // 
            this.btnCleanup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.btnCleanup.FlatAppearance.BorderSize = 0;
            this.btnCleanup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCleanup.ForeColor = System.Drawing.Color.White;
            this.btnCleanup.Location = new System.Drawing.Point(190, 140);
            this.btnCleanup.Name = "btnCleanup";
            this.btnCleanup.Size = new System.Drawing.Size(150, 40);
            this.btnCleanup.TabIndex = 4;
            this.btnCleanup.Text = "Dọn Dẹp Dữ Liệu";
            this.btnCleanup.UseVisualStyleBackColor = false;
            this.btnCleanup.Click += new System.EventHandler(this.BtnCleanup_Click);
            // 
            // btnClose
            // 
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(360, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(20, 20);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.ForeColor = System.Drawing.Color.Gray;
            this.lblDescription.Location = new System.Drawing.Point(20, 50);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(200, 13);
            this.lblDescription.TabIndex = 6;
            this.lblDescription.Text = "Test các chức năng quản lý project";
            // 
            // ProjectManagementTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.ClientSize = new System.Drawing.Size(400, 200);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCleanup);
            this.Controls.Add(this.btnRunAllTests);
            this.Controls.Add(this.btnGetProjects);
            this.Controls.Add(this.btnCreateProject);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ProjectManagementTestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ProjectManagementTestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnCreateProject;
        private System.Windows.Forms.Button btnGetProjects;
        private System.Windows.Forms.Button btnRunAllTests;
        private System.Windows.Forms.Button btnCleanup;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblDescription;
    }
}
