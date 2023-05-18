using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EsDnevnik
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void odeljenjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Odeljenje odeljenje = new Odeljenje();
            odeljenje.ShowDialog();
        }

        private void osobaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Osoba osoba = new Osoba();
            osoba.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void skolskaGodinaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            skolskaGodina skolskaGodina = new skolskaGodina();
            skolskaGodina.ShowDialog();
        }

        private void smerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Smer smer = new Smer();
            smer.ShowDialog();
        }

        private void predmetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Predmet predmet = new Predmet();
            predmet.ShowDialog();
        }

        private void upisnicaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Upisnica upisnica = new Upisnica();
            upisnica.ShowDialog();
        }

        private void raspodelaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Raspodela raspodela = new Raspodela();
            raspodela.ShowDialog();
        }

        private void oceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Ocene ocene = new Ocene();
            ocene.ShowDialog();
        }
    }
}
