using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using ClosedXML.Excel;
using System.Configuration;



namespace StudentDataBase
{
    public partial class Form1 : Form
    {
        string connectionString = "Data Source =.; Initial Catalog = Students; Integrated Security = True";
        int justLoaded = 0;


        public Form1()
        {
            InitializeComponent();
            txtboxFirstName.Focus();
            ShowData();
   
        }


        public void ShowData()
        { 
            DataTable dt = new DataTable();
            string select = "SELECT * FROM Students";

            SqlConnection c = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(select, c);

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

            dataAdapter.Fill(dt);
            dataGridViewInfo.ReadOnly = true;
            dataGridViewInfo.DataSource = dt;
            dataGridViewInfo.AutoGenerateColumns = true;
            dataGridViewInfo.Refresh();
            this.dataGridViewInfo.Columns["ID"].Visible = false;
            dataGridViewInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            DataSet appdata = new DataSet();
           
          
        }

        private void dataGridViewInfo_SelectionChanged_1(object sender, EventArgs e)
        {
            if (dataGridViewInfo.SelectedRows.Count > 0)
            {

                string First_Name = dataGridViewInfo.SelectedRows[0].Cells[1].Value + string.Empty;
                string Last_Name = dataGridViewInfo.SelectedRows[0].Cells[2].Value + string.Empty;
                string Degree = dataGridViewInfo.SelectedRows[0].Cells[3].Value + string.Empty;
                string Year = dataGridViewInfo.SelectedRows[0].Cells[4].Value + string.Empty;

                txtboxFirstName.Text = First_Name;
                txtboxLastName.Text = Last_Name;
                cmbboxDegree.Text = Degree;
                txtboxYear.Text = Year;

                if (justLoaded < 2)
                {
                    txtboxFirstName.Clear();
                    txtboxLastName.Clear();
                    txtboxYear.Clear();
                    cmbboxDegree.SelectedIndex = -1;
                    txtboxFirstName.Focus();

                    justLoaded++;
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if(txtboxFirstName.Text.Length == 0 && txtboxLastName.Text.Length == 0 && txtboxYear.Text.Length == 0)
            {
                cmbboxDegree.SelectedIndex = -1;
                txtboxFirstName.Focus();
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Everything you introduced will be deleted forever. Do you continue?", "WARNING", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    txtboxFirstName.Clear();
                    txtboxLastName.Clear();
                    txtboxYear.Clear();
                    cmbboxDegree.SelectedIndex = -1;
                    txtboxFirstName.Focus();
                }
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string insert_string = "INSERT INTO Students (First_Name, Last_Name, Degree, Year) VALUES( @First_Name, @Last_Name, @Degree, @Year)";
                SqlCommand command = new SqlCommand(insert_string, connection);


                command.Parameters.Add("@First_Name", SqlDbType.VarChar);
                command.Parameters["@First_Name"].Value = txtboxFirstName.Text;

                command.Parameters.Add("@Last_Name", SqlDbType.VarChar);
                command.Parameters["@Last_Name"].Value = txtboxLastName.Text;

                command.Parameters.Add("@Degree", SqlDbType.VarChar);
                command.Parameters["@Degree"].Value = cmbboxDegree.SelectedItem;

                command.Parameters.Add("@Year", SqlDbType.VarChar);
                command.Parameters["@Year"].Value = txtboxYear.Text;

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    dataGridViewInfo.Refresh();
                    ShowData();
                }
                catch (Exception) { };

                if (connection.State == System.Data.ConnectionState.Open) connection.Close();

                txtboxFirstName.Clear();
                txtboxLastName.Clear();
                txtboxYear.Clear();
                cmbboxDegree.SelectedIndex = -1;
                txtboxFirstName.Focus();
            }


        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if  (txtboxFirstName.Text.Length != 0 && txtboxLastName.Text.Length != 0 && txtboxYear.Text.Length != 0)
            {
               DialogResult dialogResult = MessageBox.Show("Everything you selected will be deleted forever. Do you continue?", "WARNING", MessageBoxButtons.YesNo);
               if (dialogResult == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string deleteString = "DELETE FROM Students WHERE ID = @ID";
                        SqlCommand commandDelete = new SqlCommand(deleteString, connection);
                        commandDelete.CommandType = CommandType.Text;
                        commandDelete.Parameters.AddWithValue("@ID", Convert.ToInt32(dataGridViewInfo.CurrentRow.Cells["ID"].Value));
                        commandDelete.ExecuteNonQuery();
                        ShowData();
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtboxFirstName.Text.Length != 0 && txtboxLastName.Text.Length != 0 && txtboxYear.Text.Length != 0)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string updateString = "UPDATE Students SET First_Name=@First_Name, Last_Name=@Last_Name, Degree=@Degree, Year=@Year    WHERE ID = @ID";
                    SqlCommand commandUpdate = new SqlCommand(updateString, connection);
                    commandUpdate.CommandType = CommandType.Text;
                    commandUpdate.Parameters.AddWithValue("@ID", Convert.ToInt32(dataGridViewInfo.CurrentRow.Cells["ID"].Value));
                    commandUpdate.Parameters.AddWithValue("@First_Name", txtboxFirstName.Text);
                    commandUpdate.Parameters.AddWithValue("@Last_Name", txtboxLastName.Text);
                    commandUpdate.Parameters.AddWithValue("@Degree", cmbboxDegree.SelectedItem);
                    commandUpdate.Parameters.AddWithValue("@Year", txtboxYear.Text);
                    commandUpdate.ExecuteNonQuery();
                    ShowData();
                }
            }
        }
        

       
    }
}
