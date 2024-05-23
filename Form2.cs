using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Media;

namespace AwesomeNotepad
{
    public partial class Form2 : Form
    {
        private SoundPlayer sp = new SoundPlayer();
        public Form2()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Process.Start("https://t.me/awesome_lime");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/AwesomeLime/AwesomeNotepad");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.Lang == 1)
            {
                MessageBox.Show("Awesome Notepad\nВерсия: 1.1.0 Release\nВетвь: Full\nСборка: 1100.atxtrelease\nДата сборки: 23.05.2024 2:00\nAwesomeLime // 2024", "ATXT - Информация", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("Awesome Notepad\nVersion: 1.1.0 Release\nBranch: Full\nBuild: 1100.atxtrelease\nBuild date: 23.05.2024 2:00\nAwesomeLime // 2024", "ATXT - Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void Sound_Tick(object sender, EventArgs e)
        {
            sp.Stop();
            Sound.Stop();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            sp.Stream = Properties.Resources.NotCoolSound;
            sp.Play();
            Sound.Start();
            new Form3().ShowDialog();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.Lang == 1)
            {
                this.Text = "О программе Awesome Notepad";
                label2.Text = "Простой, лёгкий и\r\nфункциональный блокнот!\r\n";
                label1.Text = "Версия 1.1.0\nAwesomeLime // 2024";
                button2.Text = "Информация о сборке";
            }
            else
            {
                this.Text = "About Awesome Notepad";
                label2.Text = "Simple, lightweight, and\nfunctional notepad!\n";
                label1.Text = "Version 1.1.0\nAwesomeLime // 2024";
                button2.Text = "About the build";
            }
        }
    }
}
