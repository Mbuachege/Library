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

namespace Library
{
    public partial class Books : Form
    {
        public Books()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ScanQR scanQR = new ScanQR();
            scanQR.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = connection.CONN())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO[dbo].[LibraryBooks] ([RegNo],[Name],[Form],[Book],[ShelfNo],[Date])" +
                    "VALUES (@RegNo, @Name, @Form, @Book, @ShelfNo, @Date)", con);
                cmd.Parameters.AddWithValue("@RegNo", TxtRegNo.Text.ToString());
                cmd.Parameters.AddWithValue("@Name", TxtName.Text.ToString());
                cmd.Parameters.AddWithValue("@Form", TxtForm.Text.ToString());
                cmd.Parameters.AddWithValue("@Book", TxtBookName.Text.ToString());
                cmd.Parameters.AddWithValue("@ShelfNo", TxtShelfNo.Text.ToString());
                cmd.Parameters.AddWithValue("@Date", TxtDate.Text.ToString());

                cmd.ExecuteNonQuery();
                MessageBox.Show("Added successfully!!!!!!");
                this.Close();
                LibraryDashboard libraryDashboard = new LibraryDashboard();
                libraryDashboard.Show();
            }
        }

        private void Books_Load(object sender, EventArgs e)
        {
            TxtDate.Text = DateTime.Now.ToString();
        }
    }
}
