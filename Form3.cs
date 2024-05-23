using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AwesomeNotepad
{
    public partial class Form3 : Form
    {
        private SoundPlayer sp = new SoundPlayer();
        public Form3()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Sound.Stop();
            sp.Stream = Properties.Resources.NotCoolSound;
            sp.Play();
            Sound.Start();
        }

        private void Sound_Tick(object sender, EventArgs e)
        {
            sp.Stop();
            Sound.Stop();
        }
    }
}
