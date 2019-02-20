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
        string textIN;
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
            query = "SELECT * FROM service";
            connection = Form0.F0.connection;
            adapter = new NpgsqlDataAdapter(query, connection);
            data = new DataSet();                        
            
            try
            {
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    toolStripStatusLabel2.Text = "Connection status: Successful Connection!";
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
                toolStripStatusLabel2.Text = "Connection status: Error connections!\nMessage error: " + error.Message;
            }
            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) // INSERT(Save)
        {
            this.Close();
            try
            {
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    
                }
                //textIN = ;
                if (textBox1.Text != "" && textBox2.Text != "")                
                    insertData(textBox1.Text, Convert.ToInt32(textBox2.Text),richTextBox1.Text);
            }
            catch (Exception error)
            {
                label1.Text = "Connection2 status: Error connections2!\nMessage error: " + error.Message;
            }
            data.Clear();
            adapter.Fill(data);
            //dataGridView1.DataSource = data.Tables[0];           
            connection.Close();
        }

        private void load_service()
        {
            List<string> service_items = new List<string>();
            //checkedListBox1.
            //service_items = checkedListBox1.CheckedItems();
            
        }

        private void ToolStripMenuItem0_Click(object sender, EventArgs e)
        {

        }

        private void insertData(string fio, int phone_num, string comment)
        {
            try
            {
                command = new NpgsqlCommand("INSERT INTO req (fio, phone_num, date, comment_req) VALUES ('" + fio + "'," + phone_num + "," + dateTimePicker1 + ",'" + comment + "');", connection); //",'" + servise + "'," + status + ",'" + name_master + "','" + name_client + "');", connection);
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
