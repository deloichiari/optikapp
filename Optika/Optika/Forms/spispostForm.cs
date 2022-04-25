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
    public partial class spispostForm : Form
    {
        public spispostForm(List<tovarcls> tovarcls)
        {
            InitializeComponent();
            label1.Visible = false;
            foreach (var i in tovarcls)
            {
                dataGridView1.Rows.Add(i.gettovname(), i.getcount());
            }
        }

        public spispostForm(int i)
        {
            InitializeComponent();
            label1.Text = "Поставка №" + i;
            loaddata(i);
        }

        public void loaddata(int i)
        {
            DBconnect Db = new DBconnect();
            Db.openConnection();

            MySqlCommand cmd = new MySqlCommand("select t.nazva, pt.tovcount from postavki_tovar pt, tovar t where pt.id_tov = t.id_tov and pt.id_postavki = @idpos;", Db.getConnection());
            cmd.Parameters.Add("@idpos", MySqlDbType.Int16).Value = i;
            MySqlDataReader reader = cmd.ExecuteReader();
            List<string[]> data = new List<string[]>();

            while (reader.Read())
            {
                data.Add(new string[2]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
            }
            reader.Close();
            Db.closeConnection();
            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);
        }
    }
}
