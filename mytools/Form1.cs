using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;


namespace mytools
{
    public partial class mytools : Form
    {

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();


        //读写配置
        public string inipath;
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public mytools()
        {
            InitializeComponent();

            //注册热键(窗体句柄,热键ID,辅助键,实键)   
            RegisterHotKey(this.Handle, 888, 1, Keys.Q);
        }

        private void getpath()
        {
            //RegistryKey hklm = Registry.LocalMachine;
            //RegistryKey run = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\mytools", false);
            //string str = (string)run.GetValue("path");
            inipath = Application.StartupPath + "\\infor.ini";
        }

        private void MaxWorkingSet()
        {
            System.Diagnostics.Process.GetCurrentProcess().MaxWorkingSet = (IntPtr)750000;
        }
        //初始化
        private void mytools_Shown(object sender, EventArgs e)
        {
            this.Hide();
            // this.WindowState = FormWindowState.Minimized;

            getpath();
            getini();
            creatpic();
            foreach (PictureBox pic in this.Controls)
            {
                pic.MouseMove += Form1_MouseDown;
            }

            MaxWorkingSet();
        }


        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MOVE = 0xF010;
        const int HTCAPTION = 0x0002;


        [DllImport("user32")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint control, Keys vk);
        //注册热键的api    
        [DllImport("user32")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //注消热键(句柄,热键ID)   
            UnregisterHotKey(this.Handle, 888);
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0312:    //这个是window消息定义的   注册的热键消息    
                    if (m.WParam.ToString().Equals("888"))  //如果是我们注册的那个热键    
                    {

                        if (this.Visible)
                        {
                            this.Hide();
                            this.WindowState = FormWindowState.Minimized;
                            MaxWorkingSet();
                        }
                        else
                        {
                            int h = Screen.PrimaryScreen.WorkingArea.Height;
                            int w = Screen.PrimaryScreen.WorkingArea.Width;
                            this.Location = new Point((w - this.Width) / 2, (h - this.Height) / 2);
                            this.WindowState = FormWindowState.Normal;
                            this.TopMost = true;
                            this.Show();
                        }
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();

            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
        //创建图标组 
        List<string> icon0 = new List<string>();
        List<string> path0 = new List<string>();
        List<string> icon1 = new List<string>();
        List<string> path1 = new List<string>();

        private void getini()
        {

            //第一排图标
            for (int i = 0; ; i++)
            {
                StringBuilder temp = new StringBuilder(500);
                StringBuilder temp1 = new StringBuilder(500);
                GetPrivateProfileString("Line0", "icon" + i, "", temp, 500, inipath);

                int has = GetPrivateProfileString("Line0", "path" + i, "", temp1, 500, inipath);

                if (has == 0) break;//结束读取
                if (!File.Exists(temp1.ToString()))
                {
                    MessageBox.Show("配置文件有误，\n第一排图标path" + i + "的文件\"" + temp1.ToString() + "\"不存在", "MyTools-OTHorizon");
                    continue;
                }
                icon0.Add(temp.ToString());
                path0.Add(temp1.ToString());
            }

            //第二排图标
            for (int i = 0; ; i++)
            {
                StringBuilder temp = new StringBuilder(500);
                StringBuilder temp1 = new StringBuilder(500);
                GetPrivateProfileString("Line1", "icon" + i, "", temp, 500, inipath);

                int has = GetPrivateProfileString("Line1", "path" + i, "", temp1, 500, inipath);

                if (has == 0) break;//结束读取

                if (!File.Exists(temp1.ToString()))
                {
                    MessageBox.Show("配置文件有误，\n第二排图标path" + i + "的文件\"" + temp1.ToString() + "\"不存在", "MyTools-OTHorizon");
                    continue;
                }
                icon1.Add(temp.ToString());
                path1.Add(temp1.ToString());
            }

        }

        private void creatpic()
        {

            //第一排
            for (int i = 0; i < icon0.Count; i++)
            {
                PictureBox pic = new PictureBox();
                pic.Name = "pic0" + i;
                pic.Tag = "0," + i;//绑定对应的序列，用于获取路径
                //判断图片是否存在,不存在则提取
                if (icon0[i].ToString() == "")
                {
                    Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(path0[i].ToString());
                    pic.Image = icon.ToBitmap();
                }
                else
                {
                    pic.Image = Image.FromFile(icon0[i].ToString());
                }
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.Width = pic.Height = 128;
                pic.Location = new Point(24 + 134 * i, 22);
                pic.MouseClick += new MouseEventHandler(pic_MouseClick);

                this.Controls.Add(pic);

            }
            //第二排
            for (int i = 0; i < icon1.Count; i++)
            {
                PictureBox pic = new PictureBox();
                pic.Name = "pic1" + i;
                pic.Tag = "1," + i;//绑定对应的序列，用于获取路径
                //判断图片是否存在,不存在则提取
                if (icon1[i].ToString() == "")
                {
                    Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(path1[i].ToString());
                    pic.Image = icon.ToBitmap();
                }
                else
                {
                    pic.Image = Image.FromFile(icon1[i].ToString());
                }
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.Width = pic.Height = 62;
                pic.Location = new Point(24 + 66 * i, 156);
                pic.MouseClick += new MouseEventHandler(pic_MouseClick);
                this.Controls.Add(pic);

            }

        }
        private void pic_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)//重新加载组件
            {

                this.Hide();
                this.WindowState = FormWindowState.Minimized;
                this.Controls.Clear();
                icon0.Clear();
                icon1.Clear();
                path0.Clear();
                path1.Clear();
                getini();
                creatpic();
                foreach (PictureBox pict in this.Controls)
                {
                    pict.MouseMove += Form1_MouseDown;
                }

                MaxWorkingSet();
            }


            PictureBox pic = (PictureBox)sender;
            if (e.Button == MouseButtons.Left)
            {

                string temp = (string)pic.Tag;
                string[] temp2 = temp.Split(',');
                int i = Convert.ToInt32(temp2[0].ToString());
                int j = Convert.ToInt32(temp2[1].ToString());
                string str = (i == 0) ? path0[j].ToString() : path1[j].ToString();//路径获取
                try
                {
                    Process.Start(str);
                }
                catch
                {
                    MessageBox.Show("文件路径错误");
                }

                this.Hide();
                this.WindowState = FormWindowState.Minimized;
                MaxWorkingSet();

            }
        }

        private void mytools_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MaxWorkingSet();
        }


    }
}
