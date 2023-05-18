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
    public partial class Upisnica : Form
    {
        SqlConnection veza;
        DataTable dt_imePrezime, dtID, dtSmer, dtOdeljenje, dtPom;
        SqlCommand menjanja;
        public Upisnica()
        {
            InitializeComponent();
        }

        private void Upisnica_Load(object sender, EventArgs e)
        {
            Konekcija.Connect();
            ID_Populate();
            imePrezime_Populate();
            Smer_Populate();
            Odeljenje_Populate();
            gridPopulate();
        }
        private void ID_Populate()
        {
            DataTable dtID = Konekcija.Unos("SELECT id FROM Osoba");
            cbID.DataSource = dtID;
            cbID.ValueMember = "id";
            cbID.DisplayMember = "ID";
        }

        private void btDodaj_Click(object sender, EventArgs e)
        {
            try
            {
                string[] ime_prezime = cbImePrezime.Text.Split(' ');
                string[] odeljenje = cbOdeljenje.Text.Split('/');
                int imePrezime_id, godina_id, predmet_id, Odeljenje_id;
                menjanja = new SqlCommand();

                dtPom = new DataTable();
                dtPom = Konekcija.Unos("SELECT id FROM Osoba WHERE ime = " + "'" + ime_prezime[0] + "' AND prezime = " + "'" + ime_prezime[1] + "'");
                int osoba_id = (int)dtPom.Rows[0][0];

                dtPom = new DataTable();
                dtPom = Konekcija.Unos("SELECT id FROM Odeljenje WHERE razred = " + "'" + odeljenje[0] + "' AND indeks = " + "'" + odeljenje[1] + "'");
                int odeljenje_id = (int)dtPom.Rows[0][0];

                dtPom = new DataTable();
                dtPom = Konekcija.Unos("SELECT * FROM Upisnica WHERE osoba_id = " + osoba_id + " AND odeljenje_id = " + odeljenje_id);
      

                menjanja.CommandText = ("INSERT INTO Upisnica VALUES (" + osoba_id + ", " + odeljenje_id + ")");

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

        private void btIzmeni_Click(object sender, EventArgs e)
        {
            menjanja = new SqlCommand();
            int indeks = int.Parse(cbID.Text);
            string[] ime_prezime = cbImePrezime.Text.Split(' ');
            string[] odeljenje = cbOdeljenje.Text.Split('/');
            int imePrezime_id, godina_id, predmet_id, Odeljenje_id;

            dtPom = new DataTable();
            dtPom = Konekcija.Unos("SELECT id FROM Osoba WHERE ime = " + "'" + ime_prezime[0] + "' AND prezime = " + "'" + ime_prezime[1] + "'");
            int osoba_id = (int)dtPom.Rows[0][0];

            dtPom = new DataTable();
            dtPom = Konekcija.Unos("SELECT id FROM Odeljenje WHERE razred = " + "'" + odeljenje[0] + "' AND indeks = " + "'" + odeljenje[1] + "'");
            int odeljenje_id = (int)dtPom.Rows[0][0];

            dtPom = new DataTable();
            dtPom = Konekcija.Unos("SELECT * FROM Upisnica WHERE osoba_id = " + osoba_id + " AND odeljenje_id = " + odeljenje_id);

            menjanja.CommandText = ("UPDATE Upisnica SET osoba_id = " + osoba_id + " WHERE id = " + indeks +
                " UPDATE Upisnica SET odeljenje_id = " + odeljenje_id + " WHERE id = " + indeks);

            SqlConnection con = new SqlConnection(Konekcija.Veza());
            con.Open();
            menjanja.Connection = con;
            menjanja.ExecuteNonQuery();
            con.Close();
            gridPopulate();
        }

        private void btObrisi_Click(object sender, EventArgs e)
        {
            try
            {
                string naredba = "DELETE FROM Upisnica WHERE id = " + cbID.Text.ToString();
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

        private void Odeljenje_Populate()
        {
            DataTable dtOdeljenje = Konekcija.Unos("SELECT STR(Odeljenje.razred,1,0) + '/' + Odeljenje.indeks AS odeljenje FROM Odeljenje");
            cbOdeljenje.DataSource = dtOdeljenje;
            cbOdeljenje.ValueMember = "odeljenje";
            cbOdeljenje.DisplayMember = "Odeljenje";
        }

        private void Smer_Populate()
        {
            DataTable dtSmer = Konekcija.Unos("SELECT naziv FROM Smer");
            cbSmer.DataSource = dtSmer;
            cbSmer.ValueMember = "naziv";
            cbSmer.DisplayMember = "Naziv";
        }

        private void imePrezime_Populate()
        {
            DataTable dt_imePrezime = Konekcija.Unos("SELECT ime + ' ' + prezime AS 'Ime i Prezime' FROM Osoba");
            cbImePrezime.DataSource = dt_imePrezime;
            cbImePrezime.ValueMember = "Ime i Prezime";
            cbImePrezime.DisplayMember = "Ime i Prezime";
        }

        private void gridPopulate()
        {
            DataTable dtPom = Konekcija.Unos("SELECT Upisnica.id AS id, Osoba.ime + ' ' + Osoba.prezime AS 'ime i prezime', STR(Odeljenje.razred,1,0) + '/' + Odeljenje.indeks AS Odeljenje, Smer.naziv AS Smer FROM Upisnica\r\nJOIN Osoba ON Upisnica.osoba_id = Osoba.id\r\nJOIN Odeljenje ON Upisnica.odeljenje_id = Odeljenje.id\r\nJOIN Smer ON Odeljenje.smer_id = Smer.id");
            ID_Populate();
            imePrezime_Populate();

            dataGridView1.DataSource = dtPom;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
        }
    }
}
