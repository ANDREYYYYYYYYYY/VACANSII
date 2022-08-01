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

    public partial class Form4 : Form
    {
        db db = new db();
        public Form4()
        {
            InitializeComponent();
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
            sad();
        }
        private void CreateColumns()
        {
            dataGridView1.Columns.Add("ID_Задачи", "ID_Задачи");
            dataGridView1.Columns.Add("ID_Вакансии", "ID_Вакансии");
            dataGridView1.Columns.Add("ID_Соискателя", "ID_Соискателя");
            dataGridView1.Columns.Add("ID_Работодателя", "ID_Работодателя");
            dataGridView1.Columns.Add("Дата_размещения_вакансии", "Дата_размещения_вакансии");
            dataGridView1.Columns.Add("Дата_завершения_вакансии", "Дата_завершения_вакансиия");
            dataGridView1.Columns.Add("Статус", "Статус");
            dataGridView1.Columns.Add("new", string.Empty);
            dataGridView1.Columns[7].Visible = false;

        }
        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetInt32(1), record.GetInt32(2), record.GetInt32(3), record.GetDateTime(4), record.GetDateTime(5), record.GetString(6), RowState.mod);
        }
        private void sad()
        {
        string quer = $"select * from Задачи where  ID_Задачи={a}";
        SqlCommand command = new SqlCommand(quer, db.GetConnection());
        db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                // выводим названия столбцов
                 while (reader.Read())
                 {
                    textBox2.Text = reader.GetInt32(0).ToString();
                   textBox5.Text = reader.GetInt32(1).ToString();
                    
                    
                     textBox6.Text = reader.GetInt32(2).ToString();
                    textBox7.Text = reader.GetInt32(3).ToString();
                    DateTime x = reader.GetDateTime(4);
                    DateTime x1 = reader.GetDateTime(5);
                    textBox3.Text = x.ToString("d") ;

                    textBox4.Text = x1.ToString("d");

                    textBox8.Text = reader.GetString(6);

                 }
            }
            reader.Close();
        }
        int a = 1;
        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string queryString = $"select * from Задачи";
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

        private void button2_Click(object sender, EventArgs e)
        {
            var stat = textBox1.Text;
            dataGridView1.Rows.Clear();
            string queryString = $"select * from Задачи WHERE LOWER(Статус) LIKE '%{stat}%'";
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
            Ex.Range r = wsh.Cells[1, 4] as Ex.Range;
            //Оформления
            r.Font.Size = 22;
            r.Font.Name = "Times New Roman";
            r.Font.Bold = true;
            wsh.Cells[2, 1] = "ID_Задачи";
            wsh.Cells[2, 2] = "ID_Вакансии";
            wsh.Cells[2, 3] = "ID_Соискателя";
            wsh.Cells[2, 4] = "ID_Работодателя";
            wsh.Cells[1, 4] = "Вывод данных о Задачах";
            wsh.Cells[2, 5] = "Дата_размещения_вакансии";
            wsh.Cells[2, 6] = "Дата_завершения_вакансии";
            wsh.Cells[2, 7] = "Статус";
            


            for (i = 0; i <= dataGridView1.RowCount - 1; i++)
            {
                for (j = 0; j <= dataGridView1.ColumnCount - 2; j++)
                {
                    wsh.Cells[i + 3, j + 1] = dataGridView1[j, i].Value.ToString();
                }
            }
            exApp.Visible = true;
        }
        private void deleterow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows[index].Visible = false;

            dataGridView1.Rows[index].Cells[7].Value = RowState.del;




        }

        private void upd()
        {
            db.openConnection();
            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowstat = (RowState)dataGridView1.Rows[index].Cells[7].Value;
                if (rowstat == RowState.exis)
                    continue;
                if (rowstat == RowState.del)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var delquery = $"delete from Задачи where ID_Задачи={id}";
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

        private void button4_Click(object sender, EventArgs e)
        {
            db.openConnection();
            var zad = textBox2.Text;
            var vac = textBox5.Text;
            var sois = textBox6.Text;
            var rab = textBox7.Text;
            var dr = textBox3.Text;
            var dz = textBox4.Text;
            var stat = textBox8.Text;
            var query = $"insert into Задачи(ID_Задачи,ID_Вакансии,ID_Соискателя,ID_Работодателя,Дата_размещения_вакансии,Дата_завершения_вакансии,Статус)  values ('{zad}','{vac}','{sois}','{rab}','{dr}','{dz}','{stat}')";
            var com = new SqlCommand(query, db.GetConnection());
            com.ExecuteNonQuery();
            MessageBox.Show("Запись успешно создана!", "Усппех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            db.closeConnection();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SqlCommand asd = new SqlCommand($"select count(*) from Задачи",db.GetConnection());
            int g = Convert.ToInt32(asd.ExecuteScalar());
            

            if (a < g)
            {
                a++;
            }
            sad();
           
        }
        
        private void button8_Click(object sender, EventArgs e)
        {
            
            if (a != 1)
            {
                a--;
            }
          
            sad();
           

        }
    }
}
