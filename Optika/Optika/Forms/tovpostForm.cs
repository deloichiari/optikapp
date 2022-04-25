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
    public partial class tovpostForm : Form
    {
        public tovpostForm(string postnazv)
        {
            InitializeComponent();
            postname = postnazv;
            label1.Text += postnazv;
        }

        string postname;

        private void loaddata()
        {
            dataGridView1.Rows.Clear();
            DBconnect db = new DBconnect();
            db.openConnection();

            MySqlCommand command = new MySqlCommand("select t.nazva from tovar t, postachal p, postach_tov pt where t.id_tov=pt.id_tov and pt.id_post=(select p.id_post from postachal p where p.name_postach=@postname) group by 1;", db.getConnection());
            command.Parameters.Add("@postname", MySqlDbType.VarChar).Value = postname;
            MySqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                data.Add(new string[1]);

                data[data.Count - 1][0] = reader[0].ToString();

            }
            reader.Close();
            db.closeConnection();
            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);
        }

        private void loadcombobox()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("");
            DBconnect db = new DBconnect();
            db.openConnection();

            MySqlCommand command = new MySqlCommand("select t.nazva from tovar t left join postach_tov pt on t.id_tov = pt.id_tov where pt.id_tov is null and t.id_cat>1;", db.getConnection());
            MySqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                comboBox1.Items.Add(reader[0].ToString());
            }
            reader.Close();
            db.closeConnection();
        }

        private void tovpostForm_Load(object sender, EventArgs e)
        {
            loaddata();
            loadcombobox();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DBconnect db = new DBconnect();

            MySqlCommand cmd = new MySqlCommand("INSERT INTO `optika2`.`postach_tov` (`id_post`, `id_tov`) VALUES ((select id_post from postachal where name_postach = @np), (select id_tov from tovar where nazva = @n))", db.getConnection());
            cmd.Parameters.Add("@np", MySqlDbType.VarChar).Value = postname;
            cmd.Parameters.Add("@n", MySqlDbType.VarChar).Value = comboBox1.Text;

            db.openConnection();
            if (cmd.ExecuteNonQuery() == 1)
            {
                toolStripStatusLabel1.Text = "Товар постачатьника було додано.";
                loaddata();
                loadcombobox();
            }
            else
            {
                toolStripStatusLabel1.Text = "Товар постачальника не було додано.";
            }
            db.closeConnection();
        }
    }
}
