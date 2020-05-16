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
    public partial class Sales : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Visual Studio\Projects\Shop_Management_System\Database_Con\SMS_Database.mdf;Integrated Security=True;Connect Timeout=30"); // here wil be the path of the database. (connection string) 

        public Sales()
        {
            InitializeComponent();
        }

        //only used in textBoxSearchChanged...
        DataTable DT;
        //used in disp_data();
        int maxSalesID;

        private void Sales_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;

            totalCost.Text = 0.ToString();
            maxSalesID = getMaxSalesID();
            disp_data();
            dispDataFromProduct();

            //To display time
            timer1.Start();
            labelTime.Text = DateTime.Now.ToLongTimeString();
            //But this will only show fixed current time. (when the form is clicked). So need a timer.


            //To display date
            labelDate.Text = DateTime.Now.ToLongDateString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelTime.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }

        int num;
        public int getMaxSalesID()
        {
            //To show Sales_ID
            try
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();

                SqlDataAdapter sda = new SqlDataAdapter("Select MAX(cast(Sales_ID as int)) from Sales", con); // TO fetch data 
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows[0][0] != DBNull.Value)
                {
                    num = Convert.ToInt32(dt.Rows[0][0].ToString());
                    num = num + 1;
                }
                else
                {
                    num = 1;
                }
                con.Close();
                return num;
                //MessageBox.Show("Sales ID and Date has been set up successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Set Error Message!!!");
                return -1;
            }
        }
        public void setDate()
        {
            //To show Date
            textBoxDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }
        private void buttonSet_Click(object sender, EventArgs e)
        {
            setDate();
            num = getMaxSalesID();
            textBoxSalesID.Text = num.ToString();
        }


        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        public void calculateTotalCost()
        {
            float total = 0, cost, temp;
            int quantity;
            
            //why .count-1 ???
            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
            {
                try
                {
                    cost = float.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                    quantity = int.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                    temp = cost * (float)quantity;

                    //MessageBox.Show("Multiplication is done.");
                    total += float.Parse((temp.ToString()));
                    //MessageBox.Show("Total parse is passed.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "CalculationTotalCost is not done! Getting Error.");
                    
                }
               
            }
            if(total.ToString() != null) 
            {
                totalCost.Text = total.ToString();
            }
            else
            {
                totalCost.Text = "0";
            }
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
                cmd.CommandText = "Select *, Sales.Quantity*Sales.Sales_Price as COST from Sales where Sales_ID >= '"+ maxSalesID.ToString() +"'";
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

        public void dispDataFromProduct()
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

                DT = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(DT);
                dataGridView2.DataSource = DT;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Record Display Error Message!!!");
            }
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

        public void clear_textBox()
        {
            //After inserting the records clearing the textBoxs
            textBoxSalesID.Text = textBoxProductID.Text = textBoxProductName.Text = textBoxPurchasePrice.Text = textBoxSalesPrice.Text = textBoxQuantity.Text = textBoxDate.Text = "";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                    textBoxSalesID.Text = row.Cells[0].Value.ToString();
                    textBoxProductID.Text = row.Cells[1].Value.ToString();
                    textBoxProductName.Text = row.Cells[2].Value.ToString();
                    textBoxPurchasePrice.Text = row.Cells[3].Value.ToString();
                    textBoxSalesPrice.Text = row.Cells[4].Value.ToString();
                    textBoxQuantity.Text = row.Cells[5].Value.ToString();
                    textBoxDate.Text = row.Cells[6].Value.ToString();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Cell click error!!!");
            }
        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {

            try
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();

                SqlDataAdapter sda = new SqlDataAdapter("Select count(*) from Sales where Sales_ID ='" + textBoxSalesID.Text + "'", con); // TO fetch data 
                DataTable dt = new DataTable();
                sda.Fill(dt);

                SqlDataAdapter sa = new SqlDataAdapter("Select count(*) from Product where Product_ID = '" + textBoxProductID.Text + "'", con);
                DataTable t = new DataTable();
                sa.Fill(t);
                /* 
                    * if sales_Id already exits count will be 1 
                */
                if (t.Rows[0][0].ToString() == "1")
                {
                    if (dt.Rows[0][0].ToString() == "1")
                    {
                        MessageBox.Show("The Sales ID already Exits.", "Error!!!");
                    }
                    else if (textBoxSalesID.Text == "") //if primary key is empty.
                    {
                        MessageBox.Show("Please! Set Sales ID.", "Error!!!");
                    }
                    else if(textBoxQuantity.Text == "")
                    { 
                        MessageBox.Show("Please! Set Quantity.", "Error!!!");
                    }
                    else
                    {
                        //Sales ID has to be positive.
                        int sid = Convert.ToInt32(textBoxSalesID.Text);
                        if(sid <= 0)
                        {
                            MessageBox.Show("Sales ID has to be positive.","Warning!!!");
                        }

                        SqlDataAdapter sqlDA = new SqlDataAdapter("Select Quantity from Product where Product_ID ='" + textBoxProductID.Text + "'", con); // TO fetch data 
                        DataTable dataT = new DataTable();
                        sqlDA.Fill(dataT);
                        int noOfProductsInStock = Convert.ToInt32(dataT.Rows[0][0]);

                        //Product Quantity has to be positive.
                        int pQuantity = Convert.ToInt32(textBoxQuantity.Text);
                        if(pQuantity <= 0) 
                        {
                            MessageBox.Show("Quantity has to be positive.", "Warning!!!");
                        }
                        else if (pQuantity > noOfProductsInStock) // Product's quantity needs to be <= No of products in the stock.
                        {
                            //MessageBox.Show(pQuantity.ToString(),"Quantity");
                            //MessageBox.Show(noOfProductsInStock.ToString(), "No of product in the stock");
                            MessageBox.Show("Product's Quantity > the no of Quantity in the stock.", "Warning!!!");
                        }
                        else
                        {
                            SqlCommand cmd = con.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "insert into Sales values('" + textBoxSalesID.Text + "','" + textBoxProductID.Text + "','" + textBoxProductName.Text + "','" + textBoxPurchasePrice.Text + "','" + textBoxSalesPrice.Text + "','" + textBoxQuantity.Text + "','" + textBoxDate.Text + "')";
                            cmd.ExecuteNonQuery();
                           
                            MessageBox.Show("Record Inserted Successfully.");

                            //********
                            //If a record is inserted in sales, quantity in product table needs to be updated.
                            int newProductQuantity = noOfProductsInStock - pQuantity;
                            cmd = con.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            textBoxQuantity.Text = newProductQuantity.ToString();
                            cmd.CommandText = "update Product set Quantity = '" + textBoxQuantity.Text + "' where Product_ID = '" + textBoxProductID.Text + "' ";
                            cmd.ExecuteNonQuery();
                            con.Close();

                            clear_textBox();
                            disp_data();
                            calculateTotalCost();
                            dispDataFromProduct();
                        }
                    }
                    con.Close();
                }
                else
                {
                    MessageBox.Show("The Product_ID doesn't exist in Product table. Select the Product_ID carefully.","Error!!!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Record Inserted Error Message!!!");
            }
        }

        public int noOfProductsFromProductTable(int productID)
        {
            //If a record is updated in sales, quantity in product table needs to be updated.
            SqlDataAdapter sqlDA = new SqlDataAdapter("Select Quantity from Product where Product_ID ='" + productID.ToString() + "'", con); // TO fetch data 
            DataTable dataT = new DataTable();
            sqlDA.Fill(dataT);
            int noOfProductsInStock = Convert.ToInt32(dataT.Rows[0][0]);
            return noOfProductsInStock;
        }

        public int noOfProductFromSales(int SalesID)
        {
            //If a record is updated in sales, quantity in product table needs to be updated.
            SqlDataAdapter DA = new SqlDataAdapter("Select Quantity from Sales where Sales_ID ='" + SalesID.ToString() + "'", con); // TO fetch data 
            DataTable dT = new DataTable();
            DA.Fill(dT);
            int saleQuantity = Convert.ToInt32(dT.Rows[0][0]);
            return saleQuantity;
        }

        private void textBoxSalesID_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

       

        private void buttonClear_Click(object sender, EventArgs e)
        {
            clear_textBox();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if(con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();

                int noOfProductsInStock = noOfProductsFromProductTable(Convert.ToInt32(textBoxProductID.Text));
                int saleQuantity = noOfProductFromSales(Convert.ToInt32(textBoxSalesID.Text));
                int newProductQuantity = noOfProductsInStock + saleQuantity - (Convert.ToInt32(textBoxQuantity.Text));

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update Product set Quantity = '" + newProductQuantity.ToString() + "' where Product_ID = '" + textBoxProductID.Text + "' ";
                cmd.ExecuteNonQuery();

                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update Sales set Product_ID = '"+textBoxProductID.Text+"', Product_Name = '" + textBoxProductName.Text + "', Purchase_Price = '" + textBoxPurchasePrice.Text + "', " +
                                   "Sales_Price = '" + textBoxSalesPrice.Text + "', Quantity = '" + textBoxQuantity.Text + "', Date = '"+textBoxDate.Text+"' where Sales_ID = '"+textBoxSalesID.Text+"'";
                cmd.ExecuteNonQuery();

                con.Close();

                clear_textBox();
                disp_data();
                calculateTotalCost();
                dispDataFromProduct();
                MessageBox.Show("Data updated Successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Data Updated Error Message!!!");
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxSalesID.Text == "")
                {
                    MessageBox.Show("Please! Set Sales ID.", "Error!!!");
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
                        cmd.CommandText = "delete from Sales where Sales_ID = '" + textBoxSalesID.Text + "' ";
                        cmd.ExecuteNonQuery();

                        //If a record is deleted from sales, quantity in product table needs to be updated.
                        SqlDataAdapter sqlDA = new SqlDataAdapter("Select Quantity from Product where Product_ID ='" + textBoxProductID.Text + "'", con); // TO fetch data 
                        DataTable dataT = new DataTable();
                        sqlDA.Fill(dataT);
                        int noOfProductsInStock = Convert.ToInt32(dataT.Rows[0][0]);

                     
                        int pQuantity = Convert.ToInt32(textBoxQuantity.Text);
                        int newProductQuantity = noOfProductsInStock + pQuantity;

                        cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        textBoxQuantity.Text = newProductQuantity.ToString();
                        cmd.CommandText = "update Product set Quantity = '" + textBoxQuantity.Text + "' where Product_ID = '" + textBoxProductID.Text + "' ";
                        cmd.ExecuteNonQuery();
                        con.Close();

                        clear_textBox();
                        disp_data();
                        calculateTotalCost();
                        dispDataFromProduct();
                        MessageBox.Show("Data Deleted Successfully.");
                    }
                    else
                    {
                        clear_textBox();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Data Deleted Error Message!!!");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void searchTextEnter(object sender, EventArgs e)
        {
            if (textBoxSearch.Text == "Example: Pendrive (Search by Group Name )")
            {
                textBoxSearch.Text = "";
                textBoxSearch.ForeColor = Color.Black;
            }
        }

        private void searchTextLeave(object sender, EventArgs e)
        {
            if(textBoxSearch.Text == "")
            {
                textBoxSearch.Text = "Example: Pendrive (Search by Group Name )";
                textBoxSearch.ForeColor = Color.SlateGray;
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];

                    //textBoxSalesID.Text = row.Cells[0].Value.ToString();
                    textBoxProductID.Text = row.Cells[0].Value.ToString();
                    textBoxProductName.Text = row.Cells[1].Value.ToString();
                    textBoxPurchasePrice.Text = row.Cells[2].Value.ToString();
                    textBoxSalesPrice.Text = row.Cells[3].Value.ToString();
                    //textBoxQuantity.Text = row.Cells[5].Value.ToString();
                    //dateTimePicker1.Value = row.Cells[6].Value.ToString();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Cell click error!!!");
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("search text changed!!!");

            DataView dv = new DataView(DT);
            dv.RowFilter = String.Format("Group_Name LIKE '%{0}%'", textBoxSearch.Text);
            dataGridView2.DataSource = dv;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonClear_Click_1(object sender, EventArgs e)
        {
            clear_textBox();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBoxSalesID_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBoxQuantity_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxSalesPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxPurchasePrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxProductName_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxProductID_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void buttonClearTable_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Do you really want to Clear Table?", "Clear Table", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                dataGridView1.DataSource = null;
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void totalCost_Click(object sender, EventArgs e)
        {

        }

        private void buttonDeleteAllSalesID_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxProductID.Text == "")
                {
                    MessageBox.Show("Please! Set Product ID.", "Error!!!");
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
                        cmd.CommandText = "delete from Sales where Product_ID = '" + textBoxProductID.Text + "' ";
                        cmd.ExecuteNonQuery();
                        con.Close();

                        clear_textBox();
                        disp_data();
                        calculateTotalCost();
                        dispDataFromProduct();
                        MessageBox.Show("Data Deleted Successfully.");
                    }
                    else
                    {
                        clear_textBox();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Data Deleted Error Message!!!");
            }
        }
    }
}
