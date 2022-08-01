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
    public partial class Form7 : Form
    {
        db db = new db();
        
        public Form7()
        {
            InitializeComponent();
        }
        private void Form7_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id", "id");
            dataGridView1.Columns.Add("login", "login");
            dataGridView1.Columns.Add("password", "password");
            dataGridView1.Columns.Add("type", "type");
            dataGridView1.Columns.Add("type", string.Empty);
            dataGridView1.Columns[4].Visible=false;


        }
        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2),record.GetString(3),RowState.mod);
        }
        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string queryString = $"select * from Пользователи";
            SqlCommand command = new SqlCommand(queryString, db.GetConnection());
            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);

            }
            reader.Close();

        }



        private void button1_Click(object sender, EventArgs e)
        {
            Owner.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            db.openConnection();
            var idrab = textBox2.Text;
            var nazvrab = textBox5.Text;
            var innrab = textBox6.Text;
            var adres = textBox7.Text;
          

            var query = $"insert into Пользователи (id,login,password,type)  values ('{idrab}','{nazvrab}','{innrab}','{adres}')";
            var com = new SqlCommand(query, db.GetConnection());
            com.ExecuteNonQuery();
            MessageBox.Show("Запись успешно создана!", "Усппех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            db.closeConnection();
        }
        private void deleterow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows[index].Visible = false;

            dataGridView1.Rows[index].Cells[4].Value = RowState.del;




        }

        private void upd()
        {
            db.openConnection();
            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowstat = (RowState)dataGridView1.Rows[index].Cells[4].Value;
                if (rowstat == RowState.exis)
                    continue;
                if (rowstat == RowState.del)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var delquery = $"delete from Пользователи where id ={id}";
                    var com = new SqlCommand(delquery, db.GetConnection());
                    com.ExecuteNonQuery();

                }
            }
            db.closeConnection();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            upd();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            deleterow();
        }
    }
}
