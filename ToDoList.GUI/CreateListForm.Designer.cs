namespace ToDoList.GUI
{
    partial class CreateListForm
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
            this.lblCreateNewList = new System.Windows.Forms.Label();
            this.picIcon = new ToDoList.GUI.CircularPictureBox();
            this.lblUploadIcon = new System.Windows.Forms.Label();
            this.lblPickColor = new System.Windows.Forms.Label();
            this.txtListTitle = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.flpColorPicker = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCreateNewList
            // 
            this.lblCreateNewList.AutoSize = true;
            this.lblCreateNewList.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblCreateNewList.ForeColor = System.Drawing.Color.White;
            this.lblCreateNewList.Location = new System.Drawing.Point(150, 40);
            this.lblCreateNewList.Name = "lblCreateNewList";
            this.lblCreateNewList.Size = new System.Drawing.Size(180, 25);
            this.lblCreateNewList.TabIndex = 0;
            this.lblCreateNewList.Text = "Create a new list";
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
            this.lblUploadIcon.Location = new System.Drawing.Point(190, 170);
            this.lblUploadIcon.Name = "lblUploadIcon";
            this.lblUploadIcon.Size = new System.Drawing.Size(100, 13);
            this.lblUploadIcon.TabIndex = 2;
            this.lblUploadIcon.Text = "UPLOAD AN ICON";
            // 
            // lblPickColor
            // 
            this.lblPickColor.AutoSize = true;
            this.lblPickColor.ForeColor = System.Drawing.Color.Gray;
            this.lblPickColor.Location = new System.Drawing.Point(50, 220);
            this.lblPickColor.Name = "lblPickColor";
            this.lblPickColor.Size = new System.Drawing.Size(82, 13);
            this.lblPickColor.TabIndex = 3;
            this.lblPickColor.Text = "Pick a list color";
            // 
            // txtListTitle
            // 
            this.txtListTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.txtListTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtListTitle.ForeColor = System.Drawing.Color.White;
            this.txtListTitle.Location = new System.Drawing.Point(50, 320);
            this.txtListTitle.Name = "txtListTitle";
            this.txtListTitle.Size = new System.Drawing.Size(380, 20);
            this.txtListTitle.TabIndex = 4;
            this.txtListTitle.Text = "Enter your list title";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(50, 370);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(180, 40);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnCreate
            // 
            this.btnCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(200)))), ((int)(((byte)(150)))));
            this.btnCreate.FlatAppearance.BorderSize = 0;
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreate.ForeColor = System.Drawing.Color.Black;
            this.btnCreate.Location = new System.Drawing.Point(250, 370);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(180, 40);
            this.btnCreate.TabIndex = 6;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(450, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(20, 20);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // flpColorPicker
            // 
            this.flpColorPicker.Location = new System.Drawing.Point(50, 250);
            this.flpColorPicker.Name = "flpColorPicker";
            this.flpColorPicker.Size = new System.Drawing.Size(380, 40);
            this.flpColorPicker.TabIndex = 8;
            // 
            // CreateListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.ClientSize = new System.Drawing.Size(480, 450);
            this.Controls.Add(this.flpColorPicker);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtListTitle);
            this.Controls.Add(this.lblPickColor);
            this.Controls.Add(this.lblUploadIcon);
            this.Controls.Add(this.picIcon);
            this.Controls.Add(this.lblCreateNewList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CreateListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CreateListForm";
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCreateNewList;
        private CircularPictureBox picIcon;
        private System.Windows.Forms.Label lblUploadIcon;
        private System.Windows.Forms.Label lblPickColor;
        private System.Windows.Forms.TextBox txtListTitle;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.FlowLayoutPanel flpColorPicker;
    }
}
