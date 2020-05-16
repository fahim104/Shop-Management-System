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
    public partial class logIn : Form
    {
        public logIn()
        {
            InitializeComponent();
        }

        private void textBoxUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonLogIn_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Visual Studio\Projects\Shop_Management_System\Database_Con\SMS_Database.mdf;Integrated Security=True;Connect Timeout=30"); // here wil be the path of the database. (connection string) 
            SqlDataAdapter sda = new SqlDataAdapter("Select count(*) from LogIn where Username ='" + textBoxUsername.Text + "' and Password ='" + textBoxPassword.Text + "' ",con); // To fetch data 
            DataTable dt = new DataTable();
            sda.Fill(dt);
            /* 
             * if username and password matches any one from the logIn table then logIn will happen. 
            */
            if(dt.Rows[0][0].ToString() == "1") 
            {
                this.Hide();
                mainPage pageObj = new mainPage();
                pageObj.Show();
            }
            else
            {
                MessageBox.Show("Check your Username and Password.");
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Do you really want to exit?","Exit",MessageBoxButtons.YesNo);
            if(dialog == DialogResult.Yes)
            {   
                //this.close();
                Application.Exit();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBoxUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void logIn_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
        }
    }
}
