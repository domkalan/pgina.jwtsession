using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pGina.Plugin.JWTSession
{
    public partial class Configuration : Form
    {
        public Configuration()
        {
            InitializeComponent();
            SettingsToUi();
        }

        public void SettingsToUi()
        {
            Loginserver_textBox.Text = Settings.Store.Loginserver;
            LoginserverPwd_textBox.Text = Settings.Store.ChangePassword;
            LoginserverSession_textBox.Text = Settings.Store.Session;
            StandardJsonLogin_Checkbox.Checked = Settings.Store.StandardJsonLogin;
            
        }

        private void Form_Load(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            toolTip1.AutoPopDelay = 10000;
            toolTip1.InitialDelay = 0;
            toolTip1.ReshowDelay = 0;
            toolTip1.ShowAlways = true;

            toolTip1.SetToolTip(this.Loginserver_textBox, "The Authentication server address");
        }

        public bool UiToSettings()
        {
            if (String.IsNullOrEmpty(Loginserver_textBox.Text))
            {
                MessageBox.Show(this, "Loginserver is empty", "Loginserver", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            int limit = 0;
            try
            {
                
            }
            catch (FormatException)
            {
                MessageBox.Show("Time limit must be a positive integer.");
                return false;
            }

            if (limit < 0)
            {
                MessageBox.Show("Time limit must be a positive integer.");
                return false;
            }

            Settings.Store.Loginserver = Loginserver_textBox.Text;
            Settings.Store.ChangePassword = LoginserverPwd_textBox.Text;
            Settings.Store.Session = LoginserverSession_textBox.Text;
            Settings.Store.StandardJsonLogin = StandardJsonLogin_Checkbox.Checked;

            return true;
        }

        private void Btn_Save(object sender, EventArgs e)
        {
            if (UiToSettings())
                this.Close();
        }

        private void Btn_Cancel(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btn_help(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://github.com/domkalan/pgina.jwtsession#README");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Loginserver_label_Click(object sender, EventArgs e)
        {

        }
    }
}
