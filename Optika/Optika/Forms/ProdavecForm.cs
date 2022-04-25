using MySql.Data.MySqlClient;
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
    public partial class ProdavecForm : Form
    {
        public ProdavecForm(spivrob sp)
        {
            InitializeComponent();
            this.sp = sp;
            label1.Text = sp.getpib();
        }

        spivrob sp = new spivrob();

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Autoriz f1 = new Autoriz();
            f1.Show();
            Hide();
        }

        private void orderStoryToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            loaddata();
        }

        private void loaddata()
        {
            dataGridView1.Rows.Clear();
            DBconnect Db = new DBconnect();
            Db.openConnection();

            MySqlCommand cmd = new MySqlCommand("select t.nazva, c.cat_name, t.price, t.tovcount from tovar t, categoria c where t.id_cat=c.id_cat group by 1;", Db.getConnection());
            MySqlDataReader reader = cmd.ExecuteReader();
            List<string[]> data = new List<string[]>();

            while (reader.Read())
            {
                data.Add(new string[4]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();
            }
            reader.Close();
            Db.closeConnection();
            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);
        }

        private void клієнтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clientsprod clientsprod = new Clientsprod();
            clientsprod.Show();
        }
    }
}
