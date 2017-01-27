using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hotspot_Controller
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        Form1 f1;
        public Form4(Form caller)
        {
            f1 = caller as Form1;
            InitializeComponent();
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Form1.backColor;
            panel1.ForeColor = Form1.foreColor;
            f1.ShowNetwork();
            string ssid = Form1.GetSSID();
            string password = Form1.ReadSetting("password");
            string ADN = Form1.ReadSetting("activedatanetwork");
            string HDN = Form1.ReadSetting("activehostnetwork");
            string ip = Form1.ReadSetting("currentscopeaddress");
            if (ip == "null")
            {
                ip = Form1.ReadSetting("initialscopeaddress");
            }
            int StatusIndex = f1.statusstring.ToLower().LastIndexOf("status");
            int startOfStatusIndex = f1.statusstring.IndexOf(':', StatusIndex);
            int endOfStatusIndex = f1.statusstring.IndexOf('\n', StatusIndex);
            string status = f1.statusstring.Substring(startOfStatusIndex + 1, endOfStatusIndex - startOfStatusIndex - 1).Trim();
            
            Console.WriteLine(status);
            string noc = "Not Available";
            if (status == "Started")
            {
                Console.WriteLine("YEP");
                status = "Running";
                int nocs = f1.statusstring.LastIndexOf("Number of clients");
                int noce = f1.statusstring.IndexOf('\n', nocs);
                int nocss = f1.statusstring.IndexOf(':', nocs);
                noc = f1.statusstring.Substring(nocss + 1, noce - nocss - 1).Trim();
                Console.WriteLine("---" + noc + "---");
                label15.Text = noc;
                //Console.WriteLine(noce + "  " + f1.statusstring.Length);
                if (Convert.ToInt16(noc) >= 1)
                {


                    string clientsunparsed = f1.statusstring.Substring(noce + 1, f1.statusstring.Length - noce - 1);
                    clientsunparsed = clientsunparsed.Trim();
                    //Console.WriteLine(clientsunparsed);
                    string[] macss = clientsunparsed.Split('\n');
                    textBox1.Text = "";
                    //Console.WriteLine(macss.Length);
                    //string[] macsstrimmed;
                    /*foreach (var item in macss)
                    {
                        //Console.WriteLine(item);
                        //Console.WriteLine(item.Substring(0, item.IndexOf(" ")));
                        //textBox1.AppendText(item.Substring(0, item.IndexOf(" ")) + Environment.NewLine);
                        
                    }*/
                    string x;
                    for (int i = 0; i < macss.Length; i++)
                    {
                        //Console.WriteLine(macss[i]);
                        x = macss[i].Trim();
                        Console.WriteLine(x);
                        textBox1.AppendText(x.Substring(0, x.IndexOf(" ")) + Environment.NewLine);
                    }
                }
                //Console.WriteLine(macss[0]);
            }
            else
            {
                status = "Not running";
            }
            label9.Text = ssid;
            label10.Text = password;
            label11.Text = status;
            label12.Text = ADN;
            label13.Text = HDN;
            label14.Text = ip;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            f1.Enabled = true;
            this.Close();
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
