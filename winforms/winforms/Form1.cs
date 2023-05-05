using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace winforms
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlConnection northwndconnection=null;
        private List<string[]> rows=null;
        private List<string[]> filteredList = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["datatest"].ConnectionString);    
            sqlConnection.Open();
            northwndconnection = new SqlConnection(ConfigurationManager.ConnectionStrings["northwnd"].ConnectionString);
            northwndconnection.Open();
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Products", northwndconnection);

         DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);  
            dataGridView2.DataSource = dataSet.Tables[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            SqlCommand command = new SqlCommand("INSERT INTO students(name,surname,birthday,phone,email,mistce_narodzenya) VALUES(@name,@surname,@birthday,@phone,@email,@mistce_narodzenya)", sqlConnection);
            DateTime date = DateTime.Parse(textBox3.Text);
            command.Parameters.AddWithValue("name", textBox1.Text);
            command.Parameters.AddWithValue("surname", textBox2.Text);
            command.Parameters.AddWithValue("birthday", $"{date.Month}/{date.Day}/{date.Year}");
            command.Parameters.AddWithValue("phone", textBox4.Text);
            command.Parameters.AddWithValue("email", textBox5.Text);
            command.Parameters.AddWithValue("mistce_narodzenya", textBox6.Text);
            MessageBox.Show(command.ExecuteNonQuery().ToString());
        }

        private void SELECT_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(textBox7.Text,northwndconnection);
            DataSet dataSet = new DataSet();    
            adapter.Fill(dataSet);
            dataGridView1.DataSource=dataSet.Tables[0];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            SqlDataReader dataReader = null;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT ProductName,QuantityPerUnit,UnitPrice FROM Products",northwndconnection);
                dataReader = sqlCommand.ExecuteReader();
                ListViewItem item = null;
                while (dataReader.Read())
                {
                    item =new ListViewItem(new String[] { Convert.ToString(dataReader["ProductName"]),
                        Convert.ToString(dataReader["QuantityPerUnit"]),
                        Convert.ToString(dataReader["UnitPrice"]) });
                    listView1.Items.Add(item);  
                }
            }
            catch ( Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close(); 
                }  
            }    
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"ProductName Like '%{textBox8.Text}%'";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:


                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock<=10";


                    break;
                case 1:

                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $" UnitsInStock >= 10 AND UnitsInStock <= 50 ";

                    break;
                case 2:


                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $" UnitsInStock >=50 ";


                    break;
                case 3:


                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"";


                    break;
            }
        }

        private void RefreshList(List<string[]> list)
        {
            listView2.Items.Clear();    
            foreach (string[] s in list)
            {
                listView2.Items.Add(new ListViewItem(s));
            }      
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            filteredList = rows.Where((x) => x[0].ToLower().Contains(textBox9.Text.ToLower())).ToList();
            RefreshList(filteredList);
        }
    }
}
