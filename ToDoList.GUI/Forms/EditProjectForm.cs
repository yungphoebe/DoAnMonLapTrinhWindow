using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using TodoListApp.DAL.Models;

namespace ToDoList.GUI.Forms
{
    public partial class EditProjectForm : Form
    {
        private Color selectedColor = Color.FromArgb(204, 180, 72);
        private ToDoListContext? _context;
        private Project _project;

        public EditProjectForm(Project project)
        {
            InitializeComponent();
            _project = project;
            InitializeDatabase();
            InitializeColorPicker();
            LoadProjectData();
            
            // Initialize icon after form is shown
            this.Load += (s, e) => InitializeIconPictureBox();
            
            picIcon.Click += PicIcon_Click;
            btnClose.Click += (s, e) => this.Close();
            btnCancel.Click += (s, e) => this.Close();
            btnSave.Click += BtnSave_Click;
            
            // Add placeholder text handling
            txtProjectName.Enter += TxtProjectName_Enter;
            txtProjectName.Leave += TxtProjectName_Leave;
            txtDescription.Enter += TxtDescription_Enter;
            txtDescription.Leave += TxtDescription_Leave;
        }

        private void InitializeDatabase()
        {
            try
            {
                _context = new ToDoListContext();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối database: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProjectData()
        {
            // Load existing project data
            txtProjectName.Text = _project.ProjectName;
            txtProjectName.ForeColor = Color.White;
            
            if (!string.IsNullOrEmpty(_project.Description))
            {
                txtDescription.Text = _project.Description;
                txtDescription.ForeColor = Color.White;
            }
            
            // Load color
            if (!string.IsNullOrEmpty(_project.ColorCode))
            {
                try
                {
                    selectedColor = ColorTranslator.FromHtml(_project.ColorCode);
                    picIcon.BackColor = selectedColor;
                }
                catch
                {
                    selectedColor = Color.FromArgb(204, 180, 72);
                }
            }
        }

        private void TxtProjectName_Enter(object sender, EventArgs e)
        {
            if (txtProjectName.Text == "Nhập tên project")
            {
                txtProjectName.Text = "";
                txtProjectName.ForeColor = Color.White;
            }
        }

        private void TxtProjectName_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjectName.Text))
            {
                txtProjectName.Text = "Nhập tên project";
                txtProjectName.ForeColor = Color.Gray;
            }
        }

        private void TxtDescription_Enter(object sender, EventArgs e)
        {
            if (txtDescription.Text == "Nhập mô tả (tùy chọn)")
            {
                txtDescription.Text = "";
                txtDescription.ForeColor = Color.White;
            }
        }

