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
    public partial class skolskaGodina : Form
    {
        SqlConnection veza;
        DataTable dt_godina, dtID, dtPom;
        SqlCommand menjanja;
        
        int broj;
        public skolskaGodina()
        {
            InitializeComponent();
        }

        private void skolskaGodina_Load(object sender, EventArgs e)
        {
            Konekcija.Connect();
            ID_Populate();
            skolskaGodina_Populate();
            gridPopulate();
        }

        private void ID_Populate()
        {
            DataTable dtID = Konekcija.Unos("SELECT id FROM Skolska_godina");
            cbID.DataSource = dtID;
            cbID.ValueMember = "id";
            cbID.DisplayMember = "ID";
        }

        private void skolskaGodina_Populate()
        {
            DataTable dt_godina = Konekcija.Unos("SELECT naziv FROM Skolska_godina");
            cbSkolskaGodina.DataSource = dt_godina;
            cbSkolskaGodina.ValueMember = "naziv";
            cbSkolskaGodina.DisplayMember = "naziv";
        }

        private void gridPopulate()
        {
            DataTable dtPom = Konekcija.Unos("SELECT * FROM Skolska_godina");
            ID_Populate();
            skolskaGodina_Populate();

            dataGridView1.DataSource = dtPom;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
        }

        private void btnIzmeni_Click(object sender, EventArgs e)
        {
            try
            {
                string naredba = "UPDATE Skolska_godina SET naziv='" + cbSkolskaGodina.Text.ToString() + "' where id = " + cbID.Text.ToString();
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

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            try
            {
                string naziv;
                naziv = cbSkolskaGodina.Text.ToString();

                menjanja = new SqlCommand();
                menjanja.CommandText = ("INSERT INTO Skolska_godina VALUES ('" + naziv + "')");

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

        private void dataGridView1_CurrentCellChanged_1(object sender, EventArgs e)
        {
            int indeks = dataGridView1.CurrentRow.Index;
            cbID.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["Id"].Value);
            cbSkolskaGodina.Text = Convert.ToString(dataGridView1.Rows[indeks].Cells["naziv"].Value);
          
        }

        private void btnIzbrisi_Click(object sender, EventArgs e)
        {
            try
            {
                string naredba = "DELETE FROM Skolska_godina WHERE id = " + cbID.Text.ToString();
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

      /*  private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                broj = dataGridView1.CurrentRow.Index;
                cbID.SelectedValue = dtID.Rows[broj]["id"].ToString();
                cbSkolskaGodina.SelectedValue = dt_godina.Rows[broj]["naziv"].ToString();
            }
        }*/

    }
}
