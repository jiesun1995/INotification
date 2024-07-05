using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.Foundation.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace INotification
{
    public partial class frm_Main : Form
    {
        private readonly HubConnection _hubConnection;
        public frm_Main(HubConnection hubConnection)
        {
            _hubConnection = hubConnection;
            InitializeComponent();
            _hubConnection.StartAsync().Wait();
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
            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                ToastArguments args = ToastArguments.Parse(toastArgs.Argument);
                // 获取任何用户输入
                ValueSet userInput = toastArgs.UserInput;

                BeginInvoke(new Action(delegate
                {
                    // TODO: UI线程的操作
                    MessageBox.Show("Toast被激活（点击），参数是: " + toastArgs.Argument);
                }));
            };

            _hubConnection.On<string>("ReceiveMessage", message =>
            {
                var toastContentBuilder = new ToastContentBuilder()
                    .AddText($"{message}")
                     .AddToastInput(new ToastSelectionBox("time")
                     {
                         DefaultSelectionBoxItemId = "0",
                         Items =
                                {
                                    new ToastSelectionBoxItem("0", "现在完成"),
                                    new ToastSelectionBoxItem("1", "推迟1分钟"),
                                    new ToastSelectionBoxItem("5", "推迟5分钟"),
                                    new ToastSelectionBoxItem("10", "推迟10分钟")
                                }
                     })
                    .AddButton(new ToastButton()
                    .SetContent("提交"));
                toastContentBuilder
                .Show();
            });
            
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
            if (e.Button == MouseButtons.Left)
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
            else
            {
                cms_Menu.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var state = _hubConnection.State;
            _hubConnection.InvokeAsync("SendMessage","test").Wait();
        }
    }
}
