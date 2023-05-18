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
    public partial class Predmet : Form
    {
        SqlConnection veza;
        DataTable dtPredmet, dtID, dtPom, dtRazred;
        int broj;
        SqlCommand menjanja;

        public Predmet()
        {
            InitializeComponent();
        }

        private void Predmet_Load(object sender, EventArgs e)
        {
            Konekcija.Connect();
            ID_Populate();
            SmerPopulate();
            gridPopulate();
        }

        private void ID_Populate()
        {
            DataTable dtID = Konekcija.Unos("SELECT id FROM Predmet");
            cbID.DataSource = dtID;
            cbID.ValueMember = "id";
            cbID.DisplayMember = "ID";
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            try
            {
                string naziv, naziv1;
                naziv = cbPredmet.Text.ToString();
                naziv1 = cbRazred.Text.ToString();

                menjanja = new SqlCommand();
                menjanja.CommandText = ("INSERT INTO predmet VALUES ('" + naziv + "', '" + naziv1 + "')");

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
                string naredba = "UPDATE Predmet SET naziv='" + cbPredmet.Text.ToString() + "' where id = " + cbID.Text.ToString() +
                    "UPDATE Predmet SET razred ='" + cbRazred.Text.ToString() + "' where id = " + cbID.Text.ToString();
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

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int indeks = dataGridView1.CurrentRow.Index;
                cbID.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Id"].Value);
                cbPredmet.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["naziv"].Value);
                cbRazred.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["razred"].Value);
            }
        }

        private void btnBrisi_Click(object sender, EventArgs e)
        {
            try
            {
                string naredba = "DELETE FROM Predmet WHERE id = " + cbID.Text.ToString();
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

       /* private void dataGridView1_CurrentCellChanged(object sender, EventArgs e) //zamisli da radi kako treba
        {
            if (dataGridView1.CurrentRow != null)
            {
                broj = dataGridView1.CurrentRow.Index;
                cbID.SelectedValue = dtID.Rows[broj]["id"].ToString();
                cbPredmet.SelectedValue = dtPredmet.Rows[broj]["naziv"].ToString();
                cbOcena.SelectedValue = dtRazred.Rows[broj]["razred"].ToString();
            }
        }*/

        private void SmerPopulate()
        {
            DataTable dtPredmet = Konekcija.Unos("SELECT naziv FROM Predmet");
            cbPredmet.DataSource = dtPredmet;
            cbPredmet.ValueMember = "naziv";
            cbPredmet.DisplayMember = "naziv";
        }

        private void gridPopulate()
        {
            DataTable dtPom = Konekcija.Unos("SELECT * FROM Predmet");

            dataGridView1.DataSource = dtPom;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
        }
    }
}
