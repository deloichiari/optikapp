using MySql.Data.MySqlClient;
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
    public partial class postavkiForm : Form
    {
        public postavkiForm()
        {
            InitializeComponent();
        }

        List<tovarcls> tovarcls = new List<tovarcls>();

        public void loaddata()
        {
            dataGridView1.Rows.Clear();
            DBconnect Db = new DBconnect();
            Db.openConnection();

            MySqlCommand cmd = new MySqlCommand("select pp.id_postavki, p.name_postach, pp.p_date from postavki pp, postachal p where pp.id_post = p.id_post;", Db.getConnection());
            MySqlDataReader reader = cmd.ExecuteReader();
            List<string[]> data = new List<string[]>();

            while (reader.Read())
            {
                data.Add(new string[3]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
            }
            reader.Close();
            Db.closeConnection();
            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);
        }

        public void loadcombobox()
        {
            comboBox1.Items.Add("");
            DBconnect db = new DBconnect();
            db.openConnection();

            MySqlCommand command = new MySqlCommand("select p.name_postach from postachal p;", db.getConnection());
            MySqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();

            while (reader.Read())
            {
                data.Add(new string[1]);

                data[data.Count - 1][0] = reader[0].ToString();
                comboBox1.Items.Add(data[data.Count - 1][0]);
            }
            reader.Close();
            db.closeConnection();
        }

        private void postavkiForm_Load(object sender, EventArgs e)
        {
            loadcombobox();
            loaddata();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox2.Items.Add("");
            comboBox1.Enabled = false;
            DBconnect db = new DBconnect();
            db.openConnection();

            MySqlCommand command = new MySqlCommand("select t.nazva from tovar t, postachal p, postach_tov pt where t.id_tov=pt.id_tov and pt.id_post=(select p.id_post from postachal p where p.name_postach=@postname) group by 1;", db.getConnection());
            command.Parameters.Add("@postname", MySqlDbType.VarChar).Value = comboBox1.Text;
            MySqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();

            while (reader.Read())
            {
                data.Add(new string[1]);

                data[data.Count - 1][0] = reader[0].ToString();
                comboBox2.Items.Add(data[data.Count - 1][0]);
            }
            reader.Close();
            db.closeConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(comboBox2.Text != "" && textBox1.Text != "")
            {
                tovarcls tcs = new tovarcls(comboBox2.Text, Convert.ToInt16(textBox1.Text));
                tovarcls.Add(tcs);
                textBox1.Text = "";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            spispostForm sf = new spispostForm(tovarcls);
            sf.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tovarcls.Count == 0 && comboBox1.Text =="")
            { MessageBox.Show("Ви не можете внести пустий запис"); }
            else 
            {
                DBconnect db = new DBconnect();
                MySqlCommand cmd = new MySqlCommand("Insert into postavki (`id_post`, `p_date`) values ((select id_post from postachal where name_postach = @npos), @dtim)", db.getConnection());
                cmd.Parameters.Add("@npos", MySqlDbType.VarChar).Value = comboBox1.Text;
                cmd.Parameters.Add("@dtim", MySqlDbType.DateTime).Value = DateTime.Now;

                db.openConnection();

                if (cmd.ExecuteNonQuery() == 1)
                {
                    foreach (var i in tovarcls)
                    {
                        cmd = new MySqlCommand("insert into postavki_tovar (`id_postavki`,`id_tov`,`tovcount`) values((select id_postavki from postavki group by 1 desc limit 1), (select id_tov from tovar where nazva = @name), @coun)", db.getConnection());
                        cmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = i.gettovname();
                        cmd.Parameters.Add("@coun", MySqlDbType.Int16).Value = i.getcount();
                        cmd.ExecuteNonQuery();
                        cmd = new MySqlCommand("Update tovar set `tovcount` = tovcount + @coun where (`nazva` = @name)", db.getConnection());
                        cmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = i.gettovname();
                        cmd.Parameters.Add("@coun", MySqlDbType.Int16).Value = i.getcount();
                        cmd.ExecuteNonQuery();
                    }
                    toolStripStatusLabel1.Text = "Запис про поставку створено";
                    tovarcls.Clear();
                    loaddata();
                    comboBox1.Enabled = true;
                    comboBox1.Text = "";
                    comboBox2.Text = "";
                }
                else
                {
                    toolStripStatusLabel1.Text = "Запис про поставку не було створено";
                }
            }
            
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            spispostForm sf = new spispostForm(Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString()));
            sf.Show();
        }
    }
}
