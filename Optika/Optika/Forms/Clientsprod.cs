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
    public partial class Clientsprod : Form
    {
        public Clientsprod()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool p = true;

            if (maskedTextBox1.Text == "" || textBox1.Text == "")
            {
                MessageBox.Show("Заповніть всі поля");
                return;
            }

            while (p == true)
            {
                if (isclientexist())
                    return;
                p = isclientexist();
                break;
            }

            DBconnect db = new DBconnect();

            MySqlCommand cmd = new MySqlCommand("INSERT INTO `optika2`.`clien` (`cl_pib`, `cl_nomer`) VALUES (@pib, @num);", db.getConnection());
            cmd.Parameters.Add("@pib", MySqlDbType.VarChar).Value = textBox1.Text;
            cmd.Parameters.Add("@num", MySqlDbType.VarChar).Value = maskedTextBox1.Text;

            db.openConnection();

            if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Дисконт було створено");
                this.Hide();
            }
            else
            { 
                toolStripStatusLabel1.Text = "Дисконт не було створено";
            }

            db.closeConnection();
        }

        public Boolean isclientexist()
        {
            DBconnect db = new DBconnect();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            //ставят тильды ````` ТЫЛЬДЫ!!!!
            MySqlCommand command = new MySqlCommand("SELECT * FROM `clien` WHERE `cl_nomer` = @num", db.getConnection());
            command.Parameters.Add("@num", MySqlDbType.VarChar).Value = maskedTextBox1.Text;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                toolStripStatusLabel1.Text = "Такий клієнт вже існує";
                return true;
            }
            else
                return false;
        }
    }
}
