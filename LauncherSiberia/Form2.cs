/*using System;
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

namespace LauncherSiberia
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public void screen_mode_check()
        {
            if (checkBox1.Checked == true)
                checkBox2.Checked = false;
            if (checkBox2.Checked == true)
                checkBox1.Checked = false;
        }
        public void ui_scale_check()
        {
            if (checkBox3.Checked == false)
                hScrollBar1.Enabled = false;
            if (checkBox3.Checked == true)
                hScrollBar1.Enabled = true;
        }

        class res
        {
            public string res_w;
            public string res_h;
            public res(string w, string h)
            {
                res_w = w;
                res_h = h;
            }
        };

        private void button1_Click(object sender, EventArgs e)
        {

            res ob1 = new res("960", "540"), ob2 = new res("1024", "768"), ob3 = new res("1280", "768"), ob4 = new res("1280", "1024"), ob5 = new res("1440", "900"),
                ob6 = new res("1400", "1050"), ob7 = new res("1600", "900"), ob8 = new res("1600", "1200"), ob9 = new res("1920", "1080"), ob10 = new res("1920", "1200");
            res active_res = new res("","");
            if (comboBox1.SelectedIndex == 1) active_res = ob1;
            if (comboBox1.SelectedIndex == 2) active_res = ob2;
            if (comboBox1.SelectedIndex == 3) active_res = ob3;
            if (comboBox1.SelectedIndex == 4) active_res = ob4;
            if (comboBox1.SelectedIndex == 5) active_res = ob5;
            if (comboBox1.SelectedIndex == 6) active_res = ob6;
            if (comboBox1.SelectedIndex == 7) active_res = ob7;
            if (comboBox1.SelectedIndex == 8) active_res = ob8;
            if (comboBox1.SelectedIndex == 9) active_res = ob9;
            if (comboBox1.SelectedIndex == 10) active_res = ob10;
            string sett = string.Empty;
            using (System.IO.StreamReader reader = System.IO.File.OpenText("client.config.ini"))
            {
                sett = reader.ReadToEnd();
            }
            sett = sett.Replace("windowed width=1920", "windowed width="+ active_res.res_w);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("client.config.ini"))
            {
                file.Write(sett);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 cl = new Form2();
            cl.Close();
        }
    }
}

*/