using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.IO.Compression;
using Ionic.Zip;
using System.Runtime.InteropServices;
using System.Reflection;
using IWshRuntimeLibrary;
namespace LauncherSiberia
{

    public partial class Form1 : Form
    {
        string path_to = String.Empty;
        WebClient client_new = new WebClient();
        SynchronizationContext context;
        String c = String.Empty;
        String re = String.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //масштаб формы (бета)
            Graphics g = CreateGraphics();
            int W = (int)(Screen.PrimaryScreen.Bounds.Width / g.DpiX * 64);
            int H = (int)(Screen.PrimaryScreen.Bounds.Height / g.DpiY * 71);

            this.Width = W;
            this.Height = H;

            Properties.Resources.background.SetResolution(W, H);
            Bitmap b = new Bitmap(Properties.Resources.background);
            b.SetResolution(W, H);
            BackgroundImage = b;
            fsProgressBar1.Visible = false;
            fsProgressBar2.Visible = false;
            ShowIcon = true;
           

            if (System.IO.File.Exists(Properties.Settings.Default.path_siberia + "\\" + "Client.exe") == false && System.IO.File.Exists(Application.StartupPath+"\\"+"Client.exe") == false)
            {
                button1.Image = Properties.Resources.skachat as Bitmap;
                
            }

            if (System.IO.File.Exists(Properties.Settings.Default.path_siberia + "\\" + "Client.exe") == true || System.IO.File.Exists(Application.StartupPath + "\\" + "Client.exe") == true)
            {
                button1.Image = Properties.Resources.igra as Bitmap;
                if (System.IO.File.Exists("remv.roms"))
                {
                    StreamReader rem = new StreamReader("remv.roms");
                    re = rem.ReadToEnd();
                    if (System.IO.File.Exists(re + "\\" + "LauncherSiberia.exe"))
                    {
                        System.IO.File.Delete(re + "\\" + "LauncherSiberia.exe");
                    }
                }
                if (System.IO.File.Exists("instpath.roms"))
                {
                    StreamReader cur_p = new StreamReader("instpath.roms");
                    c = cur_p.ReadToEnd();
                }

                if (System.IO.File.Exists(c + "//Siberia.zip"))
                    System.IO.File.Delete(c + "//Siberia.zip");


                if (System.IO.File.Exists(c + "\\" + "RuneDev.ini") == false)
                {
                    WebClient Rundev = new WebClient();
                    Rundev.DownloadFileAsync(new Uri("http://rom-siberia.ru/download/RuneDev.txt"), c + "\\" + "RuneDev.ini");
                    Thread.Sleep(500);
                }
                Thread.Sleep(1000);
                string localRunDev = System.IO.File.ReadAllLines(c + "\\" + "RuneDev.ini").Skip(4).First();
                string ServerV = "http://rom-siberia.ru/download/severip.txt";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ServerV);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string updaterSVersion = reader.ReadLine();
                    if (updaterSVersion != localRunDev)
                    {
                        Thread.Sleep(500);
                        WebClient Rundev = new WebClient();
                        Rundev.DownloadFileAsync(new Uri("http://rom-siberia.ru/download/RuneDev.txt"), c + "\\" + "RuneDev.ini");
                        Thread.Sleep(500);
                    }

                }

                if (System.IO.File.Exists(c + "\\" + "siberia.ini") == false)
                {
                    WebClient zeroini = new WebClient();
                    zeroini.DownloadFileAsync(new Uri("http://rom-siberia.ru/download/0000.txt"), c + "\\" + "siberia.ini");
                    Thread.Sleep(500);

                }
                Thread.Sleep(500);
                VersionChecker verChecker = new VersionChecker();
                System.Net.WebClient wc = new System.Net.WebClient();
                WebClient serv = new System.Net.WebClient();

                serv.DownloadFileAsync(new Uri("http://rom-siberia.ru/download/siberia.txt"), c + "\\" + "srv.ini");
                Thread.Sleep(500);

                StreamReader srvr = new StreamReader(c + "\\" + "srv.ini");
                String ServerVersion = srvr.ReadToEnd();
                srvr.Dispose();
                StreamReader srv = new StreamReader(c + "\\" + "siberia.ini");
                String localVersion = srv.ReadToEnd();
                srv.Dispose();
                label2.Text = localVersion;

