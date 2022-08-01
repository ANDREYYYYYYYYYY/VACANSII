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
using Ex = Microsoft.Office.Interop.Excel;

namespace AisUchetVakanciy
{
    enum RowState
    {
        del,
        exis,
        mod

    }
    public partial class Form2 : Form
    {
        db db = new db();
        public Form2()
        {
            InitializeComponent();
        }
        public void Form2_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }
        public void CreateColumns()
        {
            dataGridView1.Columns.Add("ID_Вакансии", "ID_Вакансии");
            dataGridView1.Columns.Add("Название_вакансии", "Название_вакансии");
            dataGridView1.Columns.Add("Оклад", "Оклад");
            dataGridView1.Columns.Add("Nw", string.Empty);

            dataGridView1.Columns[3].Visible = false;
        }
        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetFloat(2),RowState.mod);
            
        }
        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string queryString = $"select * from Вакансии";
            SqlCommand command = new SqlCommand(queryString, db.GetConnection());
            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();  
            while (reader.Read())  
            {
                ReadSingleRow(dgw, reader);

            }
            reader.Close();
            db.closeConnection();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Owner.Show();
            this.Close();
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void button2_Click(object sender, EventArgs e)
        {
            var Vac = textBox1.Text;
            dataGridView1.Rows.Clear();
            string queryString = $"select * from Вакансии where ID_Вакансии='{Vac}'";
            SqlCommand command = new SqlCommand(queryString, db.GetConnection());
            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow(dataGridView1, reader);

            }
            reader.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Ex.Application exApp = new Ex.Application();

            exApp.Workbooks.Add();
            Ex.Worksheet wsh = (Ex.Worksheet)exApp.ActiveSheet;
            int i, j;
            Ex.Range r = wsh.Cells[1, 2] as Ex.Range;
            //Оформления
            r.Font.Size = 22;
            r.Font.Name = "Times New Roman";
            r.Font.Bold = true;
            wsh.Cells[2, 1] = "ID_Вакансии";
            wsh.Cells[1, 2] = "Вывод данных о вакансиях";
            wsh.Cells[2, 2] = "Название_вакансии";
            wsh.Cells[2, 3] = "Оклад";
            

            for (i = 0; i <= dataGridView1.RowCount - 1; i++)
            {
                for (j = 0; j <= dataGridView1.ColumnCount - 2; j++)
                {
                    wsh.Cells[i + 3, j + 1] = dataGridView1[j, i].Value.ToString();
                }
            }
            exApp.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            db.openConnection();
            var idvac = textBox2.Text;
            var nazvvac = textBox3.Text;
            var oklad = textBox4.Text;
            var query =$"insert into Вакансии(ID_Вакансии,Название_вакансии,Оклад)  values ('{idvac}','{nazvvac}','{oklad}')";
            var com = new SqlCommand(query, db.GetConnection());
            com.ExecuteNonQuery();
            MessageBox.Show("Запись успешно создана!", "Усппех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            db.closeConnection();
        }
        private void deleterow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows[index].Visible = false;
            
                dataGridView1.Rows[index].Cells[3].Value = RowState.del;
           
            
        }
       
        private void upd()
        {
            db.openConnection();
            for(int  index=0; index < dataGridView1.Rows.Count; index++)
            {
                var rowstat = ( RowState)dataGridView1.Rows[index].Cells[3].Value;
                if (rowstat == RowState.exis)   
                    continue;
                if (rowstat == RowState.del)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var delquery = $"delete from Вакансии where ID_Вакансии={id}";
                    var com = new SqlCommand(delquery, db.GetConnection());
                    com.ExecuteNonQuery();
                    
                }
            }
            db.closeConnection();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            deleterow();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            upd();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
    
}
