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

namespace SMS
{
    public partial class Suppliers : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Visual Studio\Projects\Shop_Management_System\Database_Con\SMS_Database.mdf;Integrated Security=True;Connect Timeout=30"); // here wil be the path of the database. (connection string) 

        public Suppliers()
        {
            InitializeComponent();
        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();

                SqlDataAdapter sda = new SqlDataAdapter("Select count(*) from Suppliers where Suppliers_ID ='" + textBoxSupplierID.Text + "'", con); // TO fetch data 
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (textBoxSupplierID.Text == "") //if primary key is empty.
                {
                    MessageBox.Show("Please! Set Supplier ID.", "Warning!!!!");
                }
                else
                {
                    int spid = Convert.ToInt32(textBoxSupplierID.Text);

                    if (spid <= 0)
                    {
                        MessageBox.Show("Supplier ID has to be positive.", "Warning!!!");
                    }
                    //if supplierId already exits count will be 1 
                    else if (dt.Rows[0][0].ToString() == "1")
                    {
                        MessageBox.Show("The Supplier ID already Exits.", "Warning!!!");
                    }
                    else
                    {
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "insert into Suppliers values('" + textBoxSupplierID.Text + "','" + textBoxName.Text + "','" + textBoxPhone.Text + "','" + textBoxEmail.Text + "','" + textBoxAddress.Text + "','" + textBoxCompany.Text + "', '" + textBoxDescription.Text + "')";
                        cmd.ExecuteNonQuery();
                        con.Close();

                        clear_textBox();
                        disp_data();
                        MessageBox.Show("Record Inserted Successfully.");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Record Inserted Error Message!!!");
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update Suppliers set Name = '" + textBoxName.Text + "', Phone = '" + textBoxPhone.Text + "', " +
                                   "Email = '" + textBoxEmail.Text + "', Address = '" + textBoxAddress.Text + "', " +
                                   "Company = '" + textBoxCompany.Text + "', Description = '" + textBoxDescription.Text + "' where Suppliers_ID = '" + textBoxSupplierID.Text + "' ";
                cmd.ExecuteNonQuery();
                con.Close();

                disp_data();
                MessageBox.Show("Data updated Successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Data Updated Error Message!!!");
            }
        }

        public void clear_textBox()
        {
            //After inserting the records clearing the textBoxs
            textBoxSupplierID.Text = textBoxName.Text = textBoxPhone.Text = textBoxEmail.Text = textBoxAddress.Text = textBoxCompany.Text = textBoxDescription.Text = "";
        }

        public void disp_data()
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select * from Suppliers";
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Record Display Error Message!!!");
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxSupplierID.Text == "")
                {
                    MessageBox.Show("Please! Set Supplier ID.", "Warning!!!");
                }
                else
                {
                    DialogResult dialog = MessageBox.Show("Do you really want to delete?", "Delete!!!", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            con.Close();
                        }
                        con.Open();
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "delete from Suppliers where Suppliers_ID = '" + textBoxSupplierID.Text + "' ";

                        cmd.ExecuteNonQuery();
                        con.Close();

                        clear_textBox();
                        disp_data();
                        MessageBox.Show("Data Deleted Successfully.");
                    }
                    else
                    {
                        clear_textBox();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Record Display Error Message!!!");
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            clear_textBox();
        }

        int toggleShowCnt;
        private void Suppliers_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            toggleShowCnt = 0;
            //disp_data();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Do you really want to go back?", "Back", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                this.Close();
                mainPage pageObj = new mainPage();
                pageObj.Show();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                    textBoxSupplierID.Text = row.Cells[0].Value.ToString();
                    textBoxName.Text = row.Cells[1].Value.ToString();
                    textBoxPhone.Text = row.Cells[2].Value.ToString();
                    textBoxEmail.Text = row.Cells[3].Value.ToString();
                    textBoxAddress.Text = row.Cells[4].Value.ToString();
                    textBoxCompany.Text = row.Cells[5].Value.ToString();
                    textBoxDescription.Text = row.Cells[6].Value.ToString();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Cell click error!!!");
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonShow_Click(object sender, EventArgs e)
        {
            if (toggleShowCnt % 2 == 0)
            {
                buttonShow.Text = "Clear";
                disp_data();
            }
            else
            {
                buttonShow.Text = "Show";
                //To clear the datagridView;
                dataGridView1.DataSource = null;
            }
            toggleShowCnt++;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBoxDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBoxCompany_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxSupplierID_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}