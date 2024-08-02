using INotificationClient.Contracts.Dtos.MessageDtos;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using Windows.Foundation.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace INotificationClient
{
    public partial class frm_Main : Form
    {
        private readonly HubConnection _hubConnection;
        public frm_Main(HubConnection hubConnection)
        {
            _hubConnection = hubConnection;
            InitializeComponent();
        }

        private void frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)//���û�����������Ͻ�X��ť��(Alt + F4)ʱ ����           
            {
                e.Cancel = true;
                this.ShowInTaskbar = false;
                this.nfc_State.Icon = this.Icon;
                this.Hide();
            }
        }

        private async void  frm_Main_Load(object sender, EventArgs e)
        {
            await _hubConnection.StartAsync();
            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                ToastArguments args = ToastArguments.Parse(toastArgs.Argument);
                // ��ȡ�κ��û�����
                ValueSet userInput = toastArgs.UserInput;

                BeginInvoke(new Action(delegate
                {
                    // TODO: UI�̵߳Ĳ���
                    MessageBox.Show("Toast������������������: " + toastArgs.Argument);
                }));
            };

            _hubConnection.On<string>("ReceiveMessage", message =>
            {
                var dto =JsonConvert.DeserializeObject<ToastDto>(message);
                ToastHelper.ShowToast(dto?.Message, dto?.IconAddress);
            });
            
            if (this.WindowState == FormWindowState.Normal && this.Visible == true)
            {
                this.nfc_State.Visible = true;//��֪ͨ����ʾForm��Icon
                this.WindowState = FormWindowState.Minimized;
                this.Visible = false;
                this.ShowInTaskbar = false;//ʹForm��������������ʾ
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

        private async void button1_Click(object sender, EventArgs e)
        {
            var state = _hubConnection.State;
            await _hubConnection.InvokeAsync("SendMessage","test");
        }
    }
}
