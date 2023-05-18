using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsDnevnik
{
    public partial class Smer : Form
    {
        SqlConnection veza;
        DataTable dtSmer, dtID, dtPom;
        SqlCommand menjanja;
        int broj;
        public Smer()
        {
            InitializeComponent();
        }

        private void Smer_Load(object sender, EventArgs e)
        {
            Konekcija.Connect();
            ID_Populate();
            Smer_Populate();
            gridPopulate();
        }

        private void ID_Populate()
        {
            DataTable dtID = Konekcija.Unos("SELECT id FROM Smer");
            cbID.DataSource = dtID;
            cbID.ValueMember = "id";
            cbID.DisplayMember = "ID";
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            try
            {
                string naziv;
                naziv = cbSmer.Text.ToString();

                menjanja = new SqlCommand();
                menjanja.CommandText = ("INSERT INTO Smer VALUES ('" + naziv + "')");

                SqlConnection con = new SqlConnection(Konekcija.Veza());
                con.Open();
                menjanja.Connection = con;
                menjanja.ExecuteNonQuery();
                con.Close();
                gridPopulate();
            }
            catch (Exception Greska)
            {
                MessageBox.Show(Greska.Message);

            }
        }

        private void btnIzmeni_Click(object sender, EventArgs e)
        {
            try
            {
                string naredba = "UPDATE Smer SET naziv='" + cbSmer.Text.ToString() + "' where id = " + cbID.Text.ToString();
                menjanja = new SqlCommand();
                menjanja.CommandText = naredba;

                SqlConnection con = new SqlConnection(Konekcija.Veza());
                con.Open();
                menjanja.Connection = con;
                menjanja.ExecuteNonQuery();
                con.Close();
                gridPopulate();
            }
            catch (Exception Greska)
            {
                veza.Close();
                MessageBox.Show(Greska.Message);

            }
        }

        private void btnBrisi_Click(object sender, EventArgs e)
        {
            try
            {
                string naredba = "DELETE FROM Smer WHERE id = " + cbID.Text.ToString();
                menjanja = new SqlCommand();
                menjanja.CommandText = naredba;

                SqlConnection con = new SqlConnection(Konekcija.Veza());
                con.Open();
                menjanja.Connection = con;
                menjanja.ExecuteNonQuery();
                con.Close();
                gridPopulate();
            }
            catch (Exception Greska)
            {
                MessageBox.Show(Greska.Message);

            }
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            int indeks = dataGridView1.CurrentRow.Index;
            cbID.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Id"].Value);
            cbSmer.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["naziv"].Value);
        }

        /* private void dataGridView1_CurrentCellChanged(object sender, EventArgs e) //Samo ne radi bez ikakvog dobrog razloga
         {
             if (dataGridView1.CurrentRow != null)
             {
                 broj = dataGridView1.CurrentRow.Index;
                 cbID.SelectedValue = dtID.Rows[broj]["id"].ToString();
                 cbSmer.SelectedValue = dtID.Rows[broj]["naziv"].ToString();
             }
         }*/

        private void Smer_Populate()
        {
            DataTable dtSmer = Konekcija.Unos("SELECT naziv FROM Smer");
            cbSmer.DataSource = dtSmer;
            cbSmer.ValueMember = "naziv";
            cbSmer.DisplayMember = "NAZIV";
        }

        private void gridPopulate()
        {
            DataTable dtPom = Konekcija.Unos("SELECT * FROM Smer");

            dataGridView1.DataSource = dtPom;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
        }
    }
}
