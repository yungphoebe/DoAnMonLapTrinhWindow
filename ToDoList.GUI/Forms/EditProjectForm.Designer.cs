namespace ToDoList.GUI.Forms
{
    partial class EditProjectForm
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
            this.lblEditProject = new System.Windows.Forms.Label();
            this.picIcon = new ToDoList.GUI.CircularPictureBox();
            this.lblUploadIcon = new System.Windows.Forms.Label();
            this.lblPickColor = new System.Windows.Forms.Label();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.flpColorPicker = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // lblEditProject
            // 
            this.lblEditProject.AutoSize = true;
            this.lblEditProject.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblEditProject.ForeColor = System.Drawing.Color.White;
            this.lblEditProject.Location = new System.Drawing.Point(150, 40);
            this.lblEditProject.Name = "lblEditProject";
            this.lblEditProject.Size = new System.Drawing.Size(180, 25);
            this.lblEditProject.TabIndex = 0;
            this.lblEditProject.Text = "?? Ch?nh s?a Project";
            // 
            // picIcon
            // 
            this.picIcon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.picIcon.Location = new System.Drawing.Point(200, 80);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(80, 80);
            this.picIcon.TabIndex = 1;
            this.picIcon.TabStop = false;
            this.picIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            // 
            // lblUploadIcon
            // 
            this.lblUploadIcon.AutoSize = true;
            this.lblUploadIcon.ForeColor = System.Drawing.Color.Gray;
            this.lblUploadIcon.Location = new System.Drawing.Point(170, 170);
            this.lblUploadIcon.Name = "lblUploadIcon";
            this.lblUploadIcon.Size = new System.Drawing.Size(140, 13);
            this.lblUploadIcon.TabIndex = 2;
            this.lblUploadIcon.Text = "T?I ICON (tùy ch?n)";
            // 
            // lblPickColor
            // 
            this.lblPickColor.AutoSize = true;
            this.lblPickColor.ForeColor = System.Drawing.Color.Gray;
            this.lblPickColor.Location = new System.Drawing.Point(50, 220);
            this.lblPickColor.Name = "lblPickColor";
            this.lblPickColor.Size = new System.Drawing.Size(82, 13);
            this.lblPickColor.TabIndex = 3;
            this.lblPickColor.Text = "Ch?n màu s?c";
            // 
            // txtProjectName
            // 
            this.txtProjectName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtProjectName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProjectName.ForeColor = System.Drawing.Color.White;
            this.txtProjectName.Location = new System.Drawing.Point(50, 320);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(380, 20);
            this.txtProjectName.TabIndex = 4;
            this.txtProjectName.Text = "Nh?p tên project";
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescription.ForeColor = System.Drawing.Color.Gray;
            this.txtDescription.Location = new System.Drawing.Point(50, 380);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(380, 60);
            this.txtDescription.TabIndex = 5;
            this.txtDescription.Text = "Nh?p mô t? (tùy ch?n)";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.ForeColor = System.Drawing.Color.Gray;
            this.lblDescription.Location = new System.Drawing.Point(50, 360);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(40, 13);
            this.lblDescription.TabIndex = 6;
            this.lblDescription.Text = "Mô t?";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(50, 460);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(180, 40);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "H?y";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(200)))), ((int)(((byte)(150)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Location = new System.Drawing.Point(250, 460);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(180, 40);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "L?u thay ??i";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(450, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(20, 20);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // flpColorPicker
            // 
            this.flpColorPicker.Location = new System.Drawing.Point(50, 250);
            this.flpColorPicker.Name = "flpColorPicker";
            this.flpColorPicker.Size = new System.Drawing.Size(380, 40);
            this.flpColorPicker.TabIndex = 10;
            // 
            // EditProjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.ClientSize = new System.Drawing.Size(480, 540);
            this.Controls.Add(this.flpColorPicker);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtProjectName);
            this.Controls.Add(this.lblPickColor);
            this.Controls.Add(this.lblUploadIcon);
            this.Controls.Add(this.picIcon);
            this.Controls.Add(this.lblEditProject);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "EditProjectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EditProjectForm";
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblEditProject;
        private CircularPictureBox picIcon;
        private System.Windows.Forms.Label lblUploadIcon;
        private System.Windows.Forms.Label lblPickColor;
        private System.Windows.Forms.TextBox txtProjectName;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.FlowLayoutPanel flpColorPicker;
    }
}
