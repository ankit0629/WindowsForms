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

namespace WindowsForms
{
    public partial class WinForm : Form
    {
        // Server name - DESKTOP-QRB46S1\SQLEXPRESS
        string path = "Data Source=DESKTOP-QRB46S1\\SQLEXPRESS;initial catalog=person;integrated security=true";
        SqlConnection sqlConnection;
        SqlCommand cmd;
        DataTable dataTable;
        SqlDataAdapter adpt;
        //Unique ID i.e user_id
        int ID;


        public WinForm()
        {
            InitializeComponent();
            textName.MaxLength = 30;
            sqlConnection = new SqlConnection(path);
            displayGridView();

            // Button can only be enabled only if you click on the grid view data
            btnUpdate.Enabled = false;
            button2.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (textName.Text == "")
            {
                MessageBox.Show("Please enter your name", "Name Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textNumber.Text == "")
            {
                MessageBox.Show("Please enter the mobile number", "Mobile Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!radioMale.Checked && !radioFemale.Checked)
            {
                MessageBox.Show("Please select Gender", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textHobbies.Text == "")
            {
                MessageBox.Show("Please enter your hobbies", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textNumber.Text.Length < 10)
            {

                MessageBox.Show("Please enter 10 digit mobile number", "Mobile number Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                try
                {
                    string gender;
                    if (radioMale.Checked)
                    {
                        gender = "Male";
                    }
                    else
                    {
                        gender = "Female";
                    }
                    sqlConnection.Open();
                    cmd = new SqlCommand("insert into users (user_name,gender,ph_number,hobbies) values ('" + textName.Text + "','" + gender + "','" + textNumber.Text + "','" + textHobbies.Text + "')", sqlConnection);
                    cmd.ExecuteNonQuery();
                    sqlConnection.Close();
                    MessageBox.Show("Your Data has been saved");
                    clearData();
                    displayGridView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }
        }
        private void textName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                    DialogResult daName = MessageBox.Show("Pleae enter valid name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void textNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsDigit(e.KeyChar)&& e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                    DialogResult daNumber = MessageBox.Show("Please enter valid mobile number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void textHobbies_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar)&& e.KeyChar != (char)Keys.Back && !char.IsSeparator(e.KeyChar) && !char.IsPunctuation(e.KeyChar))
                {
                    e.Handled = true;
                    DialogResult daHobbies = MessageBox.Show("Enter valid details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                labelCountText.Text = (textHobbies.Text.Length+1).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void textNumber_TextChanged(object sender, EventArgs e)
        {
            if (textNumber.Text.Length == 10)
            {
                textNumber.ForeColor = Color.Black;
            }
            else {
                textNumber.ForeColor = Color.Red;
            }
        }

        public void displayGridView() {
            try
            {
                dataTable = new DataTable();
                sqlConnection.Open();
                adpt = new SqlDataAdapter("select * from users", sqlConnection);
                adpt.Fill(dataTable);
                dataGridView.DataSource = dataTable;
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        public void clearData() {
            textName.Text = "";
            textNumber.Text = "";
            textHobbies.Text = "";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string gender;
                if (radioMale.Checked)
                {
                    gender = "Male";
                }
                else
                {
                    gender = "Female";
                }
                sqlConnection.Open();
                cmd = new SqlCommand("update users set user_name='" + textName.Text + "',gender='" + gender + "',ph_number='" + textNumber.Text + "',hobbies='" + textHobbies.Text + "' where user_id='"+ID+"'", sqlConnection);
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                MessageBox.Show("Your Data has been updated");
                
                displayGridView();
                clearData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void labelCountText_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ID = int.Parse(dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString());
            textName.Text= dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
            radioMale.Checked = true;
            radioFemale.Checked = false;

            if (dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString()=="Female") {
                radioMale.Checked = false;
                radioFemale.Checked = true;

            }

            textNumber.Text= dataGridView.Rows[e.RowIndex].Cells[3].Value.ToString();
            textHobbies.Text = dataGridView.Rows[e.RowIndex].Cells[4].Value.ToString();

            // Button can only be enabled only if you click on the grid view data
            btnUpdate.Enabled = true;
            button2.Enabled = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            sqlConnection.Open();
            adpt=new SqlDataAdapter("select * from users  where user_name like '%"+ textSearch.Text + "%' or user_id like '%" + textSearch.Text + "%'",sqlConnection);
            dataTable = new DataTable();
            adpt.Fill(dataTable);
            dataGridView.DataSource = dataTable;
            sqlConnection.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
               
                sqlConnection.Open();
                cmd = new SqlCommand("delete from users where user_id='" + ID + "'", sqlConnection);
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                MessageBox.Show("Your Data has been Deleted");

                displayGridView();
                clearData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
    }

    // Thank You 
}
