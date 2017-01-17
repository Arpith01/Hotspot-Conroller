using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using NETCONLib;

namespace Hotspot_Controller
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        Form1 f1;
        public Form3(Form caller)
        {
            f1 = caller as Form1;
            InitializeComponent();
        }
        private void Form3_Load(object sender, EventArgs e)
        {

            NetSharingManager ns = new NetSharingManager();
            //Console.WriteLine(ns.ToString());

            //NETCONLib.INetConnection f;
            foreach (NETCONLib.INetConnection c in ns.EnumEveryConnection)
            {
                //Console.WriteLine(c);
                NETCONLib.INetConnectionProps p = ns.get_NetConnectionProps(c);
                NETCONLib.INetSharingConfiguration ncs = ns.get_INetSharingConfigurationForINetConnection(c);
                //Console.WriteLine(p.DeviceName + "\t" + p.Name + p.MediaType);
                //Console.WriteLine(p.Status+"\t"+p.Characteristics+"\n");
                //Console.WriteLine(ncs.SharingConnectionType + ncs.SharingEnabled.ToString() + ncs.InternetFirewallEnabled.ToString());
                //ncs.DisableSharing();
                //if(p.Name.ToLower().Contains("wi"))
                //{
                //    ncs.EnableSharing(ncs.SharingConnectionType);
                //}
                //if(p.Name.ToLower().Contains("local area"))
                //{
                //    ncs.EnableSharing((tagSHARINGCONNECTIONTYPE)1);
                //    //Console.WriteLine((tagSHARINGCONNECTIONTYPE)0);
                //    //Console.WriteLine((tagSHARINGCONNECTIONTYPE)1);
                //}
                if (p.DeviceName.ToLower().Contains("microsoft hosted network virtual adapter"))
                {

                }
                else
                {
                    comboBox1.Items.Add(p.Name);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f1.Enabled = true;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (changeip >= 1)
            {
                if (textBox1.TextLength == 0 || textBox2.TextLength == 0 || textBox4.TextLength == 0 || textBox4.TextLength == 0)
                {
                    string ipaddress = Form1.ReadSetting("initialscopeaddress");
                    Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\SharedAccess\\Parameters", "ScopeAddress", ipaddress, RegistryValueKind.String);
                    //Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\SharedAccess\\Parameters", "StandaloneDhcpAddress", "192.168.101.1", RegistryValueKind.String);
                    MessageBox.Show("IP address set to default: " + ipaddress, "Inforamataion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Form1.AddUpdateAppSettings("currentscopeaddress", ipaddress);
                }
                else
                {
                    string ipaddress = textBox1.Text + "." + textBox2.Text + "." + textBox3.Text + "." + textBox4.Text;
                    Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\SharedAccess\\Parameters", "ScopeAddress", ipaddress, RegistryValueKind.String);
                    //Registry.SetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\SharedAccess\\Parameters", "StandaloneDhcpAddress", ipaddress, RegistryValueKind.String);
                    Form1.AddUpdateAppSettings("currentscopeaddress", ipaddress);
                }

            }
            NetSharingManager ns = new NetSharingManager();
            string conn = comboBox1.SelectedItem.ToString();
            int count = 0;
            foreach (INetConnection c in ns.EnumEveryConnection)
            {
                INetSharingConfiguration nscd = ns.get_INetSharingConfigurationForINetConnection(c);
                nscd.DisableSharing();
            }
            foreach (NETCONLib.INetConnection c in ns.EnumEveryConnection)
            {
                //Console.WriteLine(c);
                INetConnectionProps p = ns.get_NetConnectionProps(c);
                INetSharingConfiguration nsc = ns.get_INetSharingConfigurationForINetConnection(c);

                if (p.DeviceName.ToLower().Contains("microsoft hosted network virtual adapter"))
                {
                    nsc.EnableSharing((tagSHARINGCONNECTIONTYPE)1);
                    count++;
                    Form1.AddUpdateAppSettings("activehostnetwork", p.Name);
                    if (count == 2)
                    {
                        break;
                    }
                }
                else if (p.Name == conn)
                {
                    nsc.EnableSharing((tagSHARINGCONNECTIONTYPE)0);
                    Form1.AddUpdateAppSettings("activedatanetwork", p.Name);
                    count++;
                    if (count == 2)
                    {
                        break;
                    }
                }

            }
            this.Update();
            System.Threading.Thread.Sleep(500);
            button2.PerformClick();
        }
        int changeip = 0;
        private string validateIP(string s)
        {
            if (s.Length != 0)
            {
                if (Convert.ToInt16(s) > 255)
                {
                    MessageBox.Show("Invalid value. Entry should be less than 255","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return "";
                }
                return s;
            }
            else
            {
                return "";
            }
        }
        private void txtHomePhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || Char.IsControl(e.KeyChar)) //The  character represents a backspace
            {
                e.Handled = false; //Do not reject the input
            }
            else
            {
                e.Handled = true; //Reject the input
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = validateIP(textBox1.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = validateIP(textBox2.Text);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.Text = validateIP(textBox3.Text);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textBox4.Text = validateIP(textBox4.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            changeip++;
            button3.Hide();
            textBox1.Show();
            textBox2.Show();
            textBox3.Show();
            textBox4.Show();
            label2.Show();
            label3.Show();
            label4.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Show();
        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void element_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }


    }
}
