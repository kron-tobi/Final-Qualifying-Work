using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Windows.Forms;

namespace Final_qualifying_work
{    
    public partial class Form1 : Form
    {
        //List<string> dataItems = new List<string>();
        public static Form1 F1;
        string parametresConnections;
        string query;
        //string textIN;
        NpgsqlConnection connection; 
        NpgsqlCommand command;        
        NpgsqlDataAdapter adapter;
        DataSet data;
        

        public Form1()
        {
            InitializeComponent();
            F1 = this;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            parametresConnections = Form0.F0.parametresConnections;            
            query = Form0.F0.query;
            connection = Form0.F0.connection;
            adapter = Form0.F0.adapter;
            data = new DataSet();
            
            try
            {
                connection.Open();                
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    toolStripStatusLabel2.Text = "Успешное подключение к базе!";
                    adapter.Fill(data);
                    dataGridView1.DataSource = data.Tables[0];
                    //dataGridView1.AutoGenerateColumns = true;
                    dataGridView1.ClearSelection();
                }
            }
            catch (Exception error)
            {
                toolStripStatusLabel2.Text = "Ошибка соединения!\nMessage error: " + error.Message;
            }     
            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)  // INSERT
        {
            //this.Hide();
            Form2 newForm2 = new Form2();
            newForm2.ShowDialog();            
        }

        private void button2_Click(object sender, EventArgs e)  // DELETE
        {            
            try
            {
                connection.Open();                
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    //DataGridViewRow line = dataGridView1.CurrentRow;
                    deleteLine(Convert.ToInt32(textBox1.Text));
                    data.Clear();
                    adapter.Fill(data);
                    dataGridView1.DataSource = data.Tables[0];
                }
            }
            catch (Exception error)
            {
                toolStripStatusLabel2.Text = "Ошибка Удаления!\nMessage error: " + error.Message;
            }        
            connection.Close();
            
            if(dataGridView1.RowCount != 0) // после удаления делаем переход к следующей ячейке
            {
                DataGridViewRow row = dataGridView1.Rows[0];
                textBox1.Text = row.Cells[0].Value.ToString();
            }                
        }

        private void button3_Click(object sender, EventArgs e)  // UpdateALL
        {            
            try
            {
                connection.Open();
                updateGridView1();
            }
            catch (Exception error)
            {
                toolStripStatusLabel2.Text = "Ошибка при Обновлении!\nMessage error: " + error.Message;
            }
            connection.Close();            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int indexRow = 0;
            if (e.RowIndex >= 0)
                indexRow = e.RowIndex;
            else return;
            DataGridViewRow row = dataGridView1.Rows[indexRow];            
            textBox1.Text = row.Cells[0].Value.ToString();
            textBox2.Text = row.Cells[1].Value.ToString();
        }        

        private void deleteLine(int id_req)
        {
            try
            {
                command = new NpgsqlCommand("DELETE FROM request WHERE id_req = " + id_req + ";", connection);
                command.ExecuteNonQuery();
                toolStripStatusLabel2.Text = "Удаление выполнено!";
            }
            catch (Exception error)
            {
                toolStripStatusLabel2.Text = "Ошибка! Удалить не удалось!\nMessage error: " + error.Message;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {            
            Application.Exit();
        }

        public void updateGridView1()   // REFRESH
        {
           try
           {               
               if (connection.State == System.Data.ConnectionState.Open)
               {
                   data.Clear();
                   adapter.Fill(data);
                   dataGridView1.DataSource = data.Tables[0];
                   toolStripStatusLabel2.Text = "Обновление выполнено!";
               }               
           }
           catch (Exception error)
           {
               toolStripStatusLabel2.Text = "Ошибка при Обновлении!\nMessage error: " + error.Message;
           } 
        }

        private void resetCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                command = new NpgsqlCommand("ALTER SEQUENCE request_id_req_seq RESTART; UPDATE request SET id_req = DEFAULT;", connection);
                command.ExecuteNonQuery();
                updateGridView1();
                textBox1.Text = "";
                textBox2.Text = "";
            }
            catch (Exception error)
            {
                toolStripStatusLabel2.Text = "Ошибка Сброса Id!\nMessage error: " + error.Message;
            }
            connection.Close();
        }
    }
}
