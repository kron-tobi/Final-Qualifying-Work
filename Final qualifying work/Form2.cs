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
                        insertData(textBox1.Text, Convert.ToInt32(textBox2.Text), dateTimePicker1, richTextBox1.Text);
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

        private void insertData(string fio_req, int phone_num, DateTimePicker date_req, string comment_req)
        {
            try
            {
                //command = new NpgsqlCommand("INSERT INTO req (fio, phone_num, date, comment_req) VALUES ('" + fio + "', " + phone_num + ", '" + date.Value.Date.ToString("yyyy.MM.dd") + "', '" + comment_req + "');", connection); //",'" + servise + "'," + status + ",'" + name_master + "','" + name_client + "');", connection);
                command = new NpgsqlCommand("INSERT INTO request (fio_client, phone_num, date_req, comment_req) VALUES ('" + fio_req + "', " + phone_num + ", '" + date_req.Value.Date.ToString("yyyy.MM.dd") + "', '" + comment_req + "');", connection); 
                command.ExecuteNonQuery();
                toolStripStatusLabel2.Text = "Успешное добавление!";
            }
            catch (Exception error)
            {
                toolStripStatusLabel2.Text = "Ошибка Вставки!\nMessage error: " + error.Message;
            }
        }

        private void takeIdFromCheckedListBox()
        {

        }

        private void takeElementArray(int[] arr)
        {
            /*dataReader = command.ExecuteReader();
            while (dataReader.Read())
                MessageBox.Show("{0}\t{1} \n", dataReader[0], dataReader[1]);
            for (int i = 0; i < arr.Length; i++)
            {
                //reader =
                
            }*/
            // выводим названия столбцов
            //Console.WriteLine("{0}\t{1}\t{2}", reader.GetName(0), reader.GetName(1), reader.GetName(2));

            while (reader.Read()) // построчно считываем данные
            {
                object id = reader.GetValue(0);
                object name = reader.GetValue(1);
                object age = reader.GetValue(2);

                Console.WriteLine("{0} \t{1} \t{2}", id, name, age);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //arr_id[click] = Convert.ToInt32(textBox3.Text);                    
                
           
            /*try
            {
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {                    
                    command = new NpgsqlCommand("SELECT * FROM request", connection); ;
                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {                        
                        while (reader.Read())
                        {                            
                            int[] list_services = (int[])reader.GetValue(7);                            
                            //toolStripStatusLabel2.Text = ("" + list_services[0]);
                        }
                    }
                    else
                    {
                        toolStripStatusLabel2.Text = "Столбец не обнаружен!";                       
                    }
                }
                //reader.Close();
            }
            catch (Exception error)
            {
                toolStripStatusLabel2.Text = "Ошибка!\nMessage error: " + error.Message;
            }
            connection.Close();*/
            
            if (textBox3.Text != "" && Convert.ToInt32(textBox3.Text) > 0)
            {
                try
                {
                    connection.Open();
                    if (connection.State == System.Data.ConnectionState.Open)
                    {                        
                        if (click == 0)
                        {
                            textUpdate = "" + textBox3.Text;
                        }
                        else if (click > 0)
                        {
                            textUpdate = textUpdate + "," + textBox3.Text;
                        }                       
                        command = new NpgsqlCommand("UPDATE request SET list_services = '{" + textUpdate + "}';", connection);
                        command.ExecuteNonQuery();
                        
                        toolStripStatusLabel2.Text = "Успешное добавление!";
                        checkedListBox1.Items.Add(textBox4.Text);
                    }
                }
                catch (Exception error)
                {
                    toolStripStatusLabel2.Text = "Ошибка!\nMessage error: " + error.Message;
                }
                connection.Close();                
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
