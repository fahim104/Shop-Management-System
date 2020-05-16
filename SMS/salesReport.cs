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
    public partial class salesReport : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Visual Studio\Projects\Shop_Management_System\Database_Con\SMS_Database.mdf;Integrated Security=True;Connect Timeout=30"); // here wil be the path of the database. (connection string) 

        public salesReport()
        {
            InitializeComponent();
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

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonSearch_Click(object sender, EventArgs e)
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
                cmd.CommandText = "Select *, Sales.Quantity*Sales.Sales_Price as COST from Sales where Date >= '"+ textBoxSearchFrom.Text +"' AND DATE <= '"+ textBoxSearchTo.Text+"' ";
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

                dataGridView1.DataSource = dt;
                calculateTotalCost();

                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Search Build Error!!!");
            }
        }

        private void textBoxSearchFrom_TextChanged(object sender, EventArgs e)
        {

        }

        public void calculateTotalCost()
        {
            float total = 0;
            // why .Count-1????
            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
            {
                try
                {
                    total += float.Parse(dataGridView1.Rows[i].Cells[7].Value.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "calculateTotalCost Error!!!");
                }
            }
            //Show total in label
            labelTotalCost.Text = total.ToString();
        }

        private void salesReport_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;

            //To show zero initially.
            labelTotalCost.Text = 0.ToString();

            //To display date
            labelDate.Text = DateTime.Now.ToLongDateString();

            //To show Date in searchBox
            textBoxSearchFrom.Text = DateTime.Now.ToString("dd-MM-yyyy");
            textBoxSearchTo.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }

        private void textBoxSearchFrom_Leave(object sender, EventArgs e)
        {
            if (textBoxSearchFrom.Text == "")
            {
                textBoxSearchFrom.Text = DateTime.Now.ToString("dd-MM-yyyy");
                //textBoxSearchFrom.ForeColor = Color.SlateGray;
            }
        }

        private void textBoxSearchTo_Leave(object sender, EventArgs e)
        {
            if (textBoxSearchTo.Text == "")
            {
                textBoxSearchTo.Text = DateTime.Now.ToString("dd-MM-yyyy");
                //textBoxSearchFrom.ForeColor = Color.SlateGray;
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
        }

        private void buttonShow_Click(object sender, EventArgs e)
        {

        }
    }
}
