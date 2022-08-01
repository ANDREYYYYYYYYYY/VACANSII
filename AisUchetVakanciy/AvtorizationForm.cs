using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AisUchetVakanciy
{
    public partial class AvtorizationForm : Form
    {
        db db = new db();
        public AvtorizationForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var login = textBox1.Text;
            var password = textBox2.Text;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"Select type From Пользователи where login ='" + textBox1.Text + "' and password ='" + textBox2.Text + "'";
            SqlCommand command = new SqlCommand(querystring, db.GetConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                // Нужный Вам ID
                string ID = table.Rows[0][0].ToString();

                if (table.Rows[0][0].ToString() == "Admin")
                {

                    MessageBox.Show("Вы вошли как админ");
                    this.Hide();
                    Form3 ss = new Form3();
                    ss.Show();
                }
                if (table.Rows[0][0].ToString() == "simple")
                {

                    MessageBox.Show("Вы вошли как обычный пользователь");
                    this.Hide();
                    Form1 f1 = new Form1();
                    f1.Show();


                }
            }

            else
            {
                MessageBox.Show("Неправильно введённые имя или пароль");
            }
        }
    }
}
