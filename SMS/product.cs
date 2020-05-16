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
    public partial class product : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Visual Studio\Projects\Shop_Management_System\Database_Con\SMS_Database.mdf;Integrated Security=True;Connect Timeout=30"); // here wil be the path of the database. (connection string) 

        int toggleShowCnt = 0;

        public product()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public bool check_dataTypeInt()
        {
            return true;
        }


        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxProductID.Text == "")
                {
                    MessageBox.Show("Please! Set Product ID.", "Warning!!!");
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

                        //Before deleting a product ID, we need to confirm that there is no sales ID in sales along with that product ID.
                        SqlDataAdapter sda = new SqlDataAdapter("Select count(*) from Sales where Product_ID ='" + textBoxProductID.Text + "'", con); // TO fetch data 
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        //if productId already exits count will >= 1
                        if (dt.Rows[0][0].ToString() == "0")
                        {
                            cmd.CommandText = "delete from Product where Product_ID = '" + textBoxProductID.Text + "' ";
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Data Deleted Successfully.");
                        }
                        else //The Product ID is found in sales.
                        {
                            MessageBox.Show("The Product ID Exits in sales. After deleting the sales IDs from Sales (Select ''Delete Sales'' Option), the Product ID can be deleted from Product.", "Warning!!!");
                        }
                        con.Close();

                        clear_textBox();
                        disp_data();
                    }
                    else
                    {
                        clear_textBox();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Data Deleted Error Message.");
            }
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

                SqlDataAdapter sda = new SqlDataAdapter("Select count(*) from Product where Product_ID ='" + textBoxProductID.Text + "'", con); // TO fetch data 
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (textBoxProductID.Text == "") //if primary key is empty.
                {
                    MessageBox.Show("Please! Set Product ID.", "Warning!!!!");
                }
                else
                {
                    int pid = Convert.ToInt32(textBoxProductID.Text);
                    int quantity = Convert.ToInt32(textBoxQuantity.Text);

                    if (pid <= 0)
                    {
                        MessageBox.Show("Product ID has to be positive.", "Warning!!!");
                    }
                    else if (quantity <= 0)
                    {
                        MessageBox.Show("Product Quantity has to be positive.", "Warning!!!");
                    }
                    //if productId already exits count will be 1 
                    else if (dt.Rows[0][0].ToString() == "1")
                    {
                        MessageBox.Show("The Product ID already Exits.", "Warning!!!");
                    }
                    else if (!(check_dataTypeInt()))
                    {

                    }
                    else
                    {
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "insert into Product values('" + textBoxProductID.Text + "','" + textBoxProductName.Text + "','" + textBoxPurchasePrice.Text + "','" + textBoxSalesPrice.Text + "','" + textBoxQuantity.Text + "','" + textBoxGroupName.Text + "', '" + textBoxCompanyName.Text + "', '" + textBoxShelfNo.Text + "')";
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
                MessageBox.Show(ex.Message,"Record Inserted Error Message!!!");
            }
        }

        public void clear_textBox()
        {
            //After inserting the records clearing the textBoxs
            textBoxProductID.Text = textBoxProductName.Text = textBoxPurchasePrice.Text = textBoxSalesPrice.Text = textBoxQuantity.Text = textBoxGroupName.Text = textBoxCompanyName.Text = textBoxShelfNo.Text = "";
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
                cmd.CommandText = "Select * from Product";
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

        private void AddItems_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            //disp_data();
            toggleShowCnt = 0;
        }

        private void buttonShow_Click(object sender, EventArgs e)
        {
            if(toggleShowCnt%2 == 0) 
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
                cmd.CommandText = "update Product set Product_Name = '"+textBoxProductName.Text+"', Purchase_Price = '"+textBoxPurchasePrice.Text+"', " +
                                   "Sales_Price = '"+textBoxSalesPrice.Text+"', Quantity = '"+textBoxQuantity.Text+"', " +
                                   "Group_Name = '"+textBoxGroupName.Text+"', Company_Name = '"+textBoxCompanyName.Text+"',  Shelf_No = '"+textBoxShelfNo.Text+"' where Product_ID = '"+textBoxProductID.Text+"' ";
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


        private void buttonClear_Click(object sender, EventArgs e)
        {
            clear_textBox();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                    textBoxProductID.Text = row.Cells[0].Value.ToString();
                    textBoxProductName.Text = row.Cells[1].Value.ToString();
                    textBoxPurchasePrice.Text = row.Cells[2].Value.ToString();
                    textBoxSalesPrice.Text = row.Cells[3].Value.ToString();
                    textBoxQuantity.Text = row.Cells[4].Value.ToString();
                    textBoxGroupName.Text = row.Cells[5].Value.ToString();
                    textBoxCompanyName.Text = row.Cells[6].Value.ToString();
                    textBoxShelfNo.Text = row.Cells[7].Value.ToString();
                }
            }
            catch (Exception)
            { 
                MessageBox.Show("Cell click error!!!");
            }
        }
    }
}
