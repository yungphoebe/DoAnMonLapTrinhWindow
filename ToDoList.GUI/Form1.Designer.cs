namespace ToDoList.GUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;



        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlSidebar = new Panel();
            lblTrialInfo = new Label();
            btnAllMyLists = new Button();
            btnCreateNewList = new Button();
            lblAppName = new Label();
            pnlTop = new Panel();
            lblUserName = new Label();
            lblGreeting = new Label();
            btnSettings = new Button();
            btnSearch = new Button();
            btnMenu = new Button();
            pnlMain = new Panel();
            pnlListsContainer = new FlowLayoutPanel();
            pnlListHeader = new Panel();
            lblYourLists = new Label();
            lblListsSubtitle = new Label();
            pnlBottom = new Panel();
            btnAddNewTask = new Button();
            btnReports = new Button();
            btnHome = new Button();
            lblPlan = new Label();
            pnlSidebar.SuspendLayout();
            pnlTop.SuspendLayout();
            pnlMain.SuspendLayout();
            pnlListHeader.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSidebar
            // 
            pnlSidebar.BackColor = Color.FromArgb(24, 24, 24);
            pnlSidebar.Controls.Add(lblTrialInfo);
            pnlSidebar.Controls.Add(btnAllMyLists);
            pnlSidebar.Controls.Add(btnCreateNewList);
            pnlSidebar.Controls.Add(lblAppName);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Location = new Point(0, 0);
            pnlSidebar.Margin = new Padding(3, 4, 3, 4);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Padding = new Padding(20, 25, 20, 25);
            pnlSidebar.Size = new Size(320, 1025);
            pnlSidebar.TabIndex = 0;
            // 
            // lblTrialInfo
            // 
            lblTrialInfo.Font = new Font("Segoe UI", 8F);
            lblTrialInfo.ForeColor = Color.Gray;
            lblTrialInfo.Location = new Point(20, 120);
            lblTrialInfo.Name = "lblTrialInfo";
            lblTrialInfo.Size = new Size(280, 120);
            lblTrialInfo.TabIndex = 2;
            lblTrialInfo.Text = "Bạn còn 6 ngày nữa trước khi kết thúc bản dùng thử. Sau đó, tất cả các tính năng sẽ bị khóa cho đến khi bạn nâng cấp tài khoản của mình.";
            // 
            // btnAllMyLists
            // 
            btnAllMyLists.BackColor = Color.FromArgb(40, 40, 40);
            btnAllMyLists.FlatAppearance.BorderSize = 0;
            btnAllMyLists.FlatStyle = FlatStyle.Flat;
            btnAllMyLists.Font = new Font("Segoe UI", 10F);
            btnAllMyLists.ForeColor = Color.White;
            btnAllMyLists.Location = new Point(20, 825);
            btnAllMyLists.Margin = new Padding(3, 4, 3, 4);
            btnAllMyLists.Name = "btnAllMyLists";
            btnAllMyLists.Size = new Size(280, 56);
            btnAllMyLists.TabIndex = 5;
            btnAllMyLists.Text = "Tất cả danh sách của tôi";
            btnAllMyLists.UseVisualStyleBackColor = false;
            // 
            // btnCreateNewList
            // 
            btnCreateNewList.FlatAppearance.BorderSize = 0;
            btnCreateNewList.FlatStyle = FlatStyle.Flat;
            btnCreateNewList.Font = new Font("Segoe UI", 9F);
            btnCreateNewList.ForeColor = Color.Gray;
            btnCreateNewList.Location = new Point(20, 756);
            btnCreateNewList.Margin = new Padding(3, 4, 3, 4);
            btnCreateNewList.Name = "btnCreateNewList";
            btnCreateNewList.Size = new Size(280, 50);
            btnCreateNewList.TabIndex = 4;
            btnCreateNewList.Text = "+ Tạo danh sách mới";
            btnCreateNewList.TextAlign = ContentAlignment.MiddleLeft;
            btnCreateNewList.UseVisualStyleBackColor = true;
            // 
            // lblAppName
            // 
            lblAppName.AutoSize = true;
            lblAppName.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblAppName.ForeColor = Color.White;
            lblAppName.Location = new Point(20, 38);
            lblAppName.Name = "lblAppName";
            lblAppName.Size = new Size(153, 37);
            lblAppName.TabIndex = 0;
            lblAppName.Text = "✓ CuLuList";
            // 
            // pnlTop
            // 
            pnlTop.BackColor = Color.FromArgb(24, 24, 24);
            pnlTop.Controls.Add(lblUserName);
            pnlTop.Controls.Add(lblGreeting);
            pnlTop.Controls.Add(btnSettings);
            pnlTop.Controls.Add(btnSearch);
            pnlTop.Controls.Add(btnMenu);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(320, 0);
            pnlTop.Margin = new Padding(3, 4, 3, 4);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(1080, 125);
            pnlTop.TabIndex = 1;
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.Font = new Font("Segoe UI", 9F);
            lblUserName.ForeColor = Color.Gray;
            lblUserName.Location = new Point(30, 75);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(349, 20);
            lblUserName.TabIndex = 4;
            lblUserName.Text = "Tuyệt vời! Bạn đang làm việc trong khi thế giới ngủ.";
            // 
            // lblGreeting
            // 
            lblGreeting.AutoSize = true;
            lblGreeting.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblGreeting.ForeColor = Color.White;
            lblGreeting.Location = new Point(30, 25);
            lblGreeting.Name = "lblGreeting";
            lblGreeting.Size = new Size(287, 41);
            lblGreeting.TabIndex = 3;
            lblGreeting.Text = "Chào buổi tối, User";
            // 
            // btnSettings
            // 
            btnSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Font = new Font("Segoe UI", 14F);
            btnSettings.ForeColor = Color.White;
            btnSettings.Location = new Point(1020, 38);
            btnSettings.Margin = new Padding(3, 4, 3, 4);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(40, 50);
            btnSettings.TabIndex = 2;
            btnSettings.Text = "⚙";
            btnSettings.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Segoe UI", 14F);
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(970, 38);
            btnSearch.Margin = new Padding(3, 4, 3, 4);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(40, 50);
            btnSearch.TabIndex = 1;
            btnSearch.Text = "🔍";
            btnSearch.UseVisualStyleBackColor = true;
            // 
            // btnMenu
            // 
            btnMenu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMenu.FlatAppearance.BorderSize = 0;
            btnMenu.FlatStyle = FlatStyle.Flat;
            btnMenu.Font = new Font("Segoe UI", 14F);
            btnMenu.ForeColor = Color.White;
            btnMenu.Location = new Point(920, 38);
            btnMenu.Margin = new Padding(3, 4, 3, 4);
            btnMenu.Name = "btnMenu";
            btnMenu.Size = new Size(40, 50);
            btnMenu.TabIndex = 0;
            btnMenu.Text = "⋮⋮⋮";
            btnMenu.UseVisualStyleBackColor = true;
            // 
            // pnlMain
            // 
            pnlMain.BackColor = Color.FromArgb(18, 18, 18);
            pnlMain.Controls.Add(pnlListsContainer);
            pnlMain.Controls.Add(pnlListHeader);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(320, 125);
            pnlMain.Margin = new Padding(3, 4, 3, 4);
            pnlMain.Name = "pnlMain";
            pnlMain.Padding = new Padding(30, 0, 30, 0);
            pnlMain.Size = new Size(1080, 812);
            pnlMain.TabIndex = 2;
            // 
            // pnlListsContainer
            // 
            pnlListsContainer.AutoScroll = true;
            pnlListsContainer.Dock = DockStyle.Fill;
            pnlListsContainer.Location = new Point(30, 100);
            pnlListsContainer.Margin = new Padding(3, 4, 3, 4);
            pnlListsContainer.Name = "pnlListsContainer";
            pnlListsContainer.Padding = new Padding(0, 10, 0, 10);
            pnlListsContainer.Size = new Size(1020, 712);
            pnlListsContainer.TabIndex = 2;
            // 
            // pnlListHeader
            // 
            pnlListHeader.Controls.Add(lblYourLists);
            pnlListHeader.Controls.Add(lblListsSubtitle);
            pnlListHeader.Dock = DockStyle.Top;
            pnlListHeader.Location = new Point(30, 0);
            pnlListHeader.Name = "pnlListHeader";
            pnlListHeader.Size = new Size(1020, 100);
            pnlListHeader.TabIndex = 3;
            // 
            // lblYourLists
            // 
            lblYourLists.AutoSize = true;
            lblYourLists.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblYourLists.ForeColor = Color.White;
            lblYourLists.Location = new Point(0, 38);
            lblYourLists.Name = "lblYourLists";
            lblYourLists.Size = new Size(260, 37);
            lblYourLists.TabIndex = 0;
            lblYourLists.Text = "Danh Sách Của Bạn";
            // 
            // lblListsSubtitle
            // 
            lblListsSubtitle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblListsSubtitle.Font = new Font("Segoe UI", 9F);
            lblListsSubtitle.ForeColor = Color.Gray;
            lblListsSubtitle.Location = new Point(670, 44);
            lblListsSubtitle.Name = "lblListsSubtitle";
            lblListsSubtitle.Size = new Size(350, 31);
            lblListsSubtitle.TabIndex = 1;
            lblListsSubtitle.Text = "Danh sách có các công việc sắp tới của bạn";
            lblListsSubtitle.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlBottom
            // 
            pnlBottom.BackColor = Color.FromArgb(24, 24, 24);
            pnlBottom.Controls.Add(btnAddNewTask);
            pnlBottom.Controls.Add(btnReports);
            pnlBottom.Controls.Add(btnHome);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(320, 937);
            pnlBottom.Margin = new Padding(3, 4, 3, 4);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(1080, 88);
            pnlBottom.TabIndex = 3;
            // 
            // btnAddNewTask
            // 
            btnAddNewTask.Anchor = AnchorStyles.Bottom;
            btnAddNewTask.BackColor = Color.FromArgb(100, 200, 150);
            btnAddNewTask.FlatAppearance.BorderSize = 0;
            btnAddNewTask.FlatStyle = FlatStyle.Flat;
            btnAddNewTask.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnAddNewTask.ForeColor = Color.Black;
            btnAddNewTask.Location = new Point(440, 12);
            btnAddNewTask.Margin = new Padding(3, 4, 3, 4);
            btnAddNewTask.Name = "btnAddNewTask";
            btnAddNewTask.Size = new Size(200, 62);
            btnAddNewTask.TabIndex = 2;
            btnAddNewTask.Text = "Thêm công việc mới";
            btnAddNewTask.UseVisualStyleBackColor = false;
            // 
            // btnReports
            // 
            btnReports.FlatAppearance.BorderSize = 0;
            btnReports.FlatStyle = FlatStyle.Flat;
            btnReports.Font = new Font("Segoe UI", 10F);
            btnReports.ForeColor = Color.Gray;
            btnReports.Location = new Point(107, 12);
            btnReports.Margin = new Padding(3, 4, 3, 4);
            btnReports.Name = "btnReports";
            btnReports.Size = new Size(141, 50);
            btnReports.TabIndex = 1;
            btnReports.Text = "📊 Báo cáo";
            btnReports.TextAlign = ContentAlignment.MiddleLeft;
            btnReports.UseVisualStyleBackColor = true;
            // 
            // btnHome
            // 
            btnHome.FlatAppearance.BorderSize = 0;
            btnHome.FlatStyle = FlatStyle.Flat;
            btnHome.Font = new Font("Segoe UI", 10F);
            btnHome.ForeColor = Color.White;
            btnHome.Location = new Point(20, 19);
            btnHome.Margin = new Padding(3, 4, 3, 4);
            btnHome.Name = "btnHome";
            btnHome.Size = new Size(100, 50);
            btnHome.TabIndex = 0;
            btnHome.Text = "🏠 Trang chủ";
            btnHome.TextAlign = ContentAlignment.MiddleLeft;
            btnHome.UseVisualStyleBackColor = true;
            // 
            // lblPlan
            // 
            lblPlan.Location = new Point(0, 0);
            lblPlan.Name = "lblPlan";
            lblPlan.Size = new Size(100, 23);
            lblPlan.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(18, 18, 18);
            ClientSize = new Size(1400, 1025);
            Controls.Add(pnlMain);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTop);
            Controls.Add(pnlSidebar);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(1400, 988);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TodoList - Quản Lý Công Việc";
            pnlSidebar.ResumeLayout(false);
            pnlSidebar.PerformLayout();
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            pnlMain.ResumeLayout(false);
            pnlListHeader.ResumeLayout(false);
            pnlListHeader.PerformLayout();
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.Button btnCreateNewList;
        private System.Windows.Forms.Button btnAllMyLists;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Label lblGreeting;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblYourLists;
        private System.Windows.Forms.Label lblListsSubtitle;
        private System.Windows.Forms.FlowLayoutPanel pnlListsContainer;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnAddNewTask;
        private Label lblPlan;
        private Label lblTrialInfo;
        private Panel pnlListHeader;
    }
}
