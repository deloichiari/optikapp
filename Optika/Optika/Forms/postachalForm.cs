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
    public partial class postachalForm : Form
    {
        public postachalForm()
        {
            InitializeComponent();
        }

        private void postachalForm_Load(object sender, EventArgs e)
        {
            loadata();
        }

        public void loadata()
        {
            dataGridView1.Rows.Clear();
            DBconnect Db = new DBconnect();
            Db.openConnection();

            MySqlCommand cmd = new MySqlCommand("SELECT name_postach FROM optika2.postachal;", Db.getConnection());
            MySqlDataReader reader = cmd.ExecuteReader();
            List<string[]> data = new List<string[]>();

            while (reader.Read())
            {
                data.Add(new string[1]);

                data[data.Count - 1][0] = reader[0].ToString();
                
            }
            reader.Close();
            Db.closeConnection();
            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);
        }

        public bool isexist()
        {
            DBconnect db = new DBconnect();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM optika2.postachal where name_postach = @namp;", db.getConnection());
            command.Parameters.Add("@namp", MySqlDbType.VarChar).Value = textBox1.Text;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                toolStripStatusLabel1.Text = "Такий постачальник вже існує";
                return true;
            }
            else
                return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="")
            {
                MessageBox.Show("Заповніть поле");
                return;
            }
            bool p = true;
            while (p == true)
            {
                if (isexist())
                    return;
                p = isexist();
                break;
            }

            DBconnect db = new DBconnect();

            MySqlCommand command = new MySqlCommand("INSERT INTO `postachal` (`name_postach`) VALUES (@namp);", db.getConnection());
            command.Parameters.Add("@namp", MySqlDbType.VarChar).Value = textBox1.Text;
            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                toolStripStatusLabel1.Text = "Постачатьника було додано.";
                loadata();
            }
            else
            {
                toolStripStatusLabel1.Text = "Постачальника не було додано.";
            }

            db.closeConnection();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            tovpostForm tp = new tovpostForm(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            tp.ShowDialog();
        }
    }
}
