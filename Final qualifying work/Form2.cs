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
        NpgsqlDataAdapter adapter;
        DataSet data;        

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {            
            parametresConnections = Form0.F0.parametresConnections;
            connection = Form0.F0.connection;
            query = "SELECT * FROM service";
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
                    for(int i = 0; i < data.Tables[0].Rows.Count; i++)
                    {
                        checkedListBox1.Items.Add(data.Tables[0].Rows[i][1].ToString());
                        listBox1.Items.Add(data.Tables[0].Rows[i][2].ToString());
                    }
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
                label1.Text = "Ошибка! Сохранить не удалось!\nMessage error: " + error.Message;
            }
            connection.Close();
            //this.Close();
        }        

        private void ToolStripMenuItem0_Click(object sender, EventArgs e)
        {

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
    }
}
