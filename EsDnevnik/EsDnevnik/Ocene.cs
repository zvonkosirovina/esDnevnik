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
    public partial class Ocene : Form
    {
        public Ocene()
        {
            InitializeComponent();
        }

        SqlConnection veza;
        DataTable dt_imePrezime, dtID, dtSmer, dtOdeljenje, dtPom;
        SqlCommand menjanja;

        private void Ocene_Load(object sender, EventArgs e)
        {
            ID_Populate();
            Datum_Populate();
            Predmet_Populate();
            imePrezime_Populate();
            ocena_Load();
            gridPopulate();
        }

        private void ID_Populate()
        {
            DataTable dtID = Konekcija.Unos("SELECT id FROM ocena");
            cbID.DataSource = dtID;
            cbID.ValueMember = "id";
            cbID.DisplayMember = "ID";
        }

        private void Datum_Populate()
        {
            DataTable dtOdeljenje = Konekcija.Unos("SELECT datum FROM ocena");
            cbDatum.DataSource = dtOdeljenje;
            cbDatum.ValueMember = "datum";
            cbDatum.DisplayMember = "Datum";
        }

        private void Predmet_Populate()
        {
            DataTable dtSmer = Konekcija.Unos("SELECT naziv FROM predmet");
            cbPredmet.DataSource = dtSmer;
            cbPredmet.ValueMember = "naziv";
            cbPredmet.DisplayMember = "Naziv";
        }

        private void imePrezime_Populate()
        {
            DataTable dt_imePrezime = Konekcija.Unos("SELECT ime + ' ' + prezime AS 'Ime i Prezime' FROM Osoba");
            cbUcenik.DataSource = dt_imePrezime;
            cbUcenik.ValueMember = "Ime i Prezime";
            cbUcenik.DisplayMember = "Ime i Prezime";
        }

        private void ocena_Load()
        {
            DataTable dtSmer = Konekcija.Unos("SELECT ocena FROM Ocena");
            cbOcena.DataSource = dtSmer;
            cbOcena.ValueMember = "ocena";
            cbOcena.DisplayMember = "Ocena";
        }

        private void gridPopulate()
        {
            DataTable dtPom = Konekcija.Unos("SELECT Ocena.id AS Id, datum AS Datum, Osoba.ime + ' ' + Osoba.prezime AS Ucenik, ocena AS Ocena, Predmet.naziv AS Predmet FROM Ocena\r\n JOIN Osoba ON Ocena.ucenik_id = Osoba.id\r\n JOIN Raspodela ON Raspodela.id = Ocena.raspodela_id\r\n JOIN Predmet ON Predmet.id = Raspodela.predmet_id");
            ID_Populate();
            imePrezime_Populate();

            dataGridView1.DataSource = dtPom;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
        }

        private void btDodaj_Click(object sender, EventArgs e)
        {
            menjanja = new SqlCommand();

            int indeks = int.Parse(cbID.Text);
            string[] ime_prezime = cbUcenik.Text.Split(' ');
            string datum = cbDatum.Text.ToString();
            string ocena = cbOcena.Text.ToString();
            string predmet = cbPredmet.Text.ToString();


            dtPom = new DataTable();
            dtPom = Konekcija.Unos("SELECT id FROM Osoba WHERE ime = " + "'" + ime_prezime[0] + "' AND prezime = " + "'" + ime_prezime[1] + "'");
            int osoba_id = (int)dtPom.Rows[0][0];

            dtPom = new DataTable();
            dtPom = Konekcija.Unos("SELECT id FROM Predmet WHERE naziv = " + "'" + predmet + "'");
            int predmet_id = (int)dtPom.Rows[0][0];

            dtPom = new DataTable();
            dtPom = Konekcija.Unos("SELECT id FROM Raspodela WHERE predmet_id = " + predmet_id);
            int raspodela_id = (int)dtPom.Rows[0][0];

            dtPom = new DataTable();
            dtPom = Konekcija.Unos("SELECT * FROM Ocena WHERE datum = '" + datum + "' AND raspodela_id = " + raspodela_id + " AND ocena = '" + ocena + "' AND ucenik_id = " + osoba_id);


            menjanja.CommandText = ("INSERT INTO Ocena VALUES ('" + datum + "', " + raspodela_id + ", " + ocena + ", " + osoba_id + ")");

            SqlConnection con = new SqlConnection(Konekcija.Veza());
            con.Open();
            menjanja.Connection = con;
            menjanja.ExecuteNonQuery();
            con.Close();
            gridPopulate();
        }

        private void btIzmeni_Click(object sender, EventArgs e)
        {
            menjanja = new SqlCommand();

            int indeks = int.Parse(cbID.Text);
            string[] ime_prezime = cbUcenik.Text.Split(' ');
            string datum = cbDatum.Text.ToString();
            string ocena = cbOcena.Text.ToString();
            string predmet = cbPredmet.Text.ToString();

            dtPom = new DataTable();
            dtPom = Konekcija.Unos("SELECT id FROM Osoba WHERE ime = " + "'" + ime_prezime[0] + "' AND prezime = " + "'" + ime_prezime[1] + "'");
            int osoba_id = (int)dtPom.Rows[0][0];

            dtPom = new DataTable();
            dtPom = Konekcija.Unos("SELECT id FROM Predmet WHERE naziv = " + "'" + predmet + "'");
            int predmet_id = (int)dtPom.Rows[0][0];

            dtPom = new DataTable();
            dtPom = Konekcija.Unos("SELECT id FROM Raspodela WHERE predmet_id = " + predmet_id);
            int raspodela_id = (int)dtPom.Rows[0][0];

            dtPom = new DataTable();
            dtPom = Konekcija.Unos("SELECT * FROM Ocena WHERE datum = '" + datum + "' AND raspodela_id = " + raspodela_id + " AND ocena = '" + ocena + "' AND ucenik_id = " + osoba_id);

            menjanja.CommandText = ("UPDATE Ocena SET datum = '" + datum + "' WHERE id = " + indeks +
                " UPDATE Ocena SET raspodela_id = " + raspodela_id + " WHERE id = " + indeks +
                " UPDATE Ocena SET ocena = '" + ocena + "' WHERE id = " + indeks +
                " UPDATE Ocena SET ucenik_id = " + osoba_id + " WHERE id = " + indeks);

            SqlConnection con = new SqlConnection(Konekcija.Veza());
            con.Open();
            menjanja.Connection = con;
            menjanja.ExecuteNonQuery();
            con.Close();
            gridPopulate();
        }

        private void btBrisi_Click(object sender, EventArgs e)
        {
            try
            {
                string naredba = "DELETE FROM Ocena WHERE id = " + cbID.Text.ToString();
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
    }
}
