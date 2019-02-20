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
using System.IO;

namespace Final_qualifying_work
{
    public partial class Form0 : Form
    {
        public static Form0 F0;
        public List<string> dataItems = new List<string>();
        public string parametresConnections;
        public string query;
        public string textIN;
        public NpgsqlConnection connection;
        //public NpgsqlCommand command;       
        public NpgsqlDataAdapter adapter;
          

        public Form0()
        {
            InitializeComponent();
            F0 = this;
        }

        private void Form0_Load(object sender, EventArgs e)
        {            
            if (!File.Exists("userParametresConnection.txt"))
            {
                using (StreamWriter strWr = new StreamWriter("userParametresConnection.txt"))
                {
                    strWr.WriteLine("127.0.0.1{0}5432{0}postgres{0}postgres{0}MyFirstBase", Environment.NewLine);
                }
            }
            else 
            {
                string[] line = File.ReadAllLines("userParametresConnection.txt", Encoding.Default);
                textBox3.Text = line[0];
                textBox4.Text = line[1];
                textBox5.Text = line[2];
                textBox6.Text = line[3];
                textBox7.Text = line[4];
            }                          
        }

        private void button3_Click(object sender, EventArgs e)  // CONNECT
        {
            try
            {
                parametresConnections = "Server = " + textBox3.Text + "; Port = " + textBox4.Text + "; User Id = " + textBox5.Text + "; Password = " + textBox6.Text + "; Database = " + textBox7.Text;
                query = "SELECT * FROM request"; // запрос к таблице
                connection = new NpgsqlConnection(parametresConnections);
                adapter = new NpgsqlDataAdapter(query, connection);
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {                    
                    connection.Close();
                    this.Hide();
                    Form1 newForm1 = new Form1();
                    newForm1.ShowDialog();
                    this.Close();
                }
            }
            catch (Exception error)
            {
                toolStripStatusLabel2.Text = "Ошибка соединения!\nMessage error: " + error.Message;
            }                     
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                using (StreamWriter strWr = new StreamWriter("userParametresConnection.txt"))
                {
                    strWr.WriteLine(textBox3.Text + "{0}" + textBox4.Text + "{0}" + textBox5.Text + "{0}" + textBox6.Text + "{0}" + textBox7.Text, Environment.NewLine);
                }
            }
            else if (!checkBox1.Checked)
            {
                using (StreamWriter strWr = new StreamWriter("userParametresConnection.txt"))
                {
                    strWr.WriteLine("127.0.0.1{0}5432{0}postgres{0}postgres{0}MyFirstBase", Environment.NewLine);
                }                               
            }
        }

        
    }
}
