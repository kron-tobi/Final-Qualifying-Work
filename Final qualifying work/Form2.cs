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
        //string textIN;
        NpgsqlConnection connection;
        //NpgsqlCommand command;
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void load_service()
        {
            List<string> service_items = new List<string>();
            //checkedListBox1.
            //service_items = checkedListBox1.CheckedItems();
            
        }
    }
}
