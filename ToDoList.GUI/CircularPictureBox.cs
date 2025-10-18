using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ToDoList.GUI
{
    public class CircularPictureBox : PictureBox
    {
        public CircularPictureBox()
        {
            this.SizeMode = PictureBoxSizeMode.Zoom;
        }
        
        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            using (var gp = new GraphicsPath())
            {
                gp.AddEllipse(new Rectangle(0, 0, this.Width - 1, this.Height - 1));
                this.Region = new Region(gp);
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (this.Image != null)
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                pe.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                pe.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                pe.Graphics.CompositingQuality = CompositingQuality.HighQuality;

                // Create circular clip region
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(0, 0, this.Width, this.Height);
                    pe.Graphics.SetClip(path);
                    
                    // Calculate the aspect ratio and draw to fill the circle
                    float imageAspect = (float)this.Image.Width / this.Image.Height;
                    float controlAspect = (float)this.Width / this.Height;
                    
                    Rectangle destRect;
                    
                    if (imageAspect > controlAspect)
                    {
                        // Image is wider - fit to height and center horizontally
                        int width = (int)(this.Height * imageAspect);
                        int x = (this.Width - width) / 2;
                        destRect = new Rectangle(x, 0, width, this.Height);
                    }
                    else
                    {
                        // Image is taller - fit to width and center vertically
                        int height = (int)(this.Width / imageAspect);
                        int y = (this.Height - height) / 2;
                        destRect = new Rectangle(0, y, this.Width, height);
                    }
                    
                    pe.Graphics.DrawImage(this.Image, destRect);
                }
            }
            else
            {
                // Fill background if no image
                using (var brush = new SolidBrush(this.BackColor))
                {
                    pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    pe.Graphics.FillEllipse(brush, 0, 0, this.Width - 1, this.Height - 1);
                }
            }
        }
    }
}
