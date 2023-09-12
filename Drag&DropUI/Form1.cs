using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drag_DropUI
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=Wings;Integrated Security=True");
        OpenFileDialog attchment;
        string filename = "";
        public Form1()
        {
            InitializeComponent();
        }
        public void Load_data()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * from Email",con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("insert into Emails Values('"+txtSEmail.Text+ "','"+txtPwd.Text+ "','"+txtSMPT.Text+"','"+txtPort.Text+"','"+txtREmail.Text+"','"+txtSub.Text+"','"+txtBody.Text+"')", con);
                cmd.ExecuteNonQuery();
                Load_data();
                con.Close();
                SmtpClient C = new SmtpClient();
                C.Port = Convert.ToInt32(txtPort.Text.Trim());
                C.Host = txtSMPT.Text.Trim();
                C.EnableSsl = checkBox1.Checked;
                C.DeliveryMethod = SmtpDeliveryMethod.Network;
                C.UseDefaultCredentials = false;
                C.Credentials = new NetworkCredential(txtSEmail.Text.Trim(), txtPwd.Text.Trim());


                MailMessage m = new MailMessage();
                m.From = new MailAddress(txtSEmail.Text.Trim());
                m.To.Add(txtREmail.Text.Trim());
                m.Subject = txtSub.Text.Trim();
                m.IsBodyHtml = checkBox1.Checked;
                m.Body = txtBody.Text.Trim();
               

                if (filename.Length >= 0)
                {
                    Attachment at = new Attachment(filename);
                    m.Attachments.Add(at);
                }
                C.Send(m);
                filename = "";
                MessageBox.Show("Email Sent");
                
               
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                attchment = new OpenFileDialog();
                attchment.Filter = "Images(.jpg,.png)|*.png;*.jpg;|pdf Files|*.pdf";
                if (attchment.ShowDialog() == DialogResult.OK)
                {
                    filename = attchment.FileName;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
