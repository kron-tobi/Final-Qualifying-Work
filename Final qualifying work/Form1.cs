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
        public static Form1 F1;
        List<string> dataItems = new List<string>();
        string parametresConnections;
        string query;
        string textIN;
        NpgsqlConnection connection; 
        NpgsqlCommand command;
        //NpgsqlDataReader dataReader;
        NpgsqlDataAdapter adapter;
        DataSet data;
        bool connected = false;
        //private Form0 MyForm = Application.OpenForms[0] as Form0;
        

        public Form1()
        {
            InitializeComponent();
            F1 = this;
            /*
            parametresConnections = "Server = " + textBox3.Text + "; Port = " + textBox4.Text + "; User Id = " + textBox5.Text + "; Password = " + textBox6.Text + "; Database = " + textBox7.Text;
            query = "SELECT * FROM request"; // запрос к таблице
            connection = new NpgsqlConnection(parametresConnections);
            //command = new NpgsqlCommand(query, connection);
            adapter = new NpgsqlDataAdapter(query, connection);
            //dataReader = command.ExecuteReader();
            data = new DataSet();*/
            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            parametresConnections = Form0.F0.parametresConnections;
            //parametresConnections = Form0.F0.parametresConnections;
            query = Form0.F0.query;
            connection = Form0.F0.connection;
            adapter = Form0.F0.adapter;
            data = new DataSet();
            try
            {
                connection.Open();                
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    label1.Text = "Connection status: Successful Connection!";
                    adapter.Fill(data);
                    dataGridView1.DataSource = data.Tables[0];
                }
            }
            catch (Exception error)
            {
                label1.Text = "Connection status: Error connections!\nMessage error: " + error.Message;
            }     
            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)  // INSERT
        {
            try
            {
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    label1.Text = "Connection2 status: Successful Connection2!";
                }
                textIN = textBox2.Text;
                if (textIN != "")                
                    insert_data(textIN);
            }
            catch (Exception error)
            {
                label1.Text = "Connection2 status: Error connections2!\nMessage error: " + error.Message;
            }
            data.Clear();
            adapter.Fill(data);
            dataGridView1.DataSource = data.Tables[0];           
            connection.Close();           
        }

        private void button2_Click(object sender, EventArgs e)  // DELETE
        {            
            try
            {
                connection.Open();
                DataGridViewRow line = dataGridView1.CurrentRow;
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    label1.Text = "Connection3 status: Successful Connection3!";                    
                    deleteLine(Convert.ToInt32(textBox1.Text));
                }
            }
            catch (Exception error)
            {
                label1.Text = "Error DELETE!\nMessage error: " + error.Message;
            }            
            data.Clear();
            adapter.Fill(data);
            dataGridView1.DataSource = data.Tables[0];            
            connection.Close();
        }

        private void button3_Click(object sender, EventArgs e)  // CONNECT
        {
            /*try
            {
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connected = true;
                    label1.Text = "Connection status: Successful Connection!";
                }
            }
            catch (Exception error)
            {
                label1.Text = "Connection status: Error connections!\nMessage error: " + error.Message;
            }
            data.Clear();
            adapter.Fill(data);
            dataGridView1.DataSource = data.Tables[0];            
            connection.Close();*/
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int indexRow = 0;
            if (e.RowIndex >= 0)
                indexRow = e.RowIndex;
            else return;
            DataGridViewRow row = dataGridView1.Rows[indexRow];
            //textBox1.Text = row.Cells[0].Value.ToString();
            //label1.Text = "RowIndex = " + row;
            textBox1.Text = row.Cells[0].Value.ToString();
            textBox2.Text = row.Cells[1].Value.ToString();
        }

        private void insert_data(string servise)
        {
            try
            {
                command = new NpgsqlCommand("INSERT INTO request (servise) VALUES ('" + servise + "');", connection); //",'" + servise + "'," + status + ",'" + name_master + "','" + name_client + "');", connection);
                command.ExecuteNonQuery();
            }
            catch (Exception error)
            {
                label1.Text = "Error INSERT!\nMessage error: " + error.Message;
            }
        }

        private void deleteLine(int id_request)
        {
            try
            {
                command = new NpgsqlCommand("DELETE FROM request WHERE id_request = " + id_request + ";", connection);
                command.ExecuteNonQuery();
            }
            catch (Exception error)
            {
                label1.Text = "Error DELETE!\nMessage error: " + error.Message;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {            
            //Application.Exit();
        }
    }
}
