using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace ToDoList.GUI.Components
{
    public class InteractiveChartControl : Control
    {
        private List<ChartDataPoint> _data = new List<ChartDataPoint>();
        private ChartType _chartType = ChartType.Bar;
        private Point _lastMousePosition;
        private bool _isDragging = false;
        private float _zoomLevel = 1.0f;
        private PointF _panOffset = new PointF(0, 0);
        private ChartDataPoint _hoveredPoint = null;
        private ToolTip _tooltip = new ToolTip();

        public enum ChartType
        {
            Bar,
            Line,
            Pie,
            Area
        }

        public class ChartDataPoint
        {
            public string Label { get; set; }
            public double Value { get; set; }
            public Color Color { get; set; }
            public string Category { get; set; }
            public DateTime? Date { get; set; }
        }

        public InteractiveChartControl()
        {
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.BackColor = Color.FromArgb(25, 25, 25);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular); // Chuẩn hóa font cho control
            
            this.MouseWheel += InteractiveChartControl_MouseWheel;
            this.MouseDown += InteractiveChartControl_MouseDown;
            this.MouseMove += InteractiveChartControl_MouseMove;
            this.MouseUp += InteractiveChartControl_MouseUp;
            this.MouseLeave += InteractiveChartControl_MouseLeave;
            
            _tooltip.InitialDelay = 0;
            _tooltip.ReshowDelay = 0;
            _tooltip.AutoPopDelay = 5000;
            _tooltip.IsBalloon = false; // Đảm bảo tooltip hiển thị chuẩn
        }

        public void SetData(List<ChartDataPoint> data, ChartType chartType = ChartType.Bar)
        {
            _data = data ?? new List<ChartDataPoint>();
            _chartType = chartType;
            _zoomLevel = 1.0f;
            _panOffset = new PointF(0, 0);
            Invalidate();
        }

        private void InteractiveChartControl_MouseWheel(object sender, MouseEventArgs e)
        {
            // Zoom with mouse wheel
            float zoomFactor = e.Delta > 0 ? 1.1f : 0.9f;
            float newZoom = _zoomLevel * zoomFactor;
            
            // Limit zoom range
            if (newZoom >= 0.5f && newZoom <= 3.0f)
            {
                _zoomLevel = newZoom;
                Invalidate();
            }
        }

        private void InteractiveChartControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                _lastMousePosition = e.Location;
                Cursor = Cursors.Hand;
            }
        }

        private void InteractiveChartControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                // Pan the chart
                _panOffset.X += (e.X - _lastMousePosition.X) / _zoomLevel;
                _panOffset.Y += (e.Y - _lastMousePosition.Y) / _zoomLevel;
                _lastMousePosition = e.Location;
                Invalidate();
            }
            else
            {
                // Check for hover
                UpdateHoverState(e.Location);
            }
        }

        private void InteractiveChartControl_MouseUp(object sender, MouseEventArgs e)
        {
            _isDragging = false;
            Cursor = Cursors.Default;
        }

        private void InteractiveChartControl_MouseLeave(object sender, EventArgs e)
        {
            _hoveredPoint = null;
            _tooltip.Hide(this);
            Invalidate();
        }

        private void UpdateHoverState(Point mousePos)
        {
            if (_chartType == ChartType.Bar)
            {
                var chartArea = GetChartArea();
                if (!_data.Any()) return;

                float barWidth = chartArea.Width / _data.Count * 0.8f;
                float spacing = chartArea.Width / _data.Count * 0.2f;
                float maxValue = (float)_data.Max(d => d.Value);

                // ✅ FIX: Xử lý maxValue = 0
                if (maxValue <= 0)
                {
                    maxValue = 1f;
                }

                ChartDataPoint newHovered = null;

                for (int i = 0; i < _data.Count; i++)
                {
                    float x = chartArea.X + i * (barWidth + spacing) + _panOffset.X * _zoomLevel;
                    float height = (float)(chartArea.Height * (_data[i].Value / maxValue) * _zoomLevel);
                    
                    // ✅ FIX: Đảm bảo chiều cao tối thiểu
                    if (height < 1f) height = 1f;
                    
                    float y = chartArea.Bottom - height;

                    var barRect = new RectangleF(x, y, barWidth * _zoomLevel, height);

                    if (barRect.Contains(mousePos))
                    {
                        newHovered = _data[i];
                        break;
                    }
                }

                if (newHovered != _hoveredPoint)
                {
                    _hoveredPoint = newHovered;
                    
                    if (_hoveredPoint != null)
                    {
                        string tooltipText = $"{_hoveredPoint.Label}\n" +
                                           $"Giá trị: {_hoveredPoint.Value:F2}\n" +
                                           (_hoveredPoint.Date.HasValue ? $"Ngày: {_hoveredPoint.Date.Value:dd/MM/yyyy}" : "");
                        _tooltip.Show(tooltipText, this, mousePos.X + 15, mousePos.Y - 30);
                    }
                    else
                    {
                        _tooltip.Hide(this);
                    }
                    
                    Invalidate();
                }
            }
        }

        private Rectangle GetChartArea()
        {
            int margin = 60;
            return new Rectangle(
                margin,
                margin,
                Width - margin * 2,
                Height - margin * 2
            );
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            if (!_data.Any())
            {
                DrawNoDataMessage(e.Graphics);
                return;
            }

            // Draw title and controls info
            DrawTitle(e.Graphics);
            DrawControls(e.Graphics);

            switch (_chartType)
            {
                case ChartType.Bar:
                    DrawBarChart(e.Graphics);
                    break;
                case ChartType.Line:
                    DrawLineChart(e.Graphics);
                    break;
                case ChartType.Pie:
                    DrawPieChart(e.Graphics);
                    break;
                case ChartType.Area:
                    DrawAreaChart(e.Graphics);
                    break;
            }
        }

        private void DrawNoDataMessage(Graphics g)
        {
            string message = "Không có dữ liệu";
            using (var font = new Font("Segoe UI", 14F, FontStyle.Regular))
            using (var brush = new SolidBrush(Color.FromArgb(150, 150, 150)))
            using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                g.DrawString(message, font, brush, new RectangleF(0, 0, Width, Height), format);
            }
        }

        private void DrawTitle(Graphics g)
        {
            string title = _chartType switch
            {
                ChartType.Bar => "📊 Biểu Đồ Cột Tương Tác",
                ChartType.Line => "📈 Biểu Đồ Đường Tương Tác",
                ChartType.Pie => "🥧 Biểu Đồ Tròn Tương Tác",
                ChartType.Area => "📉 Biểu Đồ Vùng Tương Tác",
                _ => "📊 Biểu Đồ Tương Tác"
            };

            using (var font = new Font("Segoe UI", 16F, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.White))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                g.DrawString(title, font, brush, 10, 10);
            }
        }

        private void DrawControls(Graphics g)
        {
            string controls = $"🔍 Zoom: {_zoomLevel:F1}x | 🖱️ Lăn chuột để Zoom | 🤚 Kéo để Di chuyển";
            
            using (var font = new Font("Segoe UI", 9F, FontStyle.Regular))
            using (var brush = new SolidBrush(Color.FromArgb(150, 150, 150)))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                var textSize = g.MeasureString(controls, font);
                g.DrawString(controls, font, brush, Width - textSize.Width - 10, 15);
            }
        }

        private void DrawBarChart(Graphics g)
        {
            var chartArea = GetChartArea();
            if (!_data.Any()) return;

            float barWidth = chartArea.Width / _data.Count * 0.8f;
            float spacing = chartArea.Width / _data.Count * 0.2f;
            float maxValue = (float)_data.Max(d => d.Value);

            // ✅ FIX: Xử lý trường hợp maxValue = 0 (tất cả giá trị đều = 0)
            if (maxValue <= 0)
            {
                maxValue = 1f; // Đặt giá trị mặc định để tránh chia cho 0
            }

            // ✅ FIX: Đảm bảo barWidth hợp lệ
            if (barWidth <= 0)
            {
                barWidth = 1f;
            }
    
            // Draw grid lines
            DrawGridLines(g, chartArea, maxValue);

            // Draw bars
            for (int i = 0; i < _data.Count; i++)
            {
                var dataPoint = _data[i];
                float x = chartArea.X + i * (barWidth + spacing) + _panOffset.X * _zoomLevel;
                float height = (float)(chartArea.Height * (dataPoint.Value / maxValue) * _zoomLevel);
                
                // ✅ FIX: Đảm bảo chiều cao tối thiểu để tránh ArgumentException
                if (height < 1f)
                {
                    height = 1f; // Chiều cao tối thiểu 1 pixelx    
                }
                
                float y = chartArea.Bottom - height;

                // Draw bar with gradient
                var rect = new RectangleF(x, y, barWidth * _zoomLevel, height);
                
                // ✅ FIX: Đảm bảo kích thước rect hợp lệ trước khi tạo gradient
                if (rect.Width < 1f) rect.Width = 1f;
                if (rect.Height < 1f) rect.Height = 1f;
                
                // Highlight on hover
                bool isHovered = dataPoint == _hoveredPoint;
                Color barColor = isHovered ? 
                    Color.FromArgb(255, dataPoint.Color.R, dataPoint.Color.G, dataPoint.Color.B) :
                    Color.FromArgb(200, dataPoint.Color.R, dataPoint.Color.G, dataPoint.Color.B);

                using (var brush = new LinearGradientBrush(rect, 
                    barColor, 
                    Color.FromArgb(barColor.A, barColor.R / 2, barColor.G / 2, barColor.B / 2),
                    LinearGradientMode.Vertical))
                {
                    g.FillRectangle(brush, rect);
                }

                // Draw border on hover
                if (isHovered)
                {
                    using (var pen = new Pen(Color.White, 3))
                    {
                        g.DrawRectangle(pen, Rectangle.Round(rect));
                    }
                }

                // Draw value on top
                if (_zoomLevel >= 0.8f && dataPoint.Value > 0) // ✅ Chỉ hiển thị giá trị > 0
                {
                    string valueText = dataPoint.Value.ToString("F0");
                    using (var font = new Font("Segoe UI", 10F * _zoomLevel, FontStyle.Bold))
                    using (var brush = new SolidBrush(Color.White))
                    {
                        var textSize = g.MeasureString(valueText, font);
                        g.DrawString(valueText, font, brush, 
                            x + (rect.Width - textSize.Width) / 2, 
                            y - textSize.Height - 5);
                    }
                }

                // Draw label
                if (_zoomLevel >= 0.6f)
                {
                    using (var font = new Font("Segoe UI", 9F * _zoomLevel))
                    using (var brush = new SolidBrush(Color.White))
                    {
                        var textSize = g.MeasureString(dataPoint.Label, font);
                        g.DrawString(dataPoint.Label, font, brush,
                            x + (rect.Width - textSize.Width) / 2,
                            chartArea.Bottom + 10);
                    }
                }
            }

            // Draw axes
            using (var pen = new Pen(Color.FromArgb(100, 100, 100), 2))
            {
                // Y-axis
                g.DrawLine(pen, chartArea.Left, chartArea.Top, chartArea.Left, chartArea.Bottom);
                // X-axis
                g.DrawLine(pen, chartArea.Left, chartArea.Bottom, chartArea.Right, chartArea.Bottom);
            }
        }

        private void DrawLineChart(Graphics g)
        {
            var chartArea = GetChartArea();
            if (_data.Count < 2) return;

            float maxValue = (float)_data.Max(d => d.Value);
            
            // ✅ FIX: Xử lý maxValue = 0
            if (maxValue <= 0)
            {
                maxValue = 1f;
            }
            
            float pointSpacing = chartArea.Width / (_data.Count - 1);

            // Draw grid
            DrawGridLines(g, chartArea, maxValue);

            // Create points array
            var points = new PointF[_data.Count];
            for (int i = 0; i < _data.Count; i++)
            {
                float x = chartArea.X + i * pointSpacing + _panOffset.X * _zoomLevel;
                float y = chartArea.Bottom - (float)(chartArea.Height * (_data[i].Value / maxValue) * _zoomLevel);
                points[i] = new PointF(x, y);
            }

            // Draw line with gradient
            using (var pen = new Pen(Color.FromArgb(100, 149, 237), 3 * _zoomLevel))
            {
                g.DrawLines(pen, points);
            }

            // Draw points
            for (int i = 0; i < points.Length; i++)
            {
                bool isHovered = _data[i] == _hoveredPoint;
                float pointSize = (isHovered ? 12 : 8) * _zoomLevel;
                
                using (var brush = new SolidBrush(isHovered ? Color.White : _data[i].Color))
                {
                    g.FillEllipse(brush, 
                        points[i].X - pointSize / 2, 
                        points[i].Y - pointSize / 2,
                        pointSize, pointSize);
                }

                if (isHovered)
                {
                    using (var pen = new Pen(Color.FromArgb(100, 149, 237), 2))
                    {
                        g.DrawEllipse(pen,
                            points[i].X - pointSize / 2 - 2,
                            points[i].Y - pointSize / 2 - 2,
                            pointSize + 4, pointSize + 4);
                    }
                }
            }

            // Draw axes
            using (var pen = new Pen(Color.FromArgb(100, 100, 100), 2))
            {
                g.DrawLine(pen, chartArea.Left, chartArea.Top, chartArea.Left, chartArea.Bottom);
                g.DrawLine(pen, chartArea.Left, chartArea.Bottom, chartArea.Right, chartArea.Bottom);
            }
        }

        private void DrawPieChart(Graphics g)
        {
            var chartArea = GetChartArea();
            int centerX = chartArea.X + chartArea.Width / 2;
            int centerY = chartArea.Y + chartArea.Height / 2;
            int radius = (int)(Math.Min(chartArea.Width, chartArea.Height) / 2 * 0.8f * _zoomLevel);

            // ✅ FIX: Đảm bảo radius hợp lệ
            if (radius < 10)
            {
                radius = 10; // Bán kính tối thiểu
            }

            float total = (float)_data.Sum(d => d.Value);
            
            // ✅ FIX: Xử lý tổng giá trị = 0
            if (total <= 0)
            {
                // Vẽ thông báo không có dữ liệu
                DrawNoDataMessage(g);
                return;
            }
            
            float startAngle = 0;

            for (int i = 0; i < _data.Count; i++)
            {
                var dataPoint = _data[i];
                float sweepAngle = (float)(360.0 * dataPoint.Value / total);
                
                // ✅ FIX: Bỏ qua slice có góc quét quá nhỏ (< 0.1 độ)
                if (sweepAngle < 0.1f)
                {
                    continue;
                }
                
                bool isHovered = dataPoint == _hoveredPoint;
                int adjustedRadius = radius + (isHovered ? 20 : 0);

                var rect = new Rectangle(
                    centerX - adjustedRadius, 
                    centerY - adjustedRadius,
                    adjustedRadius * 2, 
                    adjustedRadius * 2);

                using (var brush = new SolidBrush(dataPoint.Color))
                {
                    g.FillPie(brush, rect, startAngle, sweepAngle);
                }

                if (isHovered)
                {
                    using (var pen = new Pen(Color.White, 3))
                    {
                        g.DrawPie(pen, rect, startAngle, sweepAngle);
                    }
                }

                // Draw label - chỉ vẽ nếu slice đủ lớn (> 5%)
                if ((dataPoint.Value / total * 100) > 5)
                {
                    float labelAngle = startAngle + sweepAngle / 2;
                    float labelRadius = adjustedRadius + 40;
                    int labelX = centerX + (int)(labelRadius * Math.Cos(labelAngle * Math.PI / 180));
                    int labelY = centerY + (int)(labelRadius * Math.Sin(labelAngle * Math.PI / 180));

                    string label = $"{dataPoint.Label}\n{(dataPoint.Value / total * 100):F1}%";
                    using (var font = new Font("Segoe UI", 9F * _zoomLevel, FontStyle.Bold))
                    using (var brush = new SolidBrush(Color.White))
                    {
                        var textSize = g.MeasureString(label, font);
                        g.DrawString(label, font, brush, labelX - textSize.Width / 2, labelY - textSize.Height / 2);
                    }
                }

                startAngle += sweepAngle;
            }
        }

        private void DrawAreaChart(Graphics g)
        {
            var chartArea = GetChartArea();
            if (_data.Count < 2) return;

            float maxValue = (float)_data.Max(d => d.Value);
            
            // ✅ FIX: Xử lý maxValue = 0
            if (maxValue <= 0)
            {
                maxValue = 1f;
            }
            
            float pointSpacing = chartArea.Width / (_data.Count - 1);

            // Draw grid
            DrawGridLines(g, chartArea, maxValue);

            // Create points for area
            var points = new List<PointF>();
            
            // Add baseline start
            points.Add(new PointF(chartArea.Left, chartArea.Bottom));

            // Add data points
            for (int i = 0; i < _data.Count; i++)
            {
                float x = chartArea.X + i * pointSpacing + _panOffset.X * _zoomLevel;
                float y = chartArea.Bottom - (float)(chartArea.Height * (_data[i].Value / maxValue) * _zoomLevel);
                points.Add(new PointF(x, y));
            }

            // Add baseline end
            points.Add(new PointF(chartArea.Right, chartArea.Bottom));

            // Fill area with gradient
            using (var path = new GraphicsPath())
            {
                path.AddLines(points.ToArray());
                path.CloseFigure();

                // ✅ FIX: Đảm bảo chartArea hợp lệ trước khi tạo LinearGradientBrush
                if (chartArea.Height > 0)
                {
                    using (var brush = new LinearGradientBrush(
                        new Point(0, chartArea.Top),
                        new Point(0, chartArea.Bottom),
                        Color.FromArgb(120, 100, 149, 237),
                        Color.FromArgb(20, 100, 149, 237)))
                    {
                        g.FillPath(brush, path);
                    }
                }
            }

            // Draw line on top
            var linePoints = points.Skip(1).Take(_data.Count).ToArray();
            if (linePoints.Length > 1) // ✅ Đảm bảo có ít nhất 2 điểm để vẽ đường
            {
                using (var pen = new Pen(Color.FromArgb(100, 149, 237), 3 * _zoomLevel))
                {
                    g.DrawLines(pen, linePoints);
                }
            }

            // Draw axes
            using (var pen = new Pen(Color.FromArgb(100, 100, 100), 2))
            {
                g.DrawLine(pen, chartArea.Left, chartArea.Top, chartArea.Left, chartArea.Bottom);
                g.DrawLine(pen, chartArea.Left, chartArea.Bottom, chartArea.Right, chartArea.Bottom);
            }
        }

        private void DrawGridLines(Graphics g, Rectangle chartArea, float maxValue)
        {
            int gridLines = 5;
            using (var pen = new Pen(Color.FromArgb(40, 40, 40), 1))
            using (var font = new Font("Segoe UI", 8F))
            using (var brush = new SolidBrush(Color.FromArgb(150, 150, 150)))
            {
                for (int i = 0; i <= gridLines; i++)
                {
                    float y = chartArea.Bottom - (chartArea.Height * i / gridLines);
                    g.DrawLine(pen, chartArea.Left, y, chartArea.Right, y);
                    
                    float value = maxValue * i / gridLines;
                    g.DrawString(value.ToString("F0"), font, brush, chartArea.Left - 40, y - 10);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _tooltip?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
