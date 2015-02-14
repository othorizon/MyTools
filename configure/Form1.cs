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

namespace configure
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //读写配置
        static string starpath = Application.StartupPath;
        public string inipath = starpath + "\\infor.ini";
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);


        private void Form1_Load(object sender, EventArgs e)
        {
            getini();
            checkregister();
        }

        private void checkregister()
        {
            RegistryKey hklm = Registry.CurrentUser;
            RegistryKey run = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false);
            //未设置开机启动
            if (run.GetValue("mytools") == null)
                checkreg.Checked = false;
            else
                checkreg.Checked = true;

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
                if (temp.ToString() == "") break;
                GetPrivateProfileString("Line0", "path" + i, "", temp1, 500, inipath);
                if (temp1.ToString() == "")
                {
                    MessageBox.Show("配置文件有误，path" + i + "不存在");
                    continue;
                }
                icon0.Add(temp.ToString());
                path0.Add(temp1.ToString());
                listBox1.Items.Add(temp1.ToString());

            }

            //第二排图标
            for (int i = 0; ; i++)
            {
                StringBuilder temp = new StringBuilder(500);
                StringBuilder temp1 = new StringBuilder(500);
                GetPrivateProfileString("Line1", "icon" + i, "", temp, 500, inipath);
             //   if (temp.ToString() == "") break;
                GetPrivateProfileString("Line1", "path" + i, "", temp1, 500, inipath);
                if (temp1.ToString() == "")
                {
                    MessageBox.Show("配置文件有误，path" + i + "不存在");
                    break;
                }
                icon1.Add(temp.ToString());
                path1.Add(temp1.ToString());
                listBox2.Items.Add(temp1.ToString());
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                int i = listBox1.SelectedIndex;
                listBox1.Items[i] = file.FileName;
                path0[i] = file.FileName;
            }
            pictureBox1_DoubleClick(sender, e);
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                int i = listBox2.SelectedIndex;
                listBox2.Items[i] = file.FileName;
                path1[i] = file.FileName;
            }
            pictureBox1_DoubleClick(sender, e);
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Title = "选择图标";
            file.Filter = "图片文件(*.png,*.jpg,*.jpeg,*.gif,*.bmp)|*.jpg;*.gif;*.bmp;*.png;*.jpeg";
            if (file.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox1.Image = Image.FromFile(file.FileName);
                }
                catch
                {
                    MessageBox.Show("图片错误");
                    return;
                }
                if (listBox1.SelectedIndex >= 0)
                    icon0[listBox1.SelectedIndex] = file.FileName;
                else if (listBox2.SelectedIndex >= 0)
                    icon1[listBox2.SelectedIndex] = file.FileName;
                else
                    MessageBox.Show("请选择一个路径");

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Title = "选择一个程序";
            if (file.ShowDialog() == DialogResult.OK)
            {
                OpenFileDialog file2 = new OpenFileDialog();
                file2.Title = "选择程序图片";
                file2.Filter = "图片文件(*.png,*.jpg,*.jpeg,*.gif,*.bmp,*.ico)|*.jpg;*.gif;*.bmp;*.png;*.jpeg;*.ico";
                if (file2.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pictureBox1.Image = Image.FromFile(file2.FileName);
                    }
                    catch
                    {
                        MessageBox.Show("图片错误,添加失败");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("没有选择图标，将自动提取程序图标，效果会非常模糊");
                }
                listBox1.Items.Add(file.FileName);
                path0.Add(file.FileName);
                icon0.Add(file2.FileName);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Title = "选择一个程序";
            if (file.ShowDialog() == DialogResult.OK)
            {
                OpenFileDialog file2 = new OpenFileDialog();
                file2.Title = "选择程序图片";
                file2.Filter = "图片文件(*.png,*.jpg,*.jpeg,*.gif,*.bmp,*.ico)|*.jpg;*.gif;*.bmp;*.png;*.jpeg;*.ico";
                if (file2.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pictureBox1.Image = Image.FromFile(file2.FileName);
                    }
                    catch
                    {
                        MessageBox.Show("图片错误,添加失败");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("没有选择图标，将自动提取程序图标，效果会非常模糊");
                }
                listBox2.Items.Add(file.FileName);
                path1.Add(file.FileName);
                icon1.Add(file2.FileName);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                int i = listBox1.SelectedIndex;
                listBox1.Items.RemoveAt(i);
                icon0.RemoveAt(i);
                path0.RemoveAt(i);
            }
            else
                MessageBox.Show("请选则第一排的一个项目");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                int i = listBox2.SelectedIndex;
                listBox2.Items.RemoveAt(i);
                icon1.RemoveAt(i);
                path1.RemoveAt(i);
            }
            else
                MessageBox.Show("请选则第二排的一个项目");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int i = listBox1.SelectedIndex;
            if (i == -1)
            {
                MessageBox.Show("请选择第一排的一个项");
                return;
            }
            object temp = listBox1.Items[i];
            listBox1.Items.RemoveAt(i);
            listBox1.Items.Insert(i - 1, temp);
            listBox1.SelectedIndex = i - 1;
            //swap
            string str = icon0[i - 1];
            icon0[i - 1] = icon0[i];
            icon0[i] = str;
            str = path0[i - 1];
            path0[i - 1] = path0[i];
            path0[i] = str;

        }

        private void button8_Click(object sender, EventArgs e)
        {
            int i = listBox1.SelectedIndex;
            if (i == -1)
            {
                MessageBox.Show("请选择第一排的一个项");
                return;
            }
            object temp = listBox1.Items[i];
            listBox1.Items.RemoveAt(i);
            listBox1.Items.Insert(i + 1, temp);
            listBox1.SelectedIndex = i + 1;
            //swap
            string str = icon0[i + 1];
            icon0[i + 1] = icon0[i];
            icon0[i] = str;
            str = path0[i + 1];
            path0[i + 1] = path0[i];
            path0[i] = str;
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (!listBox2.Focused)
            {
                listBox2.SelectedIndex = -1;
                if (listBox1.SelectedIndex >= 0)
                    label2.Text = listBox1.Items[listBox1.SelectedIndex].ToString();
                else
                    label2.Text = "";

            }
            pictureBox1.Image = null;
            if (listBox1.SelectedIndex >= 0)
            {
                string str = icon0[listBox1.SelectedIndex];
                try
                {
                    pictureBox1.Image = Image.FromFile(str);
                }
                catch
                {
                    MessageBox.Show("图片路径错误");
                }
            }
        }

        private void listBox2_Click(object sender, EventArgs e)
        {
            if (!listBox1.Focused)
            {
                listBox1.SelectedIndex = -1;
                if (listBox2.SelectedIndex >= 0)
                    label2.Text = listBox2.Items[listBox2.SelectedIndex].ToString();
                else
                    label2.Text = "";
            }
            pictureBox1.Image = null;
            if (listBox2.SelectedIndex >= 0)
            {
                string str = icon1[listBox2.SelectedIndex];
                try
                {
                    pictureBox1.Image = Image.FromFile(str);
                }
                catch
                {
                    MessageBox.Show("图片路径错误");
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            int i = listBox2.SelectedIndex;
            if (i == -1)
            {
                MessageBox.Show("请选择第二排的一个项");
                return;
            }
            object temp = listBox2.Items[i];
            listBox2.Items.RemoveAt(i);
            listBox2.Items.Insert(i - 1, temp);
            listBox2.SelectedIndex = i - 1;
            //swap
            string str = icon1[i - 1];
            icon1[i - 1] = icon1[i];
            icon1[i] = str;
            str = path1[i - 1];
            path1[i - 1] = path1[i];
            path1[i] = str;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int i = listBox2.SelectedIndex;
            if (i == -1)
            {
                MessageBox.Show("请选择第二排的一个项");
                return;
            }
            object temp = listBox2.Items[i];
            listBox2.Items.RemoveAt(i);
            listBox2.Items.Insert(i + 1, temp);
            listBox2.SelectedIndex = i + 1;
            //swap
            string str = icon1[i + 1];
            icon1[i + 1] = icon1[i];
            icon1[i] = str;
            str = path1[i + 1];
            path1[i + 1] = path1[i];
            path1[i] = str;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                FileStream fs = new FileStream(inipath, FileMode.Create);
                fs.Close();
                for (int i = 0; i < icon0.Count; i++)
                {
                    WritePrivateProfileString("Line0", "icon" + i, icon0[i].ToString(), inipath);
                    WritePrivateProfileString("Line0", "path" + i, path0[i].ToString(), inipath);
                }
                for (int i = 0; i < icon1.Count; i++)
                {
                    WritePrivateProfileString("Line1", "icon" + i, icon1[i].ToString(), inipath);
                    WritePrivateProfileString("Line1", "path" + i, path1[i].ToString(), inipath);
                }
                MessageBox.Show("保存成功");
            }
            catch
            {
                MessageBox.Show("保存失败");
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Process.Start(inipath);
        }

        private void checkreg_CheckedChanged(object sender, EventArgs e)
        {
            if (checkreg.Checked == true)
                Register();
            else
                unregister();

        }

        private void unregister()
        {
            RegistryKey HKCU = Registry.CurrentUser;
            RegistryKey Run = HKCU.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            try
            {
                Run.DeleteValue("mytools");
            }
            catch
            {
                MessageBox.Show("取消自动启动失败");
                checkreg.Checked = true;
            }
            HKCU.Close();
        }

        private void Register()
        {

            RegistryKey HKCU = Registry.CurrentUser;
            RegistryKey Run = HKCU.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            try
            {
                Run.SetValue("mytools", starpath + "\\mytools.exe");
            }
            catch
            {
                MessageBox.Show("自动启动注册失败");
                checkreg.Checked = false;
            }
            HKCU.Close();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkreg.Checked == false)
            {
                DialogResult result = MessageBox.Show("你没有勾选开机自动启动，推荐勾选该项目，\n不然也没有快捷启动的意义了，软件内存占用很小请放心。\n       是否设为开机自启?", "注意", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel)
                    e.Cancel = true;

                if (result == DialogResult.Yes)
                    Register();

            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {


            if (e.Button == MouseButtons.Right)
            {
                if (MessageBox.Show("是否删除图标?", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    if (listBox1.SelectedIndex >= 0)
                        icon0[listBox1.SelectedIndex] = "";
                    else if (listBox2.SelectedIndex >= 0)
                        icon1[listBox2.SelectedIndex] = "";
                    else
                    {
                        MessageBox.Show("请先选择一个文件路径");
                        return;
                    }

                    pictureBox1.Image = null;

                }

            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Process.Start("http://gentlelife.lofter.com/post/290dea_dd561c");
        }



    }
}
