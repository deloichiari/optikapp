using Optika.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Optika
{
    public partial class MenegerForm : Form
    {
        public MenegerForm()
        {
            InitializeComponent();
            
        }
        public MenegerForm(spivrob sp)
        {
            InitializeComponent();
            this.sp = sp;
            label1.Text = sp.getpib();
        }

        spivrob sp = new spivrob();

        private void MenegerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            spivrobForm sf = new spivrobForm();
            sf.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            postavkiForm pf = new postavkiForm();
            pf.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            postachalForm pf = new postachalForm();
            pf.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tovarForm tf = new tovarForm();
            tf.Show();
        }
    }
}
