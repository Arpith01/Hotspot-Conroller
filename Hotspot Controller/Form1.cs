using System;
using System.Configuration.Assemblies;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using NETCONLib;
using Microsoft.Win32;

namespace Hotspot_Controller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //string stat = ShowNetwork();
            ////stat.Replace(" ", "");
            //textBox1.Text = stat;
            //int y = stat.ToLower().LastIndexOf("status");
            //int z = stat.IndexOf('\n', y);
            //label1.Text = "Status : " + y + "  " + z + stat.LastIndexOf("\n");
            InitializeComponent();
            
            //this.Update();
            //System.Threading.Thread.Sleep(1000);


        }
        public static Color backColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        public static Color foreColor = System.Drawing.Color.FromArgb(255, 255, 255);
        private bool checkWifiStat()
        {
            ProcessStartInfo psi = new ProcessStartInfo("C:\\Windows\\System32\\netsh.exe");
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.Arguments = "interface show interface";
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            var validate = Process.Start(psi);
            string output = validate.StandardOutput.ReadToEnd();
            string error = validate.StandardError.ReadToEnd();
            if (output.ToLower().Replace(" ", "").Contains("disconnecteddedicatedwi-fi"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool validateCompat()
        {
            ProcessStartInfo psi = new ProcessStartInfo("C:\\Windows\\System32\\netsh.exe");
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.Arguments = "interface show interface";
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            var validate = Process.Start(psi);
            string output = validate.StandardOutput.ReadToEnd();
            string error = validate.StandardError.ReadToEnd();
            validate.WaitForExit();
            if (output.Replace(" ", "").ToLower().Contains("enabledconnecteddedicatedwi-fi") || output.Replace(" ", "").ToLower().Contains("enableddisconnecteddedicatedwi-fi"))
            {
                psi.Arguments = "wlan show drivers";
                var validate2 = Process.Start(psi);
                string out1 = validate2.StandardOutput.ReadToEnd();
                string err1 = validate2.StandardError.ReadToEnd();
                if (out1.Replace(" ", "").ToLower().Contains("hostednetworksupported:yes"))
                {
                    return true;
                }
                return false;
            }
            else
            {
                MessageBox.Show("Your Wireless adapter is disabled or absent. Please recheck", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            StartHstNet();
            MessageBoxForm m = new MessageBoxForm("Hosted Network started", "Response", 3, this, "info",foreColor,backColor);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StopHstNet();
            MessageBoxForm m = new MessageBoxForm("Hosted Network stopped", "Response", 3, this, "info",foreColor,backColor);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            restart();
            MessageBoxForm m = new MessageBoxForm("Hosted Network restarted", "Response", 5, this, "info",foreColor,backColor);
        }
        public void restart()
        {
            ProcessStartInfo psi = new ProcessStartInfo("C:\\Windows\\System32\\netsh.exe");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.Arguments = "wlan stop hostednetwork";
            psi.Verb = "runas";
            var netshpr = Process.Start(psi);
            //Console.WriteLine(netshpr.StandardOutput.ReadToEnd());
            textBox1.AppendText(netshpr.StandardOutput.ReadToEnd() + Environment.NewLine);
            netshpr.WaitForExit();
            psi.Arguments = "wlan start hostednetwork";
            netshpr = Process.Start(psi);
            textBox1.AppendText(netshpr.StandardOutput.ReadToEnd() + Environment.NewLine);
            netshpr.WaitForExit();
            label1.Text = ShowNetwork();
        }
        public string statusstring;
        public string ShowNetwork()
        {
            ProcessStartInfo psi = new ProcessStartInfo("C:\\Windows\\System32\\netsh.exe");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.Arguments = "wlan show hostednetwork";
            psi.Verb = "runas";
            var netshpr = Process.Start(psi);
            //Console.WriteLine(netshpr.StandardOutput.ReadToEnd());

            statusstring = netshpr.StandardOutput.ReadToEnd();
            //status = status.Replace(" ", "");
            textBox1.AppendText(statusstring + Environment.NewLine);
            int startOfStatusIndex = statusstring.ToLower().LastIndexOf("status");
            int endOfStatusIndex = statusstring.IndexOf('\n', startOfStatusIndex);
            //Console.WriteLine(status.Substring(startOfStatusIndex, endOfStatusIndex - startOfStatusIndex).Replace(" ", ""));
            return statusstring.Substring(startOfStatusIndex, endOfStatusIndex - startOfStatusIndex).Replace("        ", "");
        }
        public static string GetSSID()
        {
            string ssid = null;
            ProcessStartInfo psi = new ProcessStartInfo("C:\\Windows\\System32\\netsh.exe");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.Arguments = "wlan show hostednetwork";
            psi.Verb = "runas";
            var netshpr = Process.Start(psi);
            string outString = netshpr.StandardOutput.ReadToEnd();
            string[] outstringarray = outString.Split('\n');
            for(int i = 0 ; i < outstringarray.Length;i++)
            {
                outstringarray[i] = outstringarray[i].Trim();
                if(outstringarray[i].ToLower().Contains("ssid"))
                {
                    outstringarray[i].Replace(" ",string.Empty);
                    int indexofhypen = outstringarray[i].IndexOf(':');
                    ssid = outstringarray[i].Substring(indexofhypen + 1);
                    break;
                }
            }
            if (ssid == null)
            {
                ssid = "null";
                MessageBox.Show("Create network first!","Not found",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            return ssid;

        }
        public void StartHstNet()
        {
            ProcessStartInfo psi = new ProcessStartInfo("C:\\Windows\\System32\\netsh.exe");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.Arguments = "wlan start hostednetwork";
            psi.Verb = "runas";
            var netshpr = Process.Start(psi);
            //Console.WriteLine(netshpr.StandardOutput.ReadToEnd());
            textBox1.AppendText(netshpr.StandardOutput.ReadToEnd() + Environment.NewLine);
            netshpr.WaitForExit();
            label1.Text = ShowNetwork();
            if (label1.Text.ToLower().Contains("not"))
            {
                MessageBox.Show("Trouble starting hostednetwork. Please check if wifi is turned-off", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void StopHstNet()
        {
            ProcessStartInfo psi = new ProcessStartInfo("C:\\Windows\\System32\\netsh.exe");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.Arguments = "wlan stop hostednetwork";
            psi.Verb = "runas";
            var netshpr = Process.Start(psi);
            //Console.WriteLine(netshpr.StandardOutput.ReadToEnd());
            textBox1.AppendText(netshpr.StandardOutput.ReadToEnd() + Environment.NewLine);
            netshpr.WaitForExit();
            label1.Text = ShowNetwork();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("ncpa.cpl");
            }
            catch
            {
                MessageBox.Show("Open network and Sharaing center and change adapter Settings");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            backColor = parseColorFromString(Form1.ReadSetting("backColor"));
            //MessageBox.Show(color.ToString());
            panel1.BackColor = backColor;
            panel4.BackColor = backColor;
            foreColor = parseColorFromString(ReadSetting("foreColor"));
            panel1.ForeColor = foreColor;
            panel4.ForeColor = foreColor;
            if (!validateCompat())
            {
                //Console.WriteLine("ASFAFASf");
                MessageBox.Show("HostedNetwork ain't Supported. I am sorry", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            else
            {
                label1.Text = ShowNetwork();
                string runcount = ReadSetting("runcount");
                //Console.WriteLine(runcount);
                //Console.WriteLine(ReadSetting("initialscopeaddress"));
                //Console.WriteLine(ReadSetting("initialscopebackupaddress"));
                //Console.WriteLine(ReadSetting("initialstandalonedhcpaddress"));
                if (runcount == "0")
                {
                    //Console.WriteLine("ENTEREDDDDDDDDDDDdd");
                    runcount = "1";
                    string scopeaddress = Registry.GetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\SharedAccess\\Parameters", "ScopeAddress", "Null").ToString();
                    string scopeaddressbackup = Registry.GetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\SharedAccess\\Parameters", "ScopeAddressBackup", "Null").ToString();
                    string standalaoneshcpaddress = Registry.GetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\SharedAccess\\Parameters", "StandaloneDhcpAddress", "Null").ToString();
                    //Console.WriteLine(scopeaddress + "    " + scopeaddressbackup + "   " + standalaoneshcpaddress);
                    AddUpdateAppSettings("runcount", runcount);
                    AddUpdateAppSettings("initialscopeaddress", scopeaddress);
                    AddUpdateAppSettings("initialscopebackupaddress", scopeaddressbackup);
                    AddUpdateAppSettings("initialstandalonedhcpaddress", standalaoneshcpaddress);
                    //Console.WriteLine(ReadSetting("initialscopeaddress"));
                    //Console.WriteLine(ReadSetting("initialscopebackupaddress"));
                    //Console.WriteLine(ReadSetting("initialstandalonedhcpaddress"));
                }
                else
                {
                    string scopeaddress = Registry.GetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\SharedAccess\\Parameters", "ScopeAddress", "Null").ToString();
                    string scopeaddressbackup = Registry.GetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\SharedAccess\\Parameters", "ScopeAddressBackup", "Null").ToString();
                    string standalaoneshcpaddress = Registry.GetValue("HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\SharedAccess\\Parameters", "StandaloneDhcpAddress", "Null").ToString();
                    AddUpdateAppSettings("currentscopeaddress", scopeaddress);
                    AddUpdateAppSettings("currentscopeaddressbackup", scopeaddressbackup);
                    AddUpdateAppSettings("currentdhcpaddress", standalaoneshcpaddress);
                }

                //if (!ShowNetwork().ToLower().Contains("started"))
                //{
                //    button7.PerformClick();
                //}
            }

        }

        /*private void button5_Click(object sender, EventArgs e)
        {
            //ProcessStartInfo psi = new ProcessStartInfo("C:\\Windows\\System32\\netsh.exe");
            //psi.Arguments = String.Format("wlan set hostednetwork mode = allow ssid = \"{0}\" key =\"{1}\" ", textBox2.Text, textBox3.Text);
            //psi.CreateNoWindow = true;
            //psi.UseShellExecute = false;
            //psi.RedirectStandardError = true;
            //psi.RedirectStandardOutput = true;
            //textBox3.PasswordChar = '\u25cf';
            //var createhst = Process.Start(psi);
            //textBox1.Text = createhst.StandardOutput.ReadToEnd();
            //textBox1.Text += createhst.StandardError.ReadToEnd();
            //createhst.WaitForExit();
            //StartHstNet();
            //createhst.WaitForExit();
            if(textBox2.TextLength==0||textBox3.TextLength ==0)
            {
                CreateHost();
            }
            else
            {
                if (textBox3.TextLength<8)
                {
                    MessageBox.Show("Password length must be greater than 8 characters!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    CreateHost(textBox2.Text, textBox3.Text);
                }
            }
        }*/
        public void CreateHost(string ssid = "Sample!", string key = "1234567890")
        {
            Console.WriteLine(ssid.ToString());
            //createNew.textBox1.Text = ssid;
            //createNew.textBox2.Text = key;
            //createNew.textBox3.Text = key;
            ProcessStartInfo psi = new ProcessStartInfo("C:\\Windows\\System32\\netsh.exe");
            psi.Arguments = String.Format("wlan set hostednetwork mode = allow ssid = \"{0}\" key =\"{1}\" ", ssid, key);
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            //createNew.textBox2.PasswordChar = '\u25cf';
            //createNew.textBox3.PasswordChar = '\u25cf';
            var createhst = Process.Start(psi);
            string output = createhst.StandardOutput.ReadToEnd();
            string error = createhst.StandardError.ReadToEnd();
            textBox1.Text = output;
            textBox1.Text += error;
            createhst.WaitForExit();
            AddUpdateAppSettings("ssid", ssid);
            AddUpdateAppSettings("password", key);
            StartHstNet();
        }

        private void button6_Click(object sender, EventArgs e)
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

            createNew.textBox1.Text = ssid;
            createNew.textBox2.Text = key;
            createNew.textBox3.Text = key;

        }
        Form2 createNew;
        Form3 activateConn;
        Form4 statusfrm;
        private void button7_Click(object sender, EventArgs e)
        {
            //Form createNew = new Form();
            createNew = new Form2(this);
            createNew.Owner = this;
            this.Enabled = false;
            //this.Hide();
            createNew.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            activateConn = new Form3(this);
            activateConn.Owner = this;
            this.Enabled = false;
            activateConn.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            statusfrm = new Form4(this);
            statusfrm.Owner = this;
            this.Enabled = false;
            statusfrm.Show();
        }
        public static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                    //Console.WriteLine("Done");
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                //Console.WriteLine("SAved");
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                MessageBox.Show("Error occured while updating app settings");
            }
        }
        public static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key];
                //Console.WriteLine(result);
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
                return "-1";
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            this.panel2.Show();
            this.panel2.BringToFront();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "";
            this.textBox3.Text = "";
            textBox2.PasswordChar = '\0';
            textBox3.PasswordChar = '\0';
            this.panel2.SendToBack();
            this.panel2.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '\u25cf';
            textBox3.PasswordChar = '\u25cf';
            if (textBox3.Text == textBox2.Text)
            {
                if (textBox2.TextLength > 8 && textBox3.TextLength > 8)
                {

                    string ssid = GetSSID();
                    Console.WriteLine(ssid);
                    if (ssid == "null")
                    {
                        return;
                    }
                    AddUpdateAppSettings("ssid", ssid);
                    string password = textBox2.Text;
                    StopHstNet();
                    CreateHost(ssid, password);
                    textBox2.PasswordChar = '\0';
                    textBox3.PasswordChar = '\0';
                    this.panel2.SendToBack();
                    this.panel2.Hide();
                    MessageBoxForm m = new MessageBoxForm("Password updated", "Response", 5, this, "info",foreColor,backColor);
                }
                else
                {
                    MessageBox.Show("Password must contain more than 8 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox2.PasswordChar = '\0';
                    textBox3.PasswordChar = '\0';
                    return;
                }
            }
            else
            {
                MessageBox.Show("Passwords not equal", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.PasswordChar = '\0';
                textBox3.PasswordChar = '\0';
                return;
            }

        }

        private void label4_MouseEnter(object sender, EventArgs e)
        {
            label4.BackColor = System.Drawing.Color.FromArgb(120,120,120);
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            label4.BackColor = backColor;
        }

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            label5.BackColor = System.Drawing.Color.FromArgb(120,120,120);
        }

        private void label6_MouseEnter(object sender, EventArgs e)
        {
            label6.BackColor = System.Drawing.Color.FromArgb(120,120,120);
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            label6.BackColor = backColor;
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            label5.BackColor = backColor;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            MessageBoxForm m = new MessageBoxForm("Wha","Information",5,this,"warning",foreColor,backColor);
            m.Show(this);
        }

        private void button5_Click_2(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            Form1.backColor = colorDialog1.Color;
            AddUpdateAppSettings("backColor", backColor.R.ToString()+" "+backColor.G.ToString()+" "+backColor.B.ToString());
            int colorSum = backColor.R + backColor.G + backColor.B;
            foreColor = colorSum > 383 ? Color.Black : Color.White;
            AddUpdateAppSettings("foreColor", foreColor.R.ToString() + " " + foreColor.G.ToString() + " " + foreColor.B.ToString());
            MessageBox.Show("Changes appear in the next restart" + backColor.ToString());
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.BackColor = backColor;
        }

        public Color parseColorFromString(string colorString)
        {
            int r, g, b;
            string[] colors = colorString.Split();
            r = Convert.ToInt32(colors[0]);
            g = Convert.ToInt32(colors[1]);
            b = Convert.ToInt32(colors[2]);
            Color c = System.Drawing.Color.FromArgb(r, g, b);
            return c;
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            panel3.SendToBack();
            panel3.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            panel3.BringToFront();
            panel3.Show();
        }
        private Boolean clickedColor = false;
        private Color bc, fc;
        private void button11_Click(object sender, EventArgs e)
        {
            clickedColor = true;
            bc = (sender as Button).BackColor;
            //MessageBox.Show(c.R.ToString() + " " + c.G.ToString() + " " + c.B.ToString());
            fc = bc.R+bc.G+bc.B > 383 ? Color.Black : Color.White;
            //panel1.BackColor = c;
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            if(clickedColor)
            {
                AddUpdateAppSettings("backColor", bc.R.ToString() + " " + bc.G.ToString() + " " + bc.B.ToString());
                AddUpdateAppSettings("foreColor", fc.R.ToString() + " " + fc.G.ToString() + " " + fc.B.ToString());
                MessageBoxForm m = new MessageBoxForm("Changes reflect from the next restart :)", "Notice", 5, this, "warning", foreColor, backColor);
            }
            panel3.SendToBack();
            panel3.Hide();
        }

        private void button5_Click_3(object sender, EventArgs e)
        {
            this.Dispose();
            Application.Exit();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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
