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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            //f1= (Form1)this.Owner;
            //this.Owner = (Form1)f1;
        }
        Form1 f1;
        public Form2(Form caller)
        {
            f1 = caller as Form1;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }



        private void button2_Click(object sender, EventArgs e)
        {
            var chars1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars1 = new char[8];
            var random1 = new Random();

            for (int i = 0; i < stringChars1.Length; i++)
            {
                stringChars1[i] = chars1[random1.Next(chars1.Length)];
            }

            string ssid = new String(stringChars1);

            var chars2 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            var stringChars2 = new Char[10];
            var random2 = new Random();
            for (int i = 0; i < stringChars2.Length; i++)
            {
                stringChars2[i] = chars2[random1.Next(chars2.Length)];
            }

            string key = new String(stringChars2);

            textBox1.Text = ssid;
            textBox2.Text = key;
            textBox3.Text = key;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == textBox3.Text)
            {
                if (textBox1.TextLength == 0 || textBox2.TextLength == 0)
                {
                    textBox2.PasswordChar = '\u25cf';
                    textBox3.PasswordChar = '\u25cf';
                    f1.CreateHost();
                    this.Update();
                    System.Threading.Thread.Sleep(500);
                    if (f1.ShowNetwork().ToLower().Replace(" ", "").Contains("notstarted"))
                    {
                        MessageBox.Show("Hosted Network has not been created.Please try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        f1.Enabled = true;
                        //f1.button5.PerformClick();
                        button1.PerformClick();

                    }
                }
                else
                {
                    if (textBox3.TextLength < 8)
                    {
                        MessageBox.Show("Password length must be greater than 8 characters!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        f1.CreateHost(textBox1.Text, textBox2.Text);
                        this.Update();
                        System.Threading.Thread.Sleep(500);
                        if (f1.ShowNetwork().ToLower().Replace(" ", "").Contains("notstarted"))
                        {
                            MessageBox.Show("Hosted Network has not been created.Please try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            f1.Enabled = true;
                            //f1.button5.PerformClick();
                            button1.PerformClick();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Passwords do not match", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '\0';
            textBox3.PasswordChar = '\0';
            f1.Enabled = true;
            this.Close();
        }
    }
}
