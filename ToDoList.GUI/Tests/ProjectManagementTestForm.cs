using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToDoList.GUI.Tests
{
    public partial class ProjectManagementTestForm : Form
    {
        private ProjectManagementTest _test;

        public ProjectManagementTestForm()
        {
            InitializeComponent();
            _test = new ProjectManagementTest();
        }

        private async void BtnCreateProject_Click(object sender, EventArgs e)
        {
            btnCreateProject.Enabled = false;
            try
            {
                await _test.TestCreateProject();
            }
            finally
            {
                btnCreateProject.Enabled = true;
            }
        }

        private async void BtnGetProjects_Click(object sender, EventArgs e)
        {
            btnGetProjects.Enabled = false;
            try
            {
                await _test.TestGetProjects();
            }
            finally
            {
                btnGetProjects.Enabled = true;
            }
        }

        private async void BtnRunAllTests_Click(object sender, EventArgs e)
        {
            btnRunAllTests.Enabled = false;
            try
            {
                await _test.RunAllTests();
            }
            finally
            {
                btnRunAllTests.Enabled = true;
            }
        }

        private async void BtnCleanup_Click(object sender, EventArgs e)
        {
            btnCleanup.Enabled = false;
            try
            {
                await _test.CleanupTestData();
            }
            finally
            {
                btnCleanup.Enabled = true;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _test?.Dispose();
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
