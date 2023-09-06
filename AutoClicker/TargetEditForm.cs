using System;
using System.Windows.Forms;

namespace AutoClicker
{
    public partial class TargetEditForm : Form
    {
        public event EventHandler TargetDeleted;

        private Target target;
        public TargetEditForm(Target target)
        {
            InitializeComponent();
            this.target = target;

            cbTimeUnit.DataSource = Enum.GetValues(typeof(Unit));
            cbTimeUnit.SelectedIndex = (int)target.unit;
            tbSelectedTime.Text = target.nextPointTime.ToString();
            this.Text = "Target "+target.PointNumber.ToString();

            this.Location = target.Location;
        }

        private void tbSelectedTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void btOK_Click(object sender, System.EventArgs e)
        {
            if(string.IsNullOrEmpty(tbSelectedTime.Text))
            {
                MessageBox.Show("Next point inteval must have a valid value");
                return;
            }
            this.target.nextPointTime = Convert.ToInt32(tbSelectedTime.Text);
            this.target.unit = (Unit)cbTimeUnit.SelectedIndex;

            this.Close();
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            TargetDeleted?.Invoke(this, EventArgs.Empty);
            this.Close();
        }
    }
}
