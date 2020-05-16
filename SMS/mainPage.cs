using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMS
{
    public partial class mainPage : Form
    {
        public mainPage()
        {
            InitializeComponent();
        }

        private void mainFrom_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
        }

        private void buttonLogOut_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Do you really want to Log Out?", "LogOut", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                this.Close();
                logIn pageObj = new logIn();
                pageObj.Show();
            }
        }

        private void buttonAddItems_Click(object sender, EventArgs e)
        {
            this.Close();
            product pageObj = new product();
            pageObj.Show();
        }

        private void buttonSales_Click(object sender, EventArgs e)
        {
            this.Close();
            Sales pageObj = new Sales();
            pageObj.Show();

        }

        private void buttonSuppliers_Click(object sender, EventArgs e)
        {
            this.Close();
            Suppliers pageObj = new Suppliers();
            pageObj.Show();
        }

        private void buttonSalesReport_Click(object sender, EventArgs e)
        {
            this.Close();
            salesReport pageObj = new salesReport();
            pageObj.Show();
        }
    }
}
