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
    public partial class Spivrobseccur : Form
    {
        public Spivrobseccur(string num)
        {
            InitializeComponent();
            loaduser(num);
        }

        spivrob sdat = new spivrob();

        public void loaduser(string num)
        {
            DBconnect db = new DBconnect();

            MySqlCommand cmd = new MySqlCommand("select s.sp_pib, s.sp_nomer, p.posada, s.sp_status, sb.login, sb.passwr from spivrob s, posada p, spivroblog sb where s.id_pos = p.id_pos and s.sp_id = sb.sp_id and s.sp_nomer = @num;", db.getConnection());
            cmd.Parameters.Add("@num", MySqlDbType.VarChar).Value = num;

            db.openConnection();

            MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                sdat = new spivrob(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString());
            }
            db.closeConnection();
        }

        private void Spivrobseccur_Load(object sender, EventArgs e)
        {
            textBox1.Text = sdat.getlog();
            textBox2.Text = sdat.getpass();
            label3.Text = "Користувач - " + sdat.getpib(); ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                DBconnect db = new DBconnect();

                MySqlCommand cmd = new MySqlCommand("update spivroblog SET `login` = @log, `passwr`=@pass where sp_id = (select sp_id from spivrob where sp_nomer = @nom)", db.getConnection());
                cmd.Parameters.Add("@log", MySqlDbType.VarChar).Value = textBox1.Text;
                cmd.Parameters.Add("@pass", MySqlDbType.VarChar).Value = textBox2.Text;
                cmd.Parameters.Add("@nom", MySqlDbType.VarChar).Value = sdat.getnom();

                db.openConnection();

                if(cmd.ExecuteNonQuery()==1)
                {
                    MessageBox.Show("Логін та пароль користувача змінено");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Логін та пароль користувача не змінено");
                }
                db.closeConnection();
            }
        }
    }
}
