using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Optika
{
    public partial class Autoriz : Form
    {
        public Autoriz()
        {
            InitializeComponent();
            sp = new spivrob();
        }

        spivrob sp = new spivrob();

        private void button1_Click(object sender, EventArgs e)
        {
            sp = new spivrob(loginBox.Text, passwordBox.Text);
            if (sp.autoriz())
            {
                if (sp.getpos() == "Продавець")
                {
                    ProdavecForm f2 = new ProdavecForm(sp);
                    f2.Show();
                    Hide();
                }
                else if (sp.getpos() == "Менеджер")
                {
                    MenegerForm mf = new MenegerForm(sp);
                    mf.Show();
                    Hide();
                }
                else MessageBox.Show("Даний користувач не має доступу до системи");
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