                if (verChecker.NewVersionExists(localVersion, ServerVersion))
                {
                    button1.Enabled = false;
                    if (System.IO.File.Exists(c + "\\" + "data.exe"))
                    {
                        System.IO.File.Delete(c + "\\" + "data.exe");
                    }
                    if (Directory.Exists(c + "\\" + "data"))
                    {
                        Directory.Delete(c + "\\" + "data", true);
                    }
                    if (System.IO.File.Exists(c + "\\" + "Client.exe") == true)
                    {
                        button1.Image = Properties.Resources.obnov as Bitmap;
                    }
                    WebClient client = new WebClient();
                    client.DownloadProgressChanged +=
                        new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                    client.DownloadFileAsync(new Uri("http://rom-siberia.ru/download/data.rar"), c + "\\" + "data.exe");
                    Thread.Sleep(500);
                    WebClient clientq = new WebClient();
                    clientq.DownloadFileAsync(new Uri("http://rom-siberia.ru/download/siberia.txt"), c + "\\" + "siberia.ini");
                    Thread.Sleep(300);
                    StreamReader sr = new StreamReader(c + "\\" + "siberia.ini");
                    String localVersion1 = sr.ReadToEnd();
                    sr.Dispose();
                    Thread.Sleep(300);
                    label2.Text = localVersion1;
                }

            }
            
        }
        
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            label4.Visible = true;
            label4.Text = (Math.Round(e.BytesReceived * 0.000001 / time_pass,2)).ToString() + " Мб/сек";
            fsProgressBar2.Visible = true;
            button1.Enabled = false;
            fsProgressBar2.Value = e.ProgressPercentage;
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Process.Start(c + "\\" + "data.exe", "/s -d -o+" + Properties.Settings.Default.path_siberia);
            Thread.Sleep(300);
            button1.Image = Properties.Resources.igra as Bitmap;
            button1.Enabled = true;
            fsProgressBar2.Hide();
        }

        void zip_ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            switch (e.EventType)
            {
                case ZipProgressEventType.Extracting_AfterExtractEntry:
                    if (context != null)
                        context.Send(
                            (o) =>
                            {
                                fsProgressBar1.Value = e.EntriesExtracted;
                                label3.Text = e.CurrentEntry.ToString();
                            },
                            null
                            );
                    break;
            }
            
        }

        void ExtractAsync(string to, ZipFile zip)
        {
            zip.ExtractAll(to, ExtractExistingFileAction.OverwriteSilently);
            zip.Dispose();

            MessageBox.Show("Установка завершена");
            
            Properties.Settings.Default.Save();
            WshShell shell = new WshShell();
            string shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Launcher Siberia RoM.lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
            shortcut.Description = "Siberia Launcher";
            shortcut.WorkingDirectory = path_to;
            shortcut.TargetPath =  path_to + "//LauncherSiberia.exe";
            shortcut.Save();
            Application.Exit();
        }



        void client_finish(object sender, AsyncCompletedEventArgs e)
        {
            label3.Visible = true;
            fsProgressBar1.Show();
            ZipFile inst = ZipFile.Read(path_to + "\\" + "Siberia.zip");
            fsProgressBar1.MaxValue = inst.Count();
            inst.ExtractProgress += zip_ExtractProgress;
            context = SynchronizationContext.Current;
            new Thread(
                delegate ()
                {
                    ExtractAsync(path_to, inst);
                }).Start();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(Properties.Settings.Default.path_siberia + "\\" + "Client.exe") == true )
            {
                Process.Start(Properties.Settings.Default.path_siberia + "\\" + "Client.exe", "NoCheckVersion");
                Application.Exit();
            }
            else if (System.IO.File.Exists(Application.StartupPath + "\\" + "Client.exe") == true)
            {
                Process.Start(Application.StartupPath + "\\" + "Client.exe", "NoCheckVersion");
                Application.Exit();
            }
            else if (System.IO.File.Exists(Properties.Settings.Default.path_siberia + "\\" + "Client.exe") == false && System.IO.File.Exists(Application.StartupPath + "\\" + "Client.exe") == false)
            {
                MessageBox.Show("Внимание! Для установки Runes of Magic Siberia требуется 25 Гб на жестком диске, после установки размер требуемой памяти сократится до 13 Гб!");
                FolderBrowserDialog dialog = new FolderBrowserDialog();

                fsProgressBar1.Visible = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    path_to = dialog.SelectedPath;
                    System.IO.File.WriteAllText(path_to + @"//instpath.roms", path_to);
                    Properties.Settings.Default.path_siberia = path_to;
                    System.IO.File.WriteAllText(path_to + @"//remv.roms", Application.StartupPath);
                    client_new = new WebClient();
                    client_new.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                    client_new.DownloadFileCompleted += new AsyncCompletedEventHandler(client_finish);
                    button1.Image = Properties.Resources.skachat_n as Bitmap;
                    timer1.Start();
                    client_new.DownloadFile(new Uri("http://rom-siberia.ru/download/Siberia.zip"), path_to + @"//Siberia.zip");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.FlatAppearance.BorderSize = 0;
            Process.Start("https://vk.com/topic-109238671_33065618");
        }
            
        private void button3_Click(object sender, EventArgs e)
        {
            igra.Focus();

            button3.FlatAppearance.BorderSize = 0;
            Process.Start("https://vk.com/siberia_rom_pvp");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            igra.Focus();
            Process.Start("https://vk.com/topic-109238671_33029885");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (client_new.IsBusy)
            {
                if (MessageBox.Show("Прервать загрузку?", "Идет загрузка?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Application.Exit();
            }
            else
                Application.Exit();
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            button4.Image = Properties.Resources.almaz_s as Bitmap;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            button4.Image = Properties.Resources.almaz as Bitmap;
            igra.Focus();
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {

            button3.Image = Properties.Resources.gruppa_s as Bitmap;

        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.Image = Properties.Resources.gruppa as Bitmap;
            igra.Focus();
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.Image = Properties.Resources.refer_s as Bitmap;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.Image = Properties.Resources.refer as Bitmap;
            igra.Focus();
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(Properties.Settings.Default.path_siberia + "\\" + "Client.exe") == false && System.IO.File.Exists(Application.StartupPath + "\\" + "Client.exe") == false)
            {
                button1.Image = Properties.Resources.skachat_s as Bitmap;
                button1.FlatAppearance.BorderSize = 0;
                button1.FlatStyle = FlatStyle.Flat;
            }
            if (System.IO.File.Exists(Properties.Settings.Default.path_siberia + "\\" + "Client.exe") == true || System.IO.File.Exists(Application.StartupPath + "\\" + "Client.exe") == true)
            {
                button1.Image = Properties.Resources.igra_s as Bitmap;
                button1.FlatAppearance.BorderSize = 0;
                button1.FlatStyle = FlatStyle.Flat;
            }
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(Properties.Settings.Default.path_siberia + "\\" + "Client.exe") == false && System.IO.File.Exists(Application.StartupPath + "\\" + "Client.exe") == false)
            {
                button1.Image = Properties.Resources.skachat as Bitmap;
                button1.FlatAppearance.BorderSize = 0;
                button1.FlatStyle = FlatStyle.Flat;
            }
            if (System.IO.File.Exists(Properties.Settings.Default.path_siberia + "\\" + "Client.exe") == true || System.IO.File.Exists(Application.StartupPath + "\\" + "Client.exe") == true)
            {
                button1.Image = Properties.Resources.igra as Bitmap;
                igra.Focus();
                button1.FlatAppearance.BorderSize = 0;
                button1.FlatStyle = FlatStyle.Flat;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Process.Start("http://vk.com/hellish_k");
            Process.Start("http://vk.com/if_else_human");
        }

        private void загрузитьОбновлениеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(Properties.Settings.Default.path_siberia + "\\" + "data.exe"))
            {
                System.IO.File.Delete(Properties.Settings.Default.path_siberia + "\\" + "data.exe");
            }
            if (Directory.Exists(Properties.Settings.Default.path_siberia + "\\" + "data"))
            {
                Directory.Delete(Properties.Settings.Default.path_siberia + "\\" + "data", true);
            }
            if (System.IO.File.Exists(Properties.Settings.Default.path_siberia + "\\" + "Client.exe") == true || System.IO.File.Exists("//Client.exe") == true)
            {
                button1.Image = Properties.Resources.obnov as Bitmap;
            }
            WebClient client = new WebClient();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
            client.DownloadFileAsync(new Uri("http://rom-siberia.ru/download/data.exe"), Properties.Settings.Default.path_siberia + "\\" + "data.exe");
            button1.Image = Properties.Resources.igra as Bitmap;
        }

        private void vIPToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Process.Start("https://vk.com/topic-109238671_33085148");
        }

        private void покупкаАлмазовToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Process.Start("https://vk.com/topic-109238671_33029885");
        }

        private void дополнительныеУслугиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Process.Start("https://vk.com/topic-109238671_33088032");
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            base.Capture = false;
            Message m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref m);
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            igra.Focus();
        }

        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            if (System.IO.File.Exists(Properties.Settings.Default.path_siberia + "\\" + "Client.exe") == false && System.IO.File.Exists(Application.StartupPath + "\\" + "Client.exe") == false)
            {
                button1.Image = Properties.Resources.skachat_n as Bitmap;
                button1.FlatAppearance.BorderSize = 0;
                button1.FlatStyle = FlatStyle.Flat;
            }
            if (System.IO.File.Exists(Properties.Settings.Default.path_siberia + "\\" + "Client.exe") == true || System.IO.File.Exists(Application.StartupPath + "\\" + "Client.exe") == true)
            {
                button1.Image = Properties.Resources.igra_n as Bitmap;
                button1.FlatAppearance.BorderSize = 0;
                button1.FlatStyle = FlatStyle.Flat;
            }

        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            button3.Image = Properties.Resources.gruppa_n as Bitmap;
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            button2.Image = Properties.Resources.refer_n as Bitmap;
        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            button4.Image = Properties.Resources.almaz_n as Bitmap;
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            if (System.IO.File.Exists(Properties.Settings.Default.path_siberia + "\\" + "Client.exe") == false && System.IO.File.Exists(Application.StartupPath + "\\" + "Client.exe") == false)
            {
                button1.Image = Properties.Resources.skachat as Bitmap;
            }
            if (System.IO.File.Exists(Properties.Settings.Default.path_siberia + "\\" + "Client.exe") == true || System.IO.File.Exists(Application.StartupPath + "\\" + "Client.exe") == true)
            {
                button1.Image = Properties.Resources.igra as Bitmap;
            }
        }
        int time_pass = 1;
        private void timer1_Tick(object sender, EventArgs e)
        {
            time_pass++;
        }
    }

}

