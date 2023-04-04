using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using System.Data.SqlClient;

namespace Library
{
    public partial class ScanQR : Form
    {
        public ScanQR()
        {
            InitializeComponent();
        }
        FilterInfoCollection FilterInfoCollection;
        VideoCaptureDevice VideoCaptureDevice;
        private void button1_Click(object sender, EventArgs e)
        {
            VideoCaptureDevice = new VideoCaptureDevice(FilterInfoCollection[comboBox1.SelectedIndex].MonikerString);
            VideoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            VideoCaptureDevice.Start();
        }
        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }
        private void ScanQR_Load(object sender, EventArgs e)
        {
            FilterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in FilterInfoCollection)
                comboBox1.Items.Add(filterInfo.Name);

            comboBox1.SelectedIndex = 0;
            timer1.Start();
        }

        private void ScanQR_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (VideoCaptureDevice.IsRunning == true)
            {
                VideoCaptureDevice.Stop();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            using (SqlConnection con = connection.CONN())
            {
                if (pictureBox1.Image != null)
                {
                    BarcodeReader barcodeReader = new BarcodeReader();
                    Result result = barcodeReader.Decode((Bitmap)pictureBox1.Image);
                    if (result != null)
                    {
                        textBox1.Text = result.ToString();
                        timer1.Stop();
                        if (VideoCaptureDevice.IsRunning == true)
                        {
                            VideoCaptureDevice.Stop();
                        }

                        SqlCommand cmd = new SqlCommand("select * from [dbo].[ImportData] where [QrCode] = '" + textBox1.Text + "'", con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds1 = new DataSet();
                        da.Fill(ds1);
                        int i = ds1.Tables[0].Rows.Count;
                        if (i > 0)
                        {
                            string Name = Convert.ToString(ds1.Tables[0].Rows[0]["Name"]);
                            string RegNo = Convert.ToString(ds1.Tables[0].Rows[0]["RegNo"]);
                            string Form = Convert.ToString(ds1.Tables[0].Rows[0]["Form"]);                        
                            VideoCaptureDevice.Stop();
                           
                            Books books = new Books();
                            books.TxtName.Text = Name;
                            books.TxtRegNo.Text = RegNo;
                            books.TxtForm.Text = Form;                         
                            books.Show();
                            this.Close();

                        }
                        else
                        {
                            MessageBox.Show("Error!!!!!!!Code Doesn't Exist!!!");
                        }
                    
                    }
                }
             
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Books books = new Books();
            books.Show();
        }
    }
}
