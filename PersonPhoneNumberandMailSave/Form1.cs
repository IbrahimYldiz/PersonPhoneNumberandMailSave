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
using System.IO;

namespace PersonPhoneNumberandMailSave
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-OC5036T\MSSQLSERVER1;Initial Catalog=PersonPhoneNumberandMailSave;Integrated Security=True");
        void list()
        {
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter("Select ID,PersonName as 'Ad',PersonSurname as 'Soyad',PhoneNumber as 'Telefon Numarası',Email as 'E-Mail' from PersonSave", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            connection.Close();

        }
        void clear()
        {
            TxtMail.Text = "";
            TxtName.Text = "";
            TxtSurname.Text = "";
            MskNumber.Text = "";
            label7.Text = "0";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            list();
        }
        void photo()
        {
            connection.Open();
            SqlCommand komut = new SqlCommand("Select Photo from PersonSave where ID=@p1", connection);
            komut.Parameters.AddWithValue("@p1", info);
            SqlDataReader dr = komut.ExecuteReader();

            while (dr.Read())
            {
                pictureBox2.ImageLocation = dr[0].ToString();
            }
            connection.Close();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            label7.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            info= dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            TxtName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            TxtSurname.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            MskNumber.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            TxtMail.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();

            photo();

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (TxtName.Text.Trim() != "" && TxtSurname.Text.Trim() != "" && MskNumber.Text.Trim() != "(   )    -  -" || TxtMail.Text.Trim() != "")
            {

                connection.Open();
                SqlCommand command = new SqlCommand("insert into PersonSave (PersonName,PersonSurname,PhoneNumber,Email,Photo) values(@p1,@p2,@p3,@p4,@p5)", connection);

                command.Parameters.AddWithValue("@p1", TxtName.Text);
                command.Parameters.AddWithValue("@p2", TxtSurname.Text);

                command.Parameters.AddWithValue("@p3", MskNumber.Text);


                command.Parameters.AddWithValue("@p4", TxtMail.Text);
                command.Parameters.AddWithValue("@p5", label8.Text);



                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Kayıt Tamamlandı");
                list();
                clear();
            }
            else
            {
                MessageBox.Show("Lütfen İletişim Bilgilerinden En Az Birini Giriniz.");
            }
        }
        string info;

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            
            if (label7.Text != "0")
            {
                DialogResult result = MessageBox.Show("Kişiyi Güncellemek İstediğinize Emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    if (MskNumber.Text.Trim() != "(   )    -  -" || TxtMail.Text.Trim() != "")
                    {
                        connection.Open();
                        SqlCommand command1 = new SqlCommand("update PersonSave set PersonName=@p1,PersonSurname=@p2,PhoneNumber=@p3,Email=@p4,Photo=@p6 where ID=@p5", connection);
                        command1.Parameters.AddWithValue("@p5", label7.Text);
                        command1.Parameters.AddWithValue("@p1", TxtName.Text);
                        command1.Parameters.AddWithValue("@p2", TxtSurname.Text);
                        
                        if (MskNumber.Text.Trim() != "")
                        {
                            command1.Parameters.AddWithValue("@p3", MskNumber.Text);
                        }
                        if (TxtMail.Text.Trim() != "")
                        {
                            command1.Parameters.AddWithValue("@p4", TxtMail.Text);
                        }
                        
                            command1.Parameters.AddWithValue("@p6", label8.Text);
                            
                        
                        command1.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show(TxtName.Text + " Kişisi Güncellendi");
                        list();
                        clear();
                        label8.Text = "label8";
                        pictureBox2.Image = null;
                    }

                    else
                    {
                        MessageBox.Show("Lütfen İletişim Bilgilerinden En Az Birini Giriniz.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen Güncellemek İstediğiniz Kişiyi Seçiniz.");
            }

        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (label7.Text != "0")
            {
                DialogResult result = MessageBox.Show("Kişisi Rehberden Silmek İstediğinize Emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {

                    connection.Open();
                    SqlCommand command2 = new SqlCommand("Delete from PersonSave where ID=@p1", connection);
                    command2.Parameters.AddWithValue("@p1", label7.Text);
                    command2.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show(TxtName.Text + " Kişisi Silindi");
                    list();
                    clear();

                }
            }
            else
            {
                MessageBox.Show("Lütfen Silmek İstediğiniz Kişiyi Seçiniz.");
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            connection.Open();
            DataTable dt = new DataTable();
            string text;

            SqlDataAdapter da1 = new SqlDataAdapter("select * from PersonSave where PersonName like'%" + textBox1.Text + "%'", connection);
            da1.Fill(dt);
            connection.Close();
            dataGridView1.DataSource = dt;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            pictureBox2.ImageLocation = openFileDialog1.FileName;
            label8.Text = openFileDialog1.FileName;
        }
    }
}
