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

namespace Optika.Forms
{
    public partial class tovarForm : Form
    {
        public tovarForm()
        {
            InitializeComponent();
        }

        private void tovarForm_Load(object sender, EventArgs e)
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
    }
}
