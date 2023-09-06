using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System;

namespace AutoClicker
{
    public enum Unit
    {
        ms,
        s,
        min
    }
    public class Target : PictureBox
    {
        private Point mouseOffset;
        private bool isMouseDown = false;
        private bool isSelected = true;
        private int pointNumber;
        public int nextPointTime { get; set; } =1;
        public Unit unit { get; set; } = Unit.s;

        public int PointNumber
        {
            get => pointNumber;
            set
            {

                pointNumber = value;
                this.Refresh();
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                if (value)
                    this.BackColor = Color.LightBlue;
                else
                    this.BackColor = Color.Transparent;
            }
        }

        public Target(int pointNumber)
        {
            this.MouseDown += Target_MouseDown;
            this.MouseMove += Target_MouseMove;
            this.MouseUp += Target_MouseUp;
            this.pointNumber = pointNumber;
        }



        private void Target_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                mouseOffset = e.Location;
            }
        }

        private void Target_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Point newLocation = this.PointToScreen(e.Location);
                newLocation.Offset(-mouseOffset.X, -mouseOffset.Y);
                this.Location = this.Parent.PointToClient(newLocation);
            }
        }

        private void Target_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        public int GetNextPointInMs()
        {
            if (this.unit.ToString() == "ms")
                return nextPointTime;
            else if (this.unit.ToString() == "s")
                return nextPointTime * 1000;
            else if (this.unit.ToString() == "min")
                return nextPointTime * 1000 * 60;
            else
                throw new Exception("Unexpected error");
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int circleDiameter = 30;
            Rectangle circleRect = new Rectangle((this.Width - circleDiameter) / 2, (this.Height - circleDiameter) / 2, circleDiameter, circleDiameter);
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(circleRect);
            this.Region = new Region(path);
            Graphics g = e.Graphics;

            g.SmoothingMode = SmoothingMode.AntiAlias;

            Brush brush = new SolidBrush(Color.Transparent);
            Pen pen = new Pen(Color.Black, 2);
            g.FillEllipse(brush, circleRect);
            g.DrawEllipse(pen, circleRect);

            string labelText = pointNumber.ToString();
            Font labelFont = new Font("Microsoft Sans Serif", 16, FontStyle.Bold);
            SizeF textSize = g.MeasureString(labelText, labelFont);
            PointF textLocation = new PointF((this.Width - textSize.Width) / 2, (this.Height - textSize.Height) / 2);
            g.DrawString(labelText, labelFont, Brushes.Black, textLocation);
        }
    }
}
