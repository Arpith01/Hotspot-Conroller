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
    public partial class MessageBoxForm : Form
    {
        int maxTime,count;
        string message;
        string messageType;
        string heading;
        Color fColor;
        Color bColor;
        public MessageBoxForm()
        {
            InitializeComponent();
        }
        public MessageBoxForm(String message,string heading,int timeInSeconds,Form parent,String type,Color fc,Color bc)
        {
            this.message = message;
            messageType = type;
            //this.Owner = parent as Form1;
            fColor = fc;
            bColor = bc;
            maxTime = timeInSeconds;
            this.heading = heading;
            count = 0;
            InitializeComponent();
            this.Show();
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
        private void timer1_Tick(object sender, EventArgs e)
        {
            count++;
            if(count == maxTime)
            {
                this.Dispose();
                this.Close();
            }
        }

        private void MessageBoxForm_Load(object sender, EventArgs e)
        {
            label1.Text = message;
            timer1.Start();
            this.Update();
            panel1.BackColor = bColor;
            panel1.ForeColor = fColor;
            label2.Text = heading;
            //ComponentResourceManager rm = new ComponentResourceManager();
            //pictureBox1.Image = rm.GetObject("warningimg",);
            switch (messageType)
            {
                case "info":
                    pictureBox1.Image = Hotspot_Controller.Properties.Resources._1485461303_messagebox_info;
                    break;
                case "warning":
                    pictureBox1.Image = Hotspot_Controller.Properties.Resources._1485461213_messagebox_critical;
                    break;
                case "error":
                    pictureBox1.Image = Hotspot_Controller.Properties.Resources._1485461253_messagebox_critical;
                    break;
                default:
                    break;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
    }
}
