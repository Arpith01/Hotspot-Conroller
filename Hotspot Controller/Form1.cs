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
            /*if (!checkWifiStat())
            {
                MessageBox.Show("Please turn on your Wi-Fi!","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                this.Close();
            }*/
            InitializeComponent();

            //this.Update();
            //System.Threading.Thread.Sleep(1000);


        }
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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StopHstNet();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            restart();
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
            createNew.textBox1.Text = ssid;
            createNew.textBox2.Text = key;
            createNew.textBox3.Text = key;
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
                MessageBox.Show("Error writing app settings");
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
                    string ssid = ReadSetting("ssid");
                    string password = textBox2.Text;
                    StopHstNet();
                    CreateHost(ssid, password);
                    textBox2.PasswordChar = '\0';
                    textBox3.PasswordChar = '\0';
                    this.panel2.SendToBack();
                    this.panel2.Hide();
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
            label4.BackColor = System.Drawing.Color.FromArgb(32,32,32);
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            label4.BackColor = System.Drawing.Color.Black;
        }

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            label5.BackColor = System.Drawing.Color.FromArgb(32, 32, 32);
        }

        private void label6_MouseEnter(object sender, EventArgs e)
        {
            label6.BackColor = System.Drawing.Color.FromArgb(32, 32, 32);
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            label6.BackColor = System.Drawing.Color.Black;
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            label5.BackColor = System.Drawing.Color.Black;
        }



    }
}
