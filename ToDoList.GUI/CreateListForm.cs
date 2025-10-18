using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TodoListApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.GUI
{
    public partial class CreateListForm : Form
    {
        private Color selectedColor = Color.FromArgb(204, 180, 72);
        private ToDoListContext _context;
        private int _userId = 1; // Tạm thời hardcode user ID, sau này sẽ lấy từ session

        public CreateListForm()
        {
            InitializeComponent();
            InitializeColorPicker();
            InitializeDatabase();
            
            // Initialize icon after form is shown to ensure proper dimensions
            this.Load += (s, e) => InitializeIconPictureBox();
            
            picIcon.Click += PicIcon_Click;
            btnClose.Click += (s, e) => this.Close();
            btnCancel.Click += (s, e) => this.Close();
            btnCreate.Click += BtnCreate_Click;
            
            // Add placeholder text handling
            txtListTitle.Enter += TxtListTitle_Enter;
            txtListTitle.Leave += TxtListTitle_Leave;
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

        private void TxtListTitle_Enter(object sender, EventArgs e)
        {
            if (txtListTitle.Text == "Enter your list title")
            {
                txtListTitle.Text = "";
                txtListTitle.ForeColor = Color.White;
            }
        }

        private void TxtListTitle_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtListTitle.Text))
            {
                txtListTitle.Text = "Enter your list title";
                txtListTitle.ForeColor = Color.Gray;
            }
        }

        private async void BtnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                // Disable button to prevent double click
                btnCreate.Enabled = false;
                btnCreate.Text = "Đang tạo...";

                // Validate input
                if (string.IsNullOrWhiteSpace(txtListTitle.Text) || 
                    txtListTitle.Text == "Enter your list title")
                {
                    MessageBox.Show("Vui lòng nhập tên danh sách!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnCreate.Enabled = true;
                    btnCreate.Text = "Create";
                    return;
                }

                // Check database connection
                if (_context == null)
                {
                    MessageBox.Show("Lỗi kết nối database. Vui lòng thử lại!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnCreate.Enabled = true;
                    btnCreate.Text = "Create";
                    return;
                }

                // Get or create default user
                var user = await _context.Users.FirstOrDefaultAsync();
                if (user == null)
                {
                    user = new User
                    {
                        FullName = "Default User",
                        Email = "user@example.com",
                        PasswordHash = "default",
                        CreatedAt = DateTime.Now,
                        IsActive = true
                    };
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }

                // Create new project
                var project = new Project
                {
                    UserId = user.UserId,
                    ProjectName = txtListTitle.Text.Trim(),
                    Description = $"Danh sách công việc: {txtListTitle.Text.Trim()}",
                    ColorCode = ColorTranslator.ToHtml(selectedColor),
                    CreatedAt = DateTime.Now,
                    IsArchived = false
                };

                // Save to database
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                MessageBox.Show("Tạo danh sách thành công!", "Thành công", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close form and return success
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo danh sách: {ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Re-enable button
                btnCreate.Enabled = true;
                btnCreate.Text = "Create";
            }
        }

        private void InitializeIconPictureBox()
        {
            // Set default background color
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
            // Simple check - you can improve this logic
            return picIcon.Tag == null || picIcon.Tag.ToString() == "default";
        }

        private void PicIcon_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.jpe;*.jfif|All Files|*.*";
                openFileDialog.Title = "Select an icon image";
                openFileDialog.CheckFileExists = true;
                openFileDialog.CheckPathExists = true;
                
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Validate file size (max 1MB)
                        var fileInfo = new System.IO.FileInfo(openFileDialog.FileName);
                        const long maxFileSize = 1 * 1024 * 1024; // 1MB in bytes
                        
                        if (fileInfo.Length > maxFileSize)
                        {
                            MessageBox.Show($"The selected file is too large ({fileInfo.Length / 1024.0 / 1024.0:F2} MB).\nPlease select an image smaller than 1 MB.", 
                                "File Too Large", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (fileInfo.Length == 0)
                        {
                            MessageBox.Show("The selected file is empty.", 
                                "Empty File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (picIcon.Image != null)
                        {
                            picIcon.Image.Dispose();
                            picIcon.Image = null;
                        }
                        
                        // Load and resize image to prevent memory issues
                        Image loadedImage = LoadImageSafely(openFileDialog.FileName);
                        if (loadedImage != null)
                        {
                            picIcon.Image = loadedImage;
                            picIcon.SizeMode = PictureBoxSizeMode.Zoom;
                            picIcon.Tag = "custom";
                            this.Invalidate(); // Force redraw
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Error", 
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
                // Validate file exists
                if (!System.IO.File.Exists(filePath))
                {
                    MessageBox.Show("The selected file does not exist.", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                // Method 1: Simple direct load
                try
                {
                    // Create a temporary copy to avoid file locking
                    string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), 
                        Guid.NewGuid().ToString() + System.IO.Path.GetExtension(filePath));
                    
                    System.IO.File.Copy(filePath, tempPath, true);
                    
                    loadedImage = Image.FromFile(tempPath);
                    
                    // Delete temp file
                    try { System.IO.File.Delete(tempPath); } catch { }
                    
                    if (loadedImage == null || loadedImage.Width <= 0 || loadedImage.Height <= 0)
                    {
                        loadedImage?.Dispose();
                        MessageBox.Show("The image has invalid dimensions.", 
                            "Invalid Image", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return null;
                    }

                    // Resize if needed
                    int maxSize = 512;
                    if (loadedImage.Width > maxSize || loadedImage.Height > maxSize)
                    {
                        Image resized = ResizeImage(loadedImage, maxSize, maxSize);
                        loadedImage.Dispose();
                        return resized;
                    }
                    
                    return loadedImage;
                }
                catch
                {
                    loadedImage?.Dispose();
                    
                    // Method 2: Fallback - load with Bitmap from bytes
                    try
                    {
                        byte[] imageBytes = System.IO.File.ReadAllBytes(filePath);
                        using (var ms = new System.IO.MemoryStream(imageBytes))
                        {
                            loadedImage = (Image)new Bitmap(ms);
                        }
                        
                        if (loadedImage == null || loadedImage.Width <= 0 || loadedImage.Height <= 0)
                        {
                            loadedImage?.Dispose();
                            MessageBox.Show("The image has invalid dimensions.", 
                                "Invalid Image", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        MessageBox.Show($"Cannot load this image.\n\nTry:\n1. Open the image in Paint\n2. Save As PNG\n3. Try the new file\n\nError: {ex.Message}", 
                            "Image Load Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return null;
                    }
                }
            }
            catch (OutOfMemoryException)
            {
                loadedImage?.Dispose();
                MessageBox.Show("The image is too large. Please select a smaller image.", 
                    "File Too Large", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (Exception ex)
            {
                loadedImage?.Dispose();
                MessageBox.Show($"Error: {ex.Message}", "Error", 
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
