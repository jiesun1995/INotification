using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace INotification
{
    public partial class frm_Main : Form
    {
        public frm_Main()
        {
            InitializeComponent();
        }

        private void frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (e.CloseReason == CloseReason.UserClosing)//当用户点击窗体右上角X按钮或(Alt + F4)时 发生           
            {
                e.Cancel = true;
                this.ShowInTaskbar = false;
                this.nfc_State.Icon = this.Icon;
                this.Hide();
            }
        }

        private void frm_Main_Load(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal && this.Visible == true)
            {
                this.nfc_State.Visible = true;//在通知区显示Form的Icon
                this.WindowState = FormWindowState.Minimized;
                this.Visible = false;
                this.ShowInTaskbar = false;//使Form不在任务栏上显示

            }
        }

        private void tsmi_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void nfc_State_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
            }
            else 
            {
                cms_Menu.Show();
            }

        }
    }
}
