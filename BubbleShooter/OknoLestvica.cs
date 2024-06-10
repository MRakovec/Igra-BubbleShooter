using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BubbleShooter
{
    public partial class OknoLestvica : Form
    {
        private int steviloTock = OknoIgra.steviloTock;
        private string vzdevek = OknoVpis.Vzdevek;

        // napisi
        private Label napisNaslov;
        private DataGridView dataGridViewLestvica;

        private string fontStil = OknoIgra.fontStil;
        private int naslovFontVelikost;
        private int fontVelikost;

        // datoteka z lestvico
        string pot = @"lestvica.txt";
        List<(string, int)> seznamLestvica;
        DataTable tabela;

        public OknoLestvica()
        {
            InitializeComponent();

            // velikost pisave
            fontVelikost = (int)(this.ClientSize.Height / 20);
            naslovFontVelikost = 2 * fontVelikost;

            // napis naslov
            napisNaslov = new Label();
            napisNaslov.AutoSize = true;
            napisNaslov.Font = new Font(fontStil, naslovFontVelikost, FontStyle.Bold);
            napisNaslov.Text = "LESTVICA";

            // prikaz lestvice
            dataGridViewLestvica = new DataGridView();
            dataGridViewLestvica.Font = new Font(fontStil, fontVelikost);
            dataGridViewLestvica.ReadOnly = true;
            dataGridViewLestvica.AllowUserToResizeColumns = false;
            dataGridViewLestvica.AllowUserToResizeRows = false;
            dataGridViewLestvica.ColumnHeadersVisible = false;
            dataGridViewLestvica.AllowUserToOrderColumns = false;
            dataGridViewLestvica.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewLestvica.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewLestvica.AllowUserToAddRows = false;
            dataGridViewLestvica.RowHeadersVisible = false;
            dataGridViewLestvica.ColumnHeadersDefaultCellStyle.Font = new Font(fontStil, fontVelikost, FontStyle.Bold);
            // vsebina lestvice
            tabela = new DataTable();
            tabela.Columns.Add("Ime", typeof(string));
            tabela.Columns.Add("Tocke", typeof(int));

            this.Controls.Add(napisNaslov);
            this.Controls.Add(dataGridViewLestvica);

            dodajRezultatVLestvico();
        }

        private void OknoLestvica_FormClosing(object sender, FormClosingEventArgs e)
        {
            // če zapremo trenutno okno, naj se zaprejo vsa okna in program ustavi
            Application.Exit();
        }

        private void OknoLestvica_Load(object sender, EventArgs e)
        {
            this.Height = (int)(0.4 * Screen.PrimaryScreen.Bounds.Height);
            OknoLestvica_ResizeEnd(sender, e);

            // postavimo na sredino ekrana
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);
        }

        private void OknoLestvica_ResizeEnd(object sender, EventArgs e)
        {
            this.Width = this.Height;

            // velikost pisave
            fontVelikost = (int)(this.ClientSize.Height / 20);
            naslovFontVelikost = 2 * fontVelikost;

            // napis in lestvica
            napisNaslov.Location = new Point(this.ClientSize.Width / 2 - napisNaslov.Size.Width / 2, this.ClientSize.Height / 6 - napisNaslov.Size.Height / 2);
            dataGridViewLestvica.Location = new Point(this.ClientSize.Width / 2 - dataGridViewLestvica.Size.Width / 2, napisNaslov.Location.Y + (int)(napisNaslov.Height * 1.5));

            napisNaslov.Font = new Font(fontStil, naslovFontVelikost, FontStyle.Bold);
            dataGridViewLestvica.Font = new Font(fontStil, fontVelikost);
            dataGridViewLestvica.Size = new Size((int)(this.ClientSize.Width * 0.6), (int)(this.ClientSize.Height * 0.9 - dataGridViewLestvica.Location.Y));
            dataGridViewLestvica.AutoResizeColumnHeadersHeight();
        }

        /// <summary>
        /// V datoteko na primerno mesto zapiše rezultat in celotno lestvico prebere v seznam, ki ga doda dataGridView za prikaz.
        /// Rezultati so zapisani v tabeli, oblike "ime;tocke".
        /// </summary>
        private void dodajRezultatVLestvico()
        {            
            if (!File.Exists(pot)) File.CreateText(pot);

            seznamLestvica = new List<(string, int)>();
            string[] trenutniPodatki;
            string trenutniVzdevek; 
            int trenutniSteviloTock;
            bool jeBilDodan = false;

            // preberemo in vmestimo rezultat
            using (StreamReader sr = new StreamReader(pot))
            {
                string vrstica = "";
                while ((vrstica = sr.ReadLine()) != null)
                {
                    trenutniPodatki = vrstica.Split(';');
                    trenutniVzdevek = trenutniPodatki[0];
                    trenutniSteviloTock = int.Parse(trenutniPodatki[1]);

                    if (trenutniVzdevek == vzdevek && steviloTock <= trenutniSteviloTock)
                    {
                        seznamLestvica.Add((trenutniVzdevek, trenutniSteviloTock));
                        jeBilDodan = true;
                        break;
                    }

                    if (trenutniSteviloTock < steviloTock)
                    {
                        seznamLestvica.Add((vzdevek, steviloTock));
                        
                        if (trenutniVzdevek != vzdevek)
                        {
                            seznamLestvica.Add((trenutniVzdevek, trenutniSteviloTock));
                        }

                        jeBilDodan = true;
                        break;
                    }
                    else
                    {
                        seznamLestvica.Add((trenutniVzdevek, trenutniSteviloTock));
                    }
                }
                // prepišemo ostale
                while ((vrstica = sr.ReadLine()) != null)
                {
                    trenutniPodatki = vrstica.Split(';');
                    trenutniVzdevek = trenutniPodatki[0];
                    trenutniSteviloTock = int.Parse(trenutniPodatki[1]);

                    if (trenutniVzdevek != vzdevek)
                    {
                        seznamLestvica.Add((trenutniVzdevek, trenutniSteviloTock));
                    }
                }
            }

            // če je nov vzdevek in je zadnji na lestvici
            if (!jeBilDodan && 0 < steviloTock)
            {
                seznamLestvica.Add((vzdevek, steviloTock));
            }

            // napolnimo datoteko z novo lestvico
            using (StreamWriter sw = new StreamWriter(pot))
            {
                foreach ((string ime, int stevilo) in seznamLestvica){
                    sw.WriteLine($"{ime};{stevilo}");
                    // napolnimo se DataTable za prikaz v oknu
                    tabela.Rows.Add(ime, stevilo);
                }
            }

            dataGridViewLestvica.DataSource = tabela;
        }
    }
}
