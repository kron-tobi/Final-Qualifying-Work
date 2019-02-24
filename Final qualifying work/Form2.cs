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
        int click = 0;
        int[] arr_id = new int[20];
        string textUpdate;
        double sellUpdate;
        NpgsqlDataReader reader;
        public bool editing = false;
        int id_req;

        public Form2()
        {
            InitializeComponent();
            parametresConnections = Form0.F0.parametresConnections;
            connection = Form0.F0.connection;
            data = new DataSet();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //click = 0;
            dataGridView2.ClearSelection(); // сброс селекта Tab           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Close();
            this.Close();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*Form of = Application.OpenForms["From2"];
            if (of != null)
                of.Close();*/
            connection.Close();
            Application.Exit();
            
        }

        private void button2_Click(object sender, EventArgs e) // INSERT(Save)
        {            
            try
            {
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    if (textBox1.Text != "" && textBox2.Text != "" && textBox6.Text != "")
                    {
                        insertData(textBox1.Text, textBox2.Text, dateTimePicker1, richTextBox1.Text, "active", textUpdate, sellUpdate);
                        Form1.F1.updateGridView1();                        
                    }
                }          
            }
            catch (Exception error)
            {
                toolStripStatusLabel2.Text = "Ошибка! Сохранить не удалось!\nMessage error: " + error.Message;
            }
            connection.Close();            
            this.Close();
        }

        private void insertData(string fio_req, string phone_num, DateTimePicker date_req, string comment_req, string status_req, string list_services, double sum_req)
        {
            try
            {
                if(!editing)
                {
                    command = new NpgsqlCommand("INSERT INTO request (fio_client, phone_num, date_req, comment_req, status_req, list_services, sum_req) VALUES ('" + fio_req + "', '" + phone_num + "', '" + date_req.Value.Date.ToString("yyyy.MM.dd") + "', '" + comment_req + "', '" + status_req + "', '{" + list_services + "}', " + sum_req + ");", connection);
                    command.ExecuteNonQuery();
                    toolStripStatusLabel2.Text = "Успешное добавление!";
                }
                else if(editing)
                {
                    command = new NpgsqlCommand("UPDATE request SET (fio_client, phone_num, date_req, comment_req, status_req, list_services, sum_req) = ('" + fio_req + "', '" + phone_num + "', '" + date_req.Value.Date.ToString("yyyy.MM.dd") + "', '" + comment_req + "', '" + status_req + "', '{" + list_services + "}', " + sum_req + ") WHERE id_req = " + id_req + ";", connection);
                    command.ExecuteNonQuery();
                    toolStripStatusLabel2.Text = "Успешное Изменение!";
                }
            }
            catch (Exception error)
            {
                toolStripStatusLabel2.Text = "Ошибка Вставки!\nMessage error: " + error.Message;
            }
        }
       
        private void button3_Click(object sender, EventArgs e)  // Добавить (list_services ARRAY)
        {            
            if (textBox3.Text != "" && Convert.ToInt32(textBox3.Text) > 0)
            {
                sellUpdate += Convert.ToDouble(textBox5.Text);
                textBox6.Text = sellUpdate.ToString();
                if (click == 0)
                {
                    textUpdate = textBox3.Text;
                }
                else if (click > 0)
                {
                    textUpdate = textUpdate + "," + textBox3.Text;
                }          
                listBox1.Items.Add(textBox4.Text);
                //checkedListBox1.Items.Add(textBox4.Text);
                //checkedListBox1.SetItemChecked(click, true);                        
            }
            click++;
            toolStripStatusLabel2.Text = "Успешное добавление!" + click;
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

        public void servicesForm()  // Загрузка таблицы услуг
        {           
            query = "SELECT id_service,name_service,price_service FROM service";
            //query = "SELECT id_req, fio, phone_num, date, comment_req  FROM req UNION SELECT id_req, fio, phone_num, date, comment_req FROM req_list";
            adapter = new NpgsqlDataAdapter(query, connection);

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

        public void loadForEditingForm(string id) // Изменить
        {
            editing = true;
            id_req = Convert.ToInt32(id);
            int[] list_services = { };
            try
            {
                //MessageBox.Show("кукусики!");
                //toolStripStatusLabel2.Text = "Вы приступили к изменению запроса!\nMessage error: ";
                
                connection.Open();
                command = new NpgsqlCommand("SELECT fio_client, phone_num, date_req, comment_req, list_services FROM request WHERE id_req =" + id, connection);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        object fio_client = reader.GetValue(0);
                        object phone_num = reader.GetValue(1);
                        object date_req = reader.GetValue(2);
                        object comment_req = reader.GetValue(3);
                        list_services = (int[])reader.GetValue(4);

                        textBox1.Text = fio_client.ToString();
                        textBox2.Text = phone_num.ToString();
                        dateTimePicker1.Text = date_req.ToString();
                        richTextBox1.Text = comment_req.ToString();
                    }
                }
                else
                {
                    toolStripStatusLabel2.Text = "Столбец не обнаружен!";
                }
            }
            catch (Exception error)
            {
                toolStripStatusLabel2.Text = "Ошибка!\nMessage error: " + error.Message;
            }            
            connection.Close();
            try
            {                
                connection.Open();
                for (int i = 0; i < list_services.Length; i++)
                {
                    toolStripStatusLabel2.Text = toolStripStatusLabel2.Text + list_services[i];
                }
                int id_list = 1;
                
                for (int i = 0; i < list_services.Length; i++)
                {
                    
                    id_list = list_services[i];
                    command = new NpgsqlCommand("SELECT name_service,price_service FROM service WHERE id_service =" + id_list, connection);
                    reader = command.ExecuteReader();
                    reader.Read();
                    object name_service;                    
                    if (reader.HasRows)
                    {
                        name_service = reader.GetValue(0);
                        sellUpdate += Convert.ToDouble(reader.GetValue(1));
                        textBox6.Text = sellUpdate.ToString();
                        reader.Close();
                        if (i == 0)
                        {
                            textUpdate = id_list.ToString();
                        }
                        else if (i > 0)
                        {
                            textUpdate = textUpdate + ", " + id_list.ToString();
                        }
                        click++;
                        listBox1.Items.Add(name_service);
                        //checkedListBox1.Items.Add(name_service);
                        //checkedListBox1.SetItemChecked(i, true);
                    }                   
                }                
            }
            catch (Exception error)
            {
                toolStripStatusLabel2.Text = "Ошибка!\nMessage error: " + error.Message;
            }
            connection.Close();
            //textBox3.Text = 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //if (listBox1.I)
            //connection.Close();
            //listBox1.Items.Remove(name_service);
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //string select = listBox1.SelectedItem.ToString();
            
        }
    }
}
