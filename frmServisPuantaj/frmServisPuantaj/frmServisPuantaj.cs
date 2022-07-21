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
using DevExpress.XtraReports.UI;
using System.Globalization;

namespace frmServisPuantaj
{
    public partial class frmServisPuantaj : Form
    {
        public frmServisPuantaj()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection("Data Source=ARZU\\SQLEXPRESS;Initial Catalog=Personel_Tracking_System;Integrated Security=True");
        private void frmServisPuantaj_Load(object sender, EventArgs e)
        {
            dateTimePicker_tarih.Value = DateTime.Now;
            dateTimePicker_baslangic.Value = DateTime.Now;
            dateTimePicker_bitis.Value = DateTime.Now;
            ServisListele();
            GecKayıtListele();
            EkKayıtListele();

        }
        //KAYIT LİSTELE xtraTAB GRID DOLDUR
        private void ServisListele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select id as 'Sıra',servis_durum as 'Servis Durum',tarih as Tarih,saat as Saat,departman as Departman,servis_sofor as 'Şoför',servis_plaka as Plaka,takip_eden as Görevli ,gec_geldi as 'Geç Geldi',ek_servis as 'Ek Servis' from tbl_Servis_Puantaj order by id desc", connection);
            da.Fill(dt);
            gridControl_Servisler.DataSource = dt;

        }
        private void GecKayıtListele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select id as 'Sıra', tarih as Tarih,saat as Saat,departman as Departman,servis_sofor as 'Şoför',servis_plaka as Plaka,takip_eden as Görevli ,gec_geldi as 'Geç Geldi', servis_not as 'Not', ek_servis as 'Ek Servis' from tbl_Servis_Puantaj where gec_geldi='True' order by id desc", connection);
            da.Fill(dt);
            gridControl_GecKayitlar.DataSource = dt;
        }
        private void EkKayıtListele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select id as 'Sıra', tarih as Tarih,saat as Saat,departman as Departman, servis_sofor as 'Şoför',servis_plaka as Plaka,takip_eden as Görevli ,gec_geldi as 'Geç Geldi',ek_servis as 'Ek Servis' from tbl_Servis_Puantaj where ek_servis='True' order by id desc", connection);
            da.Fill(dt);
            gridControl_EkKayitlar.DataSource = dt;
        }

        //KAYIT EKLE
        private void radioButton_Personel_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Personel.Checked == true)
            {
                lbl_departman_radio_button.Text = "Personel";

            }
        }

        private void radioButton_Dokuma_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Dokuma.Checked == true)
            {
                lbl_departman_radio_button.Text = "Dokuma";


            }
        }

        private void radioButton_Iplik_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Iplik.Checked == true)
            {
                lbl_departman_radio_button.Text = "İplik";
            }
        }
        private void radioButton_Desen_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Desen.Checked == true)
            {
                lbl_departman_radio_button.Text = "Desen";
            }
        }

        private void checkBox_ek_servis_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_ek_servis.Checked == true)
            {
                label_ek_servis.Text = "True";
            }
            else
            {
                label_ek_servis.Text = "False";
            }
        }
        private void checkBox_gec_geldi_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_gec_geldi.Checked == true)
            {
                label_gec_geldi.Text = "True";
            }
            else
            {
                label_gec_geldi.Text = "False";
            }
        }

        private void simpleButton_kaydet_Click(object sender, EventArgs e)
        {
           
            //Haftanın günlerini türkçeye çevir
            CultureInfo turkey = new CultureInfo("tr-TR");
            string day = turkey.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek).ToString();

            if (combobox_sefer_durum.Text != "" && timeEdit1.Text != "" && lbl_departman_radio_button.Text != "" &&
                comboBox_arac_plaka.Text != "" && textBox_sofor_isim.Text != "" && textBox_takip_eden.Text != "")
            {
                try
                {
                    connection.Open();
                    SqlCommand kayit_ekle_command = new SqlCommand("insert into tbl_Servis_Puantaj (servis_durum,saat,tarih,gün,departman,servis_plaka,servis_sofor,takip_eden,gec_geldi,servis_not,ek_servis) values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11)", connection);
                    kayit_ekle_command.Parameters.AddWithValue("@p1", combobox_sefer_durum.Text);
                    kayit_ekle_command.Parameters.AddWithValue("@p2", timeEdit1.Text);
                    kayit_ekle_command.Parameters.AddWithValue("@p3", dateTimePicker_tarih.Text);
                    kayit_ekle_command.Parameters.AddWithValue("@p4", day);
                    kayit_ekle_command.Parameters.AddWithValue("@p5", lbl_departman_radio_button.Text);
                    kayit_ekle_command.Parameters.AddWithValue("@p6", comboBox_arac_plaka.Text);
                    kayit_ekle_command.Parameters.AddWithValue("@p7", textBox_sofor_isim.Text);
                    kayit_ekle_command.Parameters.AddWithValue("@p8", textBox_takip_eden.Text);
                    kayit_ekle_command.Parameters.AddWithValue("@p9", label_gec_geldi.Text);
                    kayit_ekle_command.Parameters.AddWithValue("@p10", richTextBox_kayıt_not_.Text);
                    kayit_ekle_command.Parameters.AddWithValue("@p11", label_ek_servis.Text);
                    kayit_ekle_command.ExecuteNonQuery();
                    connection.Close();
                    ServisListele();
                    GecKayıtListele();
                    EkKayıtListele();
                    MessageBox.Show("Kayıt Eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Lütfen hatalı bilgi girişi yapmadığınızdan emin olup tekrar kayıt eklemeyi deneyiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Lütfen tüm bilgileri eksiksiz doldurduğunuzdan emin olup tekrar kayıt eklemeyi deneyiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //araçları temizle
            richTextBox_kayıt_not_.ResetText();
            comboBox_arac_plaka.ResetText();
            textBox_sofor_isim.ResetText();
            checkBox_ek_servis.Checked = false;
            checkBox_gec_geldi.Checked = false;

        }
        //
        //RAPORLAMA
        //
        private void filtre_personel_CheckedChanged(object sender, EventArgs e)
        {
            if (filtre_personel.Checked == true)
            {
                label_rapor.Text = "Personel";
            }
        }

        private void filtre_iplik_CheckedChanged(object sender, EventArgs e)
        {
            if (filtre_iplik.Checked == true)
            {
                label_rapor.Text = "İplik";
            }
        }

        private void filtre_dokuma_CheckedChanged(object sender, EventArgs e)
        {
            if (filtre_dokuma.Checked == true)
            {
                label_rapor.Text = "Dokuma";
            }
        }

        private void filtre_ek_servis_CheckedChanged(object sender, EventArgs e)
        {
            if (filtre_ek_servis.Checked == true)
            {
                label_rapor.Text = "Ek Servis";
            }       
        }

        private void filtre_gec_servis_CheckedChanged(object sender, EventArgs e)
        {
            if (filtre_gec_servis.Checked == true)
            {
                label_rapor.Text = "Geç Servis";
            }
        }   

        private void raporla_Click(object sender, EventArgs e)
        {
            if(dateTimePicker_baslangic.Value<=dateTimePicker_bitis.Value){
                //Rapor tab sayfasını aç
                xtraTabControl.SelectedTabPage = xtraTabPage_Rapor;
                //Departman filtreli kayıt listele
                if (label_rapor.Text == "Personel" || label_rapor.Text == "Dokuma" || label_rapor.Text == "İplik")
                {
                    if (filtre_pazar.Checked == true)
                    {
                        header.Text = label_rapor.Text.ToUpper() + " PAZAR";
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter("Select tarih as Tarih,saat as Saat, servis_plaka as Plaka,servis_sofor as 'Şoför',takip_eden as Görevli ,gec_geldi as 'Geç Geldi' from tbl_Servis_Puantaj where ek_servis = 'False' and gün = 'Pazar' and departman = '" + label_rapor.Text + "' and(tarih between '" + dateTimePicker_baslangic.Text + "' and '" + dateTimePicker_bitis.Text + "') order by tarih,saat", connection);
                        da.Fill(dt);
                        gridControl_Rapor.DataSource = dt;
                    }
                    if (filtre_pazar.Checked == false)
                    {
                        header.Text = label_rapor.Text.ToUpper();
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter("Select tarih as Tarih,saat as Saat,servis_plaka as Plaka,servis_sofor as 'Şoför',takip_eden as Görevli ,gec_geldi as 'Geç Geldi' from tbl_Servis_Puantaj where ek_servis='False' and  gün!='Pazar' and departman='" + label_rapor.Text + "' and (tarih between '" + dateTimePicker_baslangic.Text + "' and '" + dateTimePicker_bitis.Text + "') order by tarih,saat", connection);
                        da.Fill(dt);
                        gridControl_Rapor.DataSource = dt;
                    }
                    int count_departman = count_Departman(dateTimePicker_baslangic.Text, dateTimePicker_bitis.Text, label_rapor.Text, filtre_pazar.Checked);
                    int count_departman_gec_servis = count_Departman_Gec_Servis(dateTimePicker_baslangic.Text, dateTimePicker_bitis.Text, label_rapor.Text, filtre_pazar.Checked);
                    richTextBox1.Text = "Toplam " + count_departman + " kayıt bulundu. " + count_departman_gec_servis + " kayıt geç servis kaydıdır.";
                }
                //Ek Servis kayıt listele
                if (label_rapor.Text == "Ek Servis")
                {
                    if (filtre_pazar.Checked == true)
                    {
                        MessageBox.Show("Ek servis raporlaması için pazar günü filtrelemesi bulunmaz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        header.Text = label_rapor.Text.ToUpper();
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter("Select tarih as Tarih,saat as Saat,departman as Departman,servis_plaka as Plaka,servis_sofor as 'Şoför',takip_eden as Görevli ,gec_geldi as 'Geç Geldi' from tbl_Servis_Puantaj where ek_servis = 'True' and(tarih between '" + dateTimePicker_baslangic.Text + "' and '" + dateTimePicker_bitis.Text + "') order by tarih,saat", connection);
                        da.Fill(dt);
                        gridControl_Rapor.DataSource = dt;

                        int count_ek_servis = count_Ek_Servis(dateTimePicker_baslangic.Text, dateTimePicker_bitis.Text);
                        int count_ek_servis_gec = count_Ek_Servis_Gec(dateTimePicker_baslangic.Text, dateTimePicker_bitis.Text);
                        richTextBox1.Text = "Toplam " + count_ek_servis + " kayıt bulundu. " + count_ek_servis_gec + " kayıt geç servis kaydıdır.";
                    }
            }
                //Geç Servis kayıt listele
                if (label_rapor.Text == "Geç Servis")
                {
                    if (filtre_pazar.Checked == true)
                    {
                        MessageBox.Show("Geç servis raporlaması için pazar günü filtrelemesi bulunmaz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        header.Text = label_rapor.Text.ToUpper();
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter("Select tarih as Tarih,saat as Saat,departman as Departman,servis_plaka as Plaka, servis_sofor as 'Şoför',takip_eden as Görevli ,gec_geldi as 'Geç Geldi' ,servis_not as 'Not' from tbl_Servis_Puantaj where gec_geldi = 'True' and(tarih between '" + dateTimePicker_baslangic.Text + "' and '" + dateTimePicker_bitis.Text + "') order by tarih,saat", connection);
                        da.Fill(dt);
                        gridControl_Rapor.DataSource = dt;

                        int count_total_Gec_servis = count_Total_Gec_Servis(dateTimePicker_baslangic.Text, dateTimePicker_bitis.Text);
                        richTextBox1.Text = "Toplam " + count_total_Gec_servis + " kayıt bulundu. ";
                    }                  
                }
            }
            else
            {
                MessageBox.Show("Hatalı tarih girişi. Başlangıç tarihi bitiş tarihinden sonra olamaz", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        //Tamamı zamana göre filtrelenmiş
        //
        //Departmana göre kayıt sayısını hesapla
        //
        private int count_Departman(string baslangic_tarihi, string bitis_tarihi, string departman, bool isPazar)
        {
            connection.Open();
            SqlCommand count_Departman = new SqlCommand();
            int count = 0;
            if (isPazar == true)
            {
                count_Departman = new SqlCommand("select count(query.departman) from(Select departman from tbl_Servis_Puantaj where ek_servis = 'False' and  gün='Pazar' and departman =@p1 and (tarih between @p2 and @p3)) as query", connection);
            }
            if (isPazar == false)
            {
                count_Departman = new SqlCommand("select count(query.departman) from(Select departman from tbl_Servis_Puantaj where ek_servis = 'False' and  gün!='Pazar' and departman =@p1 and (tarih between @p2 and @p3)) as query", connection);
            }
            count_Departman.Parameters.AddWithValue("@p1", departman);
            count_Departman.Parameters.AddWithValue("@p2", baslangic_tarihi);
            count_Departman.Parameters.AddWithValue("@p3", bitis_tarihi);
            SqlDataReader count_Departman_reader = count_Departman.ExecuteReader();
            while (count_Departman_reader.Read())
            {
                count = Convert.ToInt32(count_Departman_reader[0]);
            }
            connection.Close();
            return count;
        }
        //
        //Departmana göre geç kayıt sayısını hesapla
        //
        private int count_Departman_Gec_Servis(string baslangic_tarihi, string bitis_tarihi, string departman, bool isPazar)
        {
            connection.Open();
            SqlCommand count_Departman_Gec_Servis = new SqlCommand();
            int count = 0;
            if (isPazar == true)
            {
                count_Departman_Gec_Servis = new SqlCommand("select count(query.gec_geldi) from(Select gec_geldi from tbl_Servis_Puantaj where gec_geldi='True' and ek_servis = 'False' and  gün='Pazar' and departman =@p1 and (tarih between @p2 and @p3)) as query", connection);
            }
            if (isPazar == false)
            {
                count_Departman_Gec_Servis = new SqlCommand("select count(query.gec_geldi) from(Select gec_geldi from tbl_Servis_Puantaj where gec_geldi='True' and ek_servis = 'False' and  gün!='Pazar' and departman =@p1 and (tarih between @p2 and @p3)) as query", connection);
            }
            count_Departman_Gec_Servis.Parameters.AddWithValue("@p1", departman);
            count_Departman_Gec_Servis.Parameters.AddWithValue("@p2", baslangic_tarihi);
            count_Departman_Gec_Servis.Parameters.AddWithValue("@p3", bitis_tarihi);
            SqlDataReader count_Departman_Gec_Servis_reader = count_Departman_Gec_Servis.ExecuteReader();
            while (count_Departman_Gec_Servis_reader.Read())
            {
                count = Convert.ToInt32(count_Departman_Gec_Servis_reader[0]);
            }
            connection.Close();
            return count;

        }
        //
        //Ek servis sayısını hesapla
        //
        private int count_Ek_Servis(String baslangic_tarihi, String bitis_tarihi)
        {
            connection.Open();
            int count = 0;
            SqlCommand count_Ek_Servis = new SqlCommand("select count(query.ek_servis) from(Select ek_servis from tbl_Servis_Puantaj where ek_servis = 'True' and (tarih between @p1 and @p2)) as query", connection);
            count_Ek_Servis.Parameters.AddWithValue("@p1", baslangic_tarihi);
            count_Ek_Servis.Parameters.AddWithValue("@p2", bitis_tarihi);
            SqlDataReader count_Ek_Servis_reader = count_Ek_Servis.ExecuteReader();
            while (count_Ek_Servis_reader.Read())
            {
                count = Convert.ToInt32(count_Ek_Servis_reader[0]);
            }
            connection.Close();
            return count;
        }
        //
        // Ek servis geç kayıt sayısını hesapla
        //
        private int count_Ek_Servis_Gec(String baslangic_tarihi, String bitis_tarihi)
        {
            connection.Open();
            int count = 0;
            SqlCommand count_Ek_Servis_Gec = new SqlCommand("select count(query.gec_geldi) from(Select gec_geldi from tbl_Servis_Puantaj where gec_geldi='True' and ek_servis = 'True' and (tarih between @p1 and @p2)) as query", connection);
            count_Ek_Servis_Gec.Parameters.AddWithValue("@p1", baslangic_tarihi);
            count_Ek_Servis_Gec.Parameters.AddWithValue("@p2", bitis_tarihi);
            SqlDataReader count_Ek_Servis_Gec_reader = count_Ek_Servis_Gec.ExecuteReader();
            while (count_Ek_Servis_Gec_reader.Read())
            {
                count = Convert.ToInt32(count_Ek_Servis_Gec_reader[0]);
            }
            connection.Close();
            return count;
        }
        //
        // Geç Kayıt Sayısını Hesapla
        // 
        private int count_Total_Gec_Servis(String baslangic_tarihi, String bitis_tarihi)
        {
            connection.Open();
            int count = 0;
            SqlCommand count_Total_Gec_Servis = new SqlCommand("select count(query.gec_geldi) from(Select gec_geldi from tbl_Servis_Puantaj where gec_geldi='True' and (tarih between @p1 and @p2)) as query", connection);
            count_Total_Gec_Servis.Parameters.AddWithValue("@p1", baslangic_tarihi);
            count_Total_Gec_Servis.Parameters.AddWithValue("@p2", bitis_tarihi);
            SqlDataReader count_Total_Gec_Servis_reader = count_Total_Gec_Servis.ExecuteReader();
            while (count_Total_Gec_Servis_reader.Read())
            {
                count = Convert.ToInt32(count_Total_Gec_Servis_reader[0]);
            }
            connection.Close();
            return count;
        }

        private void print_Click(object sender, EventArgs e)
        {
            gridControl_Rapor.Print();
        }
        int id;
        String departman;
        private void gridView_Servisler_FocusedRowChanged_1(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            checkBox_ek_servis.Checked = false;
            checkBox_gec_geldi.Checked = false;
            //araçları doldur
            id= Convert.ToInt32(gridView_Servisler.GetDataRow(gridView_Servisler.FocusedRowHandle)["Sıra"]);
            dateTimePicker_tarih.Text = gridView_Servisler.GetDataRow(gridView_Servisler.FocusedRowHandle)["Tarih"].ToString();
            combobox_sefer_durum.Text = gridView_Servisler.GetDataRow(gridView_Servisler.FocusedRowHandle)["Servis Durum"].ToString();
            comboBox_arac_plaka.Text = gridView_Servisler.GetDataRow(gridView_Servisler.FocusedRowHandle)["Plaka"].ToString();
            textBox_sofor_isim.Text = gridView_Servisler.GetDataRow(gridView_Servisler.FocusedRowHandle)["Şoför"].ToString();
            departman=gridView_Servisler.GetDataRow(gridView_Servisler.FocusedRowHandle)["Departman"].ToString();
            textBox_takip_eden.Text = gridView_Servisler.GetDataRow(gridView_Servisler.FocusedRowHandle)["Görevli"].ToString();
            switch (departman)
            {
                case "Personel":
                    radioButton_Personel.Checked = true;
                    break;
                case "Dokuma":
                    radioButton_Dokuma.Checked = true;
                    break;
                case "İplik":
                    radioButton_Iplik.Checked = true;
                    break;
                case "Desen":
                    radioButton_Desen.Checked = true;
                    break;
            }
        }

        private void simpleButton_güncelle_Click_1(object sender, EventArgs e)
        {
            if (combobox_sefer_durum.Text != "" && timeEdit1.Text != "" && lbl_departman_radio_button.Text != "" &&
               comboBox_arac_plaka.Text != "" && textBox_sofor_isim.Text != "" && textBox_takip_eden.Text != "")
            {
                try
                {
                    connection.Open();
                    SqlCommand kayit_güncelle_command = new SqlCommand("update tbl_Servis_Puantaj set servis_durum=@p1,saat=@p2,departman=@p3,servis_plaka=@p4,servis_sofor=@p5, takip_eden=@p6,gec_geldi=@p7,servis_not=@p8,ek_servis=@p9 where id=@p10", connection);
                    kayit_güncelle_command.Parameters.AddWithValue("@p1", combobox_sefer_durum.Text);
                    kayit_güncelle_command.Parameters.AddWithValue("@p2", timeEdit1.Text);
                    kayit_güncelle_command.Parameters.AddWithValue("@p3", lbl_departman_radio_button.Text);
                    kayit_güncelle_command.Parameters.AddWithValue("@p4", comboBox_arac_plaka.Text);
                    kayit_güncelle_command.Parameters.AddWithValue("@p5", textBox_sofor_isim.Text);
                    kayit_güncelle_command.Parameters.AddWithValue("@p6", textBox_takip_eden.Text);
                    kayit_güncelle_command.Parameters.AddWithValue("@p7", label_gec_geldi.Text);
                    kayit_güncelle_command.Parameters.AddWithValue("@p8", richTextBox_kayıt_not_.Text);
                    kayit_güncelle_command.Parameters.AddWithValue("@p9", label_ek_servis.Text);
                    kayit_güncelle_command.Parameters.AddWithValue("@p10", id);
                    kayit_güncelle_command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Kayıt Güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Lütfen hatalı bilgi girişi yapmadığınızdan emin olup tekrar kayıt güncellemeyi deneyiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Lütfen tüm bilgileri eksiksiz doldurduğunuzdan emin olup tekrar kayıt güncellemeyi deneyiniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ServisListele();
            GecKayıtListele();
            EkKayıtListele();
            //araçları temizle
            dateTimePicker_tarih.Value = DateTime.Now;
            richTextBox_kayıt_not_.ResetText();
            comboBox_arac_plaka.ResetText();
            textBox_sofor_isim.ResetText();
            checkBox_ek_servis.Checked = false;
            checkBox_gec_geldi.Checked = false;
        }

      
    }
}
