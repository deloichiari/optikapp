using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Optika
{
    public class spivrob
    {
        private string pib;
        private string num;
        private string posada;
        private string sts;
        private string login;
        private string password;

        public spivrob()
        {
        }

        public spivrob(string pib, string num, string posada, string sts, string login, string password)
        {
            this.pib = pib;
            this.num = num;
            this.posada = posada;
            this.sts = sts;
            this.login = login;
            this.password = password;
        }

        public spivrob(string pib, string num, string posada, string sts)
        {
            this.pib = pib;
            this.num = num;
            this.posada = posada;
            this.sts = sts;
        }

        public spivrob(string login, string password)
        {
            this.login = login;
            this.password = password;
        }
        public string getlog()
        {
            return this.login;
        }
        public string getpass()
        {
            return this.password;
        }

        public string getnom()
        {
            return this.num;
        }
        public string getpib()
        {
            return this.pib;
        }
        public string getpos()
        {
            return this.posada;
        }
        public bool autoriz()
        {
            DBconnect db = new DBconnect();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand cmd = new MySqlCommand("select s.sp_pib, p.posada, s.sp_status from spivrob s, spivroblog sb, posada p where sb.sp_id=s.sp_id and p.id_pos=s.id_pos and login = @log and passwr = @pass;", db.getConnection());
            cmd.Parameters.Add("@log", MySqlDbType.VarChar).Value = this.login;
            cmd.Parameters.Add("@pass", MySqlDbType.VarChar).Value = this.password;

            db.openConnection();

            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            if (table.Rows.Count == 0)
            {
                MessageBox.Show("Користувача з таким логіном та паралолем не існує");
                return false;
            }
            else
            {
                MySqlDataReader reader = null;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    this.pib = reader[0].ToString();
                    this.posada = reader[1].ToString();
                    this.sts = reader[2].ToString();
                }
                if(this.sts == "Активний")
                {
                    this.login = null;
                    this.password = null;
                    db.closeConnection();
                    return true;
                }
                else
                {
                    MessageBox.Show("Дані даного користувача неактивні");
                    db.closeConnection();
                    return false;
                }
                
            }
        }
    }
}