        private void TxtDescription_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                txtDescription.Text = "Nhập mô tả (tùy chọn)";
                txtDescription.ForeColor = Color.Gray;
            }
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Disable button to prevent double click
                btnSave.Enabled = false;
                btnSave.Text = "Đang lưu...";

                // Validate input
                if (string.IsNullOrWhiteSpace(txtProjectName.Text) || 
                    txtProjectName.Text == "Nhập tên project")
                {
                    MessageBox.Show("Vui lòng nhập tên project!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnSave.Enabled = true;
                    btnSave.Text = "Lưu thay đổi";
                    return;
                }

                // Check database connection
                if (_context == null)
                {
                    MessageBox.Show("Lỗi kết nối database. Vui lòng thử lại!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnSave.Enabled = true;
                    btnSave.Text = "Lưu thay đổi";
                    return;
                }

                // Find the project in context and update
                var projectToUpdate = await _context.Projects.FindAsync(_project.ProjectId);
                if (projectToUpdate == null)
                {
                    MessageBox.Show("Không tìm thấy project!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // Update project properties
                projectToUpdate.ProjectName = txtProjectName.Text.Trim();
                projectToUpdate.Description = string.IsNullOrWhiteSpace(txtDescription.Text) || 
                                            txtDescription.Text == "Nhập mô tả (tùy chọn)" 
                    ? null 
                    : txtDescription.Text.Trim();
                projectToUpdate.ColorCode = ColorTranslator.ToHtml(selectedColor);

                // Save to database
                await _context.SaveChangesAsync();

                MessageBox.Show("Cập nhật project thành công!", "Thành công", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close form and return success
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật project: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Re-enable button
                btnSave.Enabled = true;
                btnSave.Text = "Lưu thay đổi";
            }
        }

        private void InitializeIconPictureBox()
        {
            // Set background color
            picIcon.BackColor = selectedColor;
            
            // Draw default icon
            DrawDefaultIcon();
        }

        private void DrawDefaultIcon()
        {
            if (picIcon.Image != null)
            {
                picIcon.Image.Dispose();
                picIcon.Image = null;
            }

            // Check if picIcon has valid dimensions
            if (picIcon.Width <= 0 || picIcon.Height <= 0)
            {
                return;
            }

            try
            {
                Bitmap bmp = new Bitmap(picIcon.Width, picIcon.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.Clear(selectedColor);
                    
                    // Draw image icon in the center
                    using (Font font = new Font("Segoe UI", 24, FontStyle.Bold))
                    {
                        string iconText = "📋";
                        SizeF textSize = g.MeasureString(iconText, font);
                        PointF location = new PointF(
                            (picIcon.Width - textSize.Width) / 2,
                            (picIcon.Height - textSize.Height) / 2
                        );
                        g.DrawString(iconText, font, Brushes.White, location);
                    }
                }
                picIcon.Image = bmp;
                picIcon.Tag = "default";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error drawing default icon: {ex.Message}");
            }
        }

        private void InitializeColorPicker()
        {
            Color[] colors = new Color[]
            {
                Color.FromArgb(171, 224, 255),
                Color.FromArgb(153, 170, 255),
                Color.FromArgb(204, 153, 255),
                Color.FromArgb(154, 229, 178),
                Color.FromArgb(153, 255, 255),
                Color.FromArgb(255, 255, 153),
                Color.FromArgb(204, 180, 72),
                Color.FromArgb(255, 153, 153),
                Color.FromArgb(153, 255, 153),
                Color.FromArgb(255, 204, 153)
            };

            foreach (Color color in colors)
            {
                CircularPanel colorPanel = new CircularPanel
                {
                    Width = 30,
                    Height = 30,
                    BackColor = color,
                    Margin = new Padding(5),
                    Cursor = Cursors.Hand
                };
                
                // Add click event to select color
                colorPanel.Click += (s, e) =>
                {
                    selectedColor = color;
                    picIcon.BackColor = color;
                    if (picIcon.Image == null || IsDefaultIcon())
                    {
                        DrawDefaultIcon();
                    }
                };
                
                flpColorPicker.Controls.Add(colorPanel);
            }
        }

        private bool IsDefaultIcon()
        {
            return picIcon.Tag == null || picIcon.Tag.ToString() == "default";
        }

        private void PicIcon_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.jpe;*.jfif|All Files|*.*";
                openFileDialog.Title = "Chọn icon cho project";
                openFileDialog.CheckFileExists = true;
                openFileDialog.CheckPathExists = true;
                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var fileInfo = new System.IO.FileInfo(openFileDialog.FileName);
                        const long maxFileSize = 1 * 1024 * 1024; // 1MB
                        
                        if (fileInfo.Length > maxFileSize)
                        {
                            MessageBox.Show($"File quá lớn ({fileInfo.Length / 1024.0 / 1024.0:F2} MB).\nVui lòng chọn ảnh nhỏ hơn 1 MB.", 
                                "File quá lớn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (picIcon.Image != null)
                        {
                            picIcon.Image.Dispose();
                            picIcon.Image = null;
                        }
                        
                        Image loadedImage = LoadImageSafely(openFileDialog.FileName);
                        if (loadedImage != null)
                        {
                            picIcon.Image = loadedImage;
                            picIcon.SizeMode = PictureBoxSizeMode.Zoom;
                            picIcon.Tag = "custom";
                            this.Invalidate();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi tải ảnh: {ex.Message}", "Lỗi", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private Image LoadImageSafely(string filePath)
        {
            Image loadedImage = null;
            
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    MessageBox.Show("File không tồn tại.", 
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), 
                    Guid.NewGuid().ToString() + System.IO.Path.GetExtension(filePath));
                
                System.IO.File.Copy(filePath, tempPath, true);
                loadedImage = Image.FromFile(tempPath);
                
                try { System.IO.File.Delete(tempPath); } catch { }
                
                if (loadedImage == null || loadedImage.Width <= 0 || loadedImage.Height <= 0)
                {
                    loadedImage?.Dispose();
                    return null;
                }

                int maxSize = 512;
                if (loadedImage.Width > maxSize || loadedImage.Height > maxSize)
                {
                    Image resized = ResizeImage(loadedImage, maxSize, maxSize);
                    loadedImage.Dispose();
                    return resized;
                }
                
                return loadedImage;
            }
            catch (Exception ex)
            {
                loadedImage?.Dispose();
                MessageBox.Show($"Không thể tải ảnh: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private Image ResizeImage(Image image, int maxWidth, int maxHeight)
        {
            if (image == null || image.Width <= 0 || image.Height <= 0)
            {
                throw new ArgumentException("Invalid image dimensions");
            }

            double ratioX = (double)maxWidth / image.Width;
            double ratioY = (double)maxHeight / image.Height;
            double ratio = Math.Min(ratioX, ratioY);

            int newWidth = Math.Max(1, (int)(image.Width * ratio));
            int newHeight = Math.Max(1, (int)(image.Height * ratio));

            Bitmap newImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return newImage;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}