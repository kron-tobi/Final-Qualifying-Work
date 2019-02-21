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
    public partial class Form2 : Form
    {
        string parametresConnections;
        string query;        
        NpgsqlConnection connection;
        NpgsqlCommand command;
        NpgsqlDataReader reader;
        NpgsqlDataAdapter adapter;
        DataSet data;
        int click = 0;
        int[] arr_id = new int[20];
        string textUpdate;

        public Form2()
        {
            InitializeComponent(); 
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //if(checkedListBox1.)           
            
            parametresConnections = Form0.F0.parametresConnections;
            connection = Form0.F0.connection;
            query = "SELECT id_service,name_service,price_service FROM service";
            //query = "SELECT id_req, fio, phone_num, date, comment_req  FROM req UNION SELECT id_req, fio, phone_num, date, comment_req FROM req_list";
            adapter = new NpgsqlDataAdapter(query, connection);
            data = new DataSet();                        
            
            try
            {
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    toolStripStatusLabel2.Text = "Успешное подключение к форме Услуг!";
                    adapter.Fill(data);
                    dataGridView2.DataSource = data.Tables[0];
                    dataGridView2.ClearSelection();
                    /*
                    for(int i = 0; i < data.Tables[0].Rows.Count; i++)
                    {
                        checkedListBox1.Items.Add(data.Tables[0].Rows[i][1].ToString());
                        listBox1.Items.Add(data.Tables[0].Rows[i][2].ToString());
                    }*/
                    //checkedListBox1.DataSource = data.Tables[0];                    
                }
            }
            catch (Exception error)
            {
                toolStripStatusLabel2.Text = "Ошибка подключение к форме Услуг!\nMessage error: " + error.Message;
            }
            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) // INSERT(Save)
        {            
            try
            {
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    if (textBox1.Text != "" && textBox2.Text != "")
                    {
                        insertData(textBox1.Text, Convert.ToInt32(textBox2.Text), dateTimePicker1, richTextBox1.Text, "active", textUpdate);
                        Form1.F1.updateGridView1();                        
                    }
                }          
            }
            catch (Exception error)
            {
                toolStripStatusLabel2.Text = "Ошибка! Сохранить не удалось!\nMessage error: " + error.Message;
            }
            connection.Close();            
            //this.Close();
        }

        private void insertData(string fio_req, int phone_num, DateTimePicker date_req, string comment_req, string status_req, string list_services)
        {
            try
            {
                command = new NpgsqlCommand("INSERT INTO request (fio_client, phone_num, date_req, comment_req, status_req,list_services) VALUES ('" + fio_req + "', " + phone_num + ", '" + date_req.Value.Date.ToString("yyyy.MM.dd") + "', '" + comment_req + "', '" + status_req + "', '{" + list_services + "}'" + ");", connection); 
                command.ExecuteNonQuery();
                toolStripStatusLabel2.Text = "Успешное добавление!";
            }
            catch (Exception error)
            {
                toolStripStatusLabel2.Text = "Ошибка Вставки!\nMessage error: " + error.Message;
            }
        }
       
        private void button3_Click(object sender, EventArgs e)  // list_services ARRAY
        {
            if (textBox3.Text != "" && Convert.ToInt32(textBox3.Text) > 0)
            {
                if (click == 0)
                {
                    textUpdate = "" + textBox3.Text;
                }
                else if (click > 0)
                {
                    textUpdate = textUpdate + "," + textBox3.Text;
                }
                toolStripStatusLabel2.Text = "Успешное добавление!";
                checkedListBox1.Items.Add(textBox4.Text);
                checkedListBox1.SetItemChecked(click, true);                        
            }
            click++;
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int indexRow = 0;
            if (e.RowIndex >= 0)
                indexRow = e.RowIndex;
            else return;
            DataGridViewRow row = dataGridView2.Rows[indexRow];
            textBox3.Text = row.Cells[0].Value.ToString();
            textBox4.Text = row.Cells[1].Value.ToString();
            textBox5.Text = row.Cells[2].Value.ToString();
        }
    }
}
