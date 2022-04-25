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
    public partial class spivrobForm : Form
    {
        public spivrobForm()
        {
            InitializeComponent();
        }

        public void loaddata()
        {
            dataGridView1.Rows.Clear();
            DBconnect Db = new DBconnect();
            Db.openConnection();

            MySqlCommand cmd = new MySqlCommand("select s.sp_pib, s.sp_nomer, p.posada, s.sp_status from spivrob s, posada p where s.id_pos = p.id_pos;", Db.getConnection());
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

        private void spivrobForm_Load(object sender, EventArgs e)
        {
            loaddata();
            loadcombobox();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text=="" || comboBox1.Text=="" || maskedTextBox1.Text == "+38(   )    -  -")
            { 
                toolStripStatusLabel1.Text = "Заповніть усі поля";
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
            MySqlCommand command = new MySqlCommand("INSERT INTO `spivrob` (`sp_pib`, `sp_nomer`, `id_pos`) VALUES (@spb, @spn, (select p.id_pos from posada p where p.posada = @ppos));", db.getConnection());
            command.Parameters.Add("@spb", MySqlDbType.VarChar).Value = textBox1.Text;
            command.Parameters.Add("@spn", MySqlDbType.VarChar).Value = maskedTextBox1.Text;
            command.Parameters.Add("@ppos", MySqlDbType.VarChar).Value = comboBox1.Text;


            db.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                toolStripStatusLabel1.Text="Співробітника було додано.";
                loaddata();
            }
            else
            {
                toolStripStatusLabel1.Text="Співробітника не було додано.";
            }

            db.closeConnection();
        }

        public void loadcombobox()
        {
            DBconnect db = new DBconnect();
            db.openConnection();

            MySqlCommand command = new MySqlCommand("select p.posada from posada p;", db.getConnection());
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

        public bool isexist()
        {
            DBconnect db = new DBconnect();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `spivrob` WHERE `sp_nomer` = @num", db.getConnection());
            command.Parameters.Add("@num", MySqlDbType.VarChar).Value = maskedTextBox1.Text;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                toolStripStatusLabel1.Text = "Такий працівник вже існує";
                return true;
            }
            else
                return false;
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells[3].Value.ToString() == "Активний" && dataGridView1.CurrentRow.Cells[2].Value.ToString() != "Майстер-оптиметріст")
            {
                Spivrobseccur sc = new Spivrobseccur(dataGridView1.CurrentRow.Cells[1].Value.ToString());
                sc.Show();
            }
            else MessageBox.Show("Даний користувач не має дозволів для використання системи");
        }


        spivrob sb = new spivrob();
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString(); textBox1.Enabled = false;
            maskedTextBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            comboBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            comboBox2.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString(); comboBox2.Enabled = true;
            sb = new spivrob(textBox1.Text, maskedTextBox1.Text, comboBox1.Text, comboBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || comboBox1.Text == "" || maskedTextBox1.Text == "+38(   )    -  -")
            {
                toolStripStatusLabel1.Text = "Заповніть усі поля";
                return;
            }

            DBconnect db = new DBconnect();
            MySqlCommand cmd = new MySqlCommand("update spivrob set sp_nomer = @num, id_pos = (select id_pos from posada where posada = @pos), sp_status = @sts where (sp_nomer = @numold)", db.getConnection());
            cmd.Parameters.Add("@num", MySqlDbType.VarChar).Value = maskedTextBox1.Text;
            cmd.Parameters.Add("@pos", MySqlDbType.VarChar).Value = comboBox1.Text;
            cmd.Parameters.Add("@sts", MySqlDbType.VarChar).Value = comboBox2.Text;
            cmd.Parameters.Add("@numold", MySqlDbType.VarChar).Value = sb.getnom();

            db.openConnection();

            if (cmd.ExecuteNonQuery() == 1)
            {
                toolStripStatusLabel1.Text = "Дані співробітника змінено.";
                loaddata();
                textBox1.Text = ""; textBox1.Enabled = true;
                maskedTextBox1.Text = "+38(   )    -  -";
                comboBox1.Items.Add(""); comboBox1.Text = ""; comboBox1.Items.Remove("");
                comboBox2.Items.Add(""); comboBox2.Text = ""; comboBox2.Enabled = false; comboBox2.Items.Remove("");
            }
            else
            {
                toolStripStatusLabel1.Text = "Дані співробітника не змінено.";
            }

            db.closeConnection();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = ""; textBox1.Enabled = true;
            maskedTextBox1.Text = "+38(   )    -  -";
            comboBox1.Items.Add(""); comboBox1.Text = ""; comboBox1.Items.Remove("");
            comboBox2.Items.Add(""); comboBox2.Text = ""; comboBox2.Enabled = false; comboBox2.Items.Remove("");
        }
    }
}
