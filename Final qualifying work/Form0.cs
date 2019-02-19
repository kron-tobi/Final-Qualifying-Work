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
    public partial class Form0 : Form
    {
        public static Form0 F0;
        public List<string> dataItems = new List<string>();
        public string parametresConnections;
        public string query;
        public string textIN;
        public NpgsqlConnection connection;
        public NpgsqlCommand command;       
        public NpgsqlDataAdapter adapter;
          

        public Form0()
        {
            InitializeComponent();
            F0 = this;
        }

        private void Form0_Load(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
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
                    label6.Text = "Connection status: Successful Connection!";
                    connection.Close();
                    this.Hide();
                    Form1 newForm1 = new Form1();
                    newForm1.ShowDialog();
                    this.Close();
                }
            }
            catch (Exception error)
            {
                label6.Text = "Connection status: Error connections!\nMessage error: " + error.Message;
            }                     
            
        }
    }
}
