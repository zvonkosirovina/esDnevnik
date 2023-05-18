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
    public partial class Raspodela : Form
    {
        SqlConnection veza;
        DataTable dt_imePrezime, dtID, dtSmer, dtOdeljenje, dtPom;
        SqlCommand menjanja;

        public Raspodela()
        {
            InitializeComponent();
        }

        private void Raspodela_Load(object sender, EventArgs e)
        {
            ID_Populate();
            imePrezime_Populate();
            Odeljenje_Populate();
            Predmet_Populate();
            godina_Load();
            gridPopulate();
        }

        private void ID_Populate()
        {
            DataTable dtID = Konekcija.Unos("SELECT id FROM raspodela");
            cbID.DataSource = dtID;
            cbID.ValueMember = "id";
            cbID.DisplayMember = "ID";
        }

        private void Odeljenje_Populate()
        {
            DataTable dtOdeljenje = Konekcija.Unos("SELECT STR(Odeljenje.razred,1,0) + '/' + Odeljenje.indeks AS odeljenje FROM Odeljenje");
            cbOdeljenje.DataSource = dtOdeljenje;
            cbOdeljenje.ValueMember = "odeljenje";
            cbOdeljenje.DisplayMember = "Odeljenje";
        }

        private void btBrisi_Click(object sender, EventArgs e)
        {
            try
            {
                string naredba = "DELETE FROM Raspodela WHERE id = " + cbID.Text.ToString();
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

        private void btIzmeni_Click(object sender, EventArgs e)
        {
            menjanja = new SqlCommand();

            int indeks = int.Parse(cbID.Text);
            string[] ime_prezime = cbNastavnik.Text.Split(' ');
            string[] odeljenje = cbOdeljenje.Text.Split('/');
            int imePrezime_id, godina_id, predmet_id, Odeljenje_id;

            dtPom = new DataTable();
            dtPom = Konekcija.Unos("SELECT id FROM Osoba WHERE ime = " + "'" + ime_prezime[0] + "' AND prezime = " + "'" + ime_prezime[1] + "'");
            imePrezime_id = (int)dtPom.Rows[0][0];

            dtPom = new DataTable();
            dtPom = Konekcija.Unos("SELECT id FROM Skolska_godina WHERE naziv = " + "'" + cbGodina.Text.ToString() + "'");
            godina_id = (int)dtPom.Rows[0][0];

            dtPom = new DataTable();
            dtPom = Konekcija.Unos("SELECT id FROM Predmet WHERE naziv = " + "'" + cbPredmet.Text.ToString() + "'");
            predmet_id = (int)dtPom.Rows[0][0];

            dtPom = new DataTable();
            dtPom = Konekcija.Unos("SELECT id FROM Odeljenje WHERE razred = " + "'" + odeljenje[0] + "' AND indeks = " + "'" + odeljenje[1] + "'");
            Odeljenje_id = (int)dtPom.Rows[0][0];

            menjanja.CommandText = ("UPDATE Raspodela SET nastavnik_id = " + imePrezime_id + " WHERE id = " + indeks +
                " UPDATE Raspodela SET godina_id = " + godina_id + " WHERE id = " + indeks +
                " UPDATE Raspodela SET predmet_id = " + predmet_id + " WHERE id = " + indeks +
                " UPDATE Raspodela SET odeljenje_id = " + Odeljenje_id + " WHERE id = " + indeks);
        }

        private void btDodaj_Click(object sender, EventArgs e)
        {
            try
            {
                string[] ime_prezime = cbNastavnik.Text.Split(' ');
                string[] odeljenje = cbOdeljenje.Text.Split('/');
                int imePrezime_id, godina_id, predmet_id, Odeljenje_id;

                dtPom = new DataTable();
                dtPom = Konekcija.Unos("SELECT id FROM Osoba WHERE ime = " + "'" + ime_prezime[0] + "' AND prezime = " + "'" + ime_prezime[1] + "'");
                imePrezime_id = (int)dtPom.Rows[0][0];

                dtPom = new DataTable();
                dtPom = Konekcija.Unos("SELECT id FROM Skolska_godina WHERE naziv = " + "'" + cbGodina.Text.ToString() + "'");
                godina_id = (int)dtPom.Rows[0][0];

                dtPom = new DataTable();
                dtPom = Konekcija.Unos("SELECT id FROM Predmet WHERE naziv = " + "'" + cbPredmet.Text.ToString() + "'");
                predmet_id = (int)dtPom.Rows[0][0];

                dtPom = new DataTable();
                dtPom = Konekcija.Unos("SELECT id FROM Odeljenje WHERE razred = " + "'" + odeljenje[0] + "' AND indeks = " + "'" + odeljenje[1] + "'");
                Odeljenje_id = (int)dtPom.Rows[0][0];

                menjanja = new SqlCommand();
                menjanja.CommandText = ("INSERT INTO Raspodela VALUES (" + imePrezime_id + ", " + godina_id + ", " + predmet_id + ", " + Odeljenje_id + ")");

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

        private void Predmet_Populate()
        {
            DataTable dtSmer = Konekcija.Unos("SELECT naziv FROM predmet");
            cbPredmet.DataSource = dtSmer;
            cbPredmet.ValueMember = "naziv";
            cbPredmet.DisplayMember = "Naziv";
        }

        private void imePrezime_Populate()
        {
            DataTable dt_imePrezime = Konekcija.Unos("SELECT ime + ' ' + prezime AS Nastavnik FROM Osoba WHERE uloga = 2");
            cbNastavnik.DataSource = dt_imePrezime;
            cbNastavnik.ValueMember = "Nastavnik";
            cbNastavnik.DisplayMember = "Nastavnik";
        }

        private void godina_Load()
        {
            DataTable dtSmer = Konekcija.Unos("SELECT naziv FROM Skolska_godina");
            cbGodina.DataSource = dtSmer;
            cbGodina.ValueMember = "naziv";
            cbGodina.DisplayMember = "Naziv";
        }

        private void gridPopulate()
        {
            DataTable dtPom = Konekcija.Unos("SELECT raspodela.id, Osoba.ime + ' ' + Osoba.prezime AS Nastavnik, Skolska_godina.naziv AS Godina, Predmet.naziv AS Predmet, STR(Odeljenje.razred,1,0) + '/' + Odeljenje.indeks AS Odeljenje FROM Raspodela left join Osoba ON Raspodela.nastavnik_id = Osoba.id left join Skolska_godina ON Raspodela.godina_id = Skolska_godina.id left join Predmet ON Raspodela.predmet_id = Predmet.id left join Odeljenje ON Raspodela.odeljenje_id = Odeljenje.id");
            ID_Populate();
            imePrezime_Populate();

            dataGridView1.DataSource = dtPom;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
        }
    }
}
