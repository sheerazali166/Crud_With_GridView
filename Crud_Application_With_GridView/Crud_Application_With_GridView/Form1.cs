using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crud_Application_With_GridView
{
    public partial class Form1 : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["csdb"].ConnectionString;
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(cs);
            string querySelect = "select * from Employee where Id = @id";
            SqlCommand cmdSelect = new SqlCommand(querySelect, conn);
            cmdSelect.Parameters.AddWithValue("@id", textBoxId.Text);

            conn.Open();
            
            SqlDataReader reader = cmdSelect.ExecuteReader();

            if (reader.HasRows == true)
            {

                MessageBox.Show($"{textBoxId.Text} Id Already Exists.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
            else
            {

                conn.Close();
                string query = "insert into Employee values(@id, @name, @gender, @age, @designation, @salary)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", textBoxId.Text);
                cmd.Parameters.AddWithValue("@name", textBoxName.Text);
                cmd.Parameters.AddWithValue("@gender", comboBoxGender.SelectedItem);
                cmd.Parameters.AddWithValue("@age", numericUpDownAge.Value);
                cmd.Parameters.AddWithValue("@designation", comboBoxDesignation.SelectedItem);
                cmd.Parameters.AddWithValue("@salary", textBoxSalary.Text);

                conn.Open();

                int n = cmd.ExecuteNonQuery();

                if (n > 0)
                {

                    MessageBox.Show("Insertion Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BindWithGridView();
                    ResetControlls();
                }
                else
                {

                    MessageBox.Show("Insertion Failed.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                conn.Close();
            }

        }

        public void BindWithGridView() {

            SqlConnection conn = new SqlConnection(cs);
            string query = "select * from Employee";
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            DataTable data = new DataTable();
            adapter.Fill(data);
            dataGridViewShowData.DataSource = data;


        }

        private void buttonDisplayData_Click(object sender, EventArgs e)
        {
            BindWithGridView();
        }

        private void dataGridViewShowData_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBoxId.Text = dataGridViewShowData.SelectedRows[0].Cells[0].Value.ToString();
            textBoxName.Text = dataGridViewShowData.SelectedRows[0].Cells[1].Value.ToString();
            comboBoxGender.Text = dataGridViewShowData.SelectedRows[0].Cells[2].Value.ToString();
            numericUpDownAge.Value = Convert.ToInt32(dataGridViewShowData.SelectedRows[0].Cells[3].Value.ToString());
            comboBoxDesignation.Text = dataGridViewShowData.SelectedRows[0].Cells[4].Value.ToString();
            textBoxSalary.Text = dataGridViewShowData.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void buttonUpdateData_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(cs);
            string query = "update Employee set Name = @name, Gender = @gender, Age = @age, Designation = @designation, Salary = @salary where Id = @id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", textBoxId.Text);
            cmd.Parameters.AddWithValue("@name", textBoxName.Text);
            cmd.Parameters.AddWithValue("@gender", comboBoxGender.SelectedItem);
            cmd.Parameters.AddWithValue("@age", numericUpDownAge.Value);
            cmd.Parameters.AddWithValue("@designation", comboBoxDesignation.SelectedItem);
            cmd.Parameters.AddWithValue("@salary", textBoxSalary.Text);

            conn.Open();

            int n = cmd.ExecuteNonQuery();

            if (n > 0)
            {

                MessageBox.Show("Updation Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BindWithGridView();
                ResetControlls();
            }
            else
            {

                MessageBox.Show("Updation Failed.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            conn.Close();
        }

        private void buttonDeleteData_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(cs);
            string query = "delete from Employee where Id = @id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", textBoxId.Text);
            conn.Open();

            int n = cmd.ExecuteNonQuery();

            if (n > 0)
            {

                MessageBox.Show("Deletion Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BindWithGridView();
                ResetControlls();
            }
            else
            {

                MessageBox.Show("Deletion Failed.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            conn.Close();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            ResetControlls();
        }

        public void ResetControlls() {

            textBoxId.Clear();
            textBoxName.Clear();
            comboBoxGender.SelectedItem = null;
            numericUpDownAge.Value = 1;
            comboBoxDesignation.SelectedItem = null;
            textBoxSalary.Clear();
            textBoxId.Focus();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(cs);
            string query = "select * from Employee where Name like @name + '%'";
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            adapter.SelectCommand.Parameters.AddWithValue("@name", textBoxSearch.Text.Trim());
            DataTable data = new DataTable();
            adapter.Fill(data);

            if (data.Rows.Count > 0)
            {

                dataGridViewShowData.DataSource = data;
            }
            else {

                MessageBox.Show("Nothing Found", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dataGridViewShowData.DataSource = null;
            }

        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(cs);
            string query = "select * from Employee where Name like @name + '%'";
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            adapter.SelectCommand.Parameters.AddWithValue("@name", textBoxSearch.Text.Trim());
            DataTable data = new DataTable();
            adapter.Fill(data);

            if (data.Rows.Count > 0)
            {

                dataGridViewShowData.DataSource = data;
            }
            else
            {

                MessageBox.Show("Nothing Found", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dataGridViewShowData.DataSource = null;
            }
        }
    }
}
