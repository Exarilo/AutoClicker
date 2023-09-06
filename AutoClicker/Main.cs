using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoClicker
{
    public partial class Main : Form
    {
        private int nextPointNumber = 1;
        public Main()
        {
            InitializeComponent();
            this.TransparencyKey = this.BackColor;
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Target target = new Target(nextPointNumber);
            target.Location= new Point(50,50);
            target.BringToFront();
            panel1.Visible = false;
            nextPointNumber++;
            SetSelectedTarget(target);
            target.MouseDown += (s, ev) =>{
                SetSelectedTarget(s as Target);
                if (ev.Button == MouseButtons.Right)
                {
                    TargetEditForm targetEditForm = new TargetEditForm(s as Target);
                   
                    targetEditForm.TargetDeleted += (se, evt) =>
                    {
                        DeleteTarget(s as Target);
                    };
                    targetEditForm.ShowDialog();
                }
            };

            this.Controls.Add(target);
        }

        public void SetSelectedTarget(Target selectedTarget)
        {
            selectedTarget.IsSelected = true;
            this.Controls.OfType<Target>()?.Where(x => x != selectedTarget)?.ToList()?.ForEach(x => x.IsSelected = false);
        }

        public void DeleteTarget(Target target)
        {
            nextPointNumber--;
            this.Controls.OfType<Target>()?.Where(x => x.PointNumber > target.PointNumber)?.ToList()?.ForEach(x => x.PointNumber--);
            this.Controls.Remove(target);
        }


    }
}
