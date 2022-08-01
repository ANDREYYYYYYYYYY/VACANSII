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
    public partial class Form5 : Form
    {
        db db = new db();
        public Form5()
        {
            InitializeComponent();
        }
        private void Form5_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);

        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("ID_Работодателя", "ID_Работодателя");
            dataGridView1.Columns.Add("Название_работодателя", "Название_работодателя");
            dataGridView1.Columns.Add("Инн_работодателя", "Инн_работодателя");
            dataGridView1.Columns.Add("Адрес_работодателя", "Адрес_работодателя");
            dataGridView1.Columns.Add("Телефон_работодателя", "Телефон_работодателя");
            dataGridView1.Columns.Add("nw",string.Empty);
            dataGridView1.Columns[5].Visible = false;

        }
        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1),record.GetValue(2),record.GetString(3),record.GetValue(4),RowState.mod);
        }
        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string queryString = $"select * from Работодатели";
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
            var telef = textBox3.Text;
          
            var query = $"insert into Работодатели (ID_Работодателя,Название_работодателя,Инн_работодателя,Адрес_работодателя,Телефон_работодателя)  values ('{idrab}','{nazvrab}','{innrab}','{adres}','{telef}')";
            var com = new SqlCommand(query, db.GetConnection());
            com.ExecuteNonQuery();
            MessageBox.Show("Запись успешно создана!", "Усппех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            db.closeConnection();
        }
        private void deleterow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows[index].Visible = false;

            dataGridView1.Rows[index].Cells[5].Value = RowState.del;




        }

        private void upd()
        {
            db.openConnection();
            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowstat = (RowState)dataGridView1.Rows[index].Cells[5].Value;
                if (rowstat == RowState.exis)
                    continue;
                if (rowstat == RowState.del)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var delquery = $"delete from Работодатели where ID_Работодателя={id}";
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
