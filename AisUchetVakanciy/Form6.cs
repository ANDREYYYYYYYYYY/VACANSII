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
    public partial class Form6 : Form
    {
        db db = new db();
        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }
        private void CreateColumns()
        {
            dataGridView1.Columns.Add("ID_Соискателя", "ID_Соискателя");
            dataGridView1.Columns.Add("ФИО", "ФИО");
            dataGridView1.Columns.Add("Гражданство", "Гражданство");
            dataGridView1.Columns.Add("Дата_рождения", "Дата_рождения");
            dataGridView1.Columns.Add("Пол", "Пол");
            dataGridView1.Columns.Add("Телефон", "Телефон");
            dataGridView1.Columns.Add("Образование", "Образование");
            dataGridView1.Columns.Add("Предпочтения", "Предпочтения");
            dataGridView1.Columns.Add("nw",string.Empty);
            dataGridView1.Columns[8].Visible = false;

        }
        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetDateTime(3), record.GetString(4), record.GetString(5), record.GetString(6), record.GetString(7),RowState.mod);
        }
        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string queryString = $"select * from Соискатели";
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
            var telefon = textBox1.Text;
            var obraz = textBox4.Text;
            var predpotch = textBox8.Text;

            var query = $"insert into Соискатели (ID_Соискателя,ФИО,Гражданство,Дата_рождения,Пол,Телефон,Образование,Предпочтения)  values ('{idrab}','{nazvrab}','{innrab}','{adres}','{telef}','{telefon}','{obraz}','{predpotch}')";
            var com = new SqlCommand(query, db.GetConnection());
            com.ExecuteNonQuery();
            MessageBox.Show("Запись успешно создана!", "Усппех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            db.closeConnection();
        }
        private void deleterow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows[index].Visible = false;

            dataGridView1.Rows[index].Cells[8].Value = RowState.del;




        }

        private void upd()
        {
            db.openConnection();
            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowstat = (RowState)dataGridView1.Rows[index].Cells[8].Value;
                if (rowstat == RowState.exis)
                    continue;
                if (rowstat == RowState.del)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var delquery = $"delete from Соискатели where ID_Соискателя={id}";
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
