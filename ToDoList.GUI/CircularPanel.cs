using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ToDoList.GUI
{
    public class CircularPanel : Panel
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(0, 0, this.Width, this.Height);
                this.Region = new Region(path);
                using (var brush = new SolidBrush(this.BackColor))
                {
                    e.Graphics.FillEllipse(brush, 0, 0, this.Width, this.Height);
                }
            }
        }
    }
}
