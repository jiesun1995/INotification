namespace INotification
{
    partial class frm_Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Main));
            nfc_State = new NotifyIcon(components);
            cms_Menu = new ContextMenuStrip(components);
            tsmi_Exit = new ToolStripMenuItem();
            cms_Menu.SuspendLayout();
            SuspendLayout();
            // 
            // nfc_State
            // 
            nfc_State.ContextMenuStrip = cms_Menu;
            nfc_State.Icon = (Icon)resources.GetObject("nfc_State.Icon");
            nfc_State.Text = "我的订阅";
            nfc_State.Visible = true;
            nfc_State.MouseClick += nfc_State_MouseClick;
            // 
            // cms_Menu
            // 
            cms_Menu.Items.AddRange(new ToolStripItem[] { tsmi_Exit });
            cms_Menu.Name = "contextMenuStrip1";
            cms_Menu.Size = new Size(101, 26);
            // 
            // tsmi_Exit
            // 
            tsmi_Exit.Name = "tsmi_Exit";
            tsmi_Exit.Size = new Size(100, 22);
            tsmi_Exit.Text = "退出";
            tsmi_Exit.Click += tsmi_Exit_Click;
            // 
            // frm_Main
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(800, 450);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frm_Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "我的订阅";
            FormClosing += frm_Main_FormClosing;
            Load += frm_Main_Load;
            cms_Menu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private NotifyIcon nfc_State;
        private ContextMenuStrip cms_Menu;
        private ToolStripMenuItem tsmi_Exit;
    }
}
