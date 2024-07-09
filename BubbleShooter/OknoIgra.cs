using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BubbleShooter
{
    public partial class OknoIgra : Form
    {
        // Konstante
        private static int stVrstic = 20;
        private static int stStolpcev = 15;
        private static int stVrsticZacetek = 10;
        private Mehurcek[,] mrezaMehurckov;
        private Random random = new Random();

        // Animacija Brisanja in Padanja Mehurčkov
        private Timer timerAnimacijeBrisanja;
        private int stevecSlik = 0;
        private int maxSteviloSlik = 10;
        private int hitrostAnimacijeBrisanja = 40;
        private List<Mehurcek> SosediIsteBarve;

        private (float, float, Color)[] NepripetiMehurckiXYBarva;
        private Timer timerAnimacijePadanja;
        private int hitrostPadanja;
        private int hitrostAnimacijePadanja = 15;
        private int stevecNepripetih;

        // Nov mehurcek za streljanje
        private Color streljanMehurcekBarva;
        private float streljanMehurcekSredisceXZacetek;
        private float streljanMehurcekSredisceYZacetek;
        private float streljanMehurcekSredisceX;
        private float streljanMehurcekSredisceY;
        private Color naslednjiMehurcekBarva;
        private float naslednjiMehurcekFaktorVelikosti = 0.75f;
        private float naslednjiMehurcekX;
        private float naslednjiMehurcekY;
        private Timer timerStrela;
        private int hitrostStrela;
        private float smerStrelaX;
        private float smerStrelaY;
        private int hitrostAnimacijeStrela = 15;

        private float polozajMiskeX;
        private float polozajMiskeY;

        private float dolzinaPuscice;
        private float velikostPuscice;

        // Gumbi in napisi
        private Button gumbZamenjaj;
        private Label napisNaslednjiMehurcek;
        private Label napisSteviloTock;
        private int fontVelikost;

        public static int steviloTock = 0;
        public static string fontStil = "Arial";

        // dodajanje vrstic
        private int stevecStrelov = 0;
        private int maxSteviloStrelov = 10;
        Timer timerCakaj;
        Timer timerDodatneVrstice;
        private int hitrostDodajanjaVrstic = 20000;

        public OknoIgra()
        {
            InitializeComponent();

            mrezaMehurckov = new Mehurcek[stVrstic, stStolpcev];

            // risanje mehurčkov
            NapolniMrezoMehurckov();
            this.DoubleBuffered = true;
            this.Paint += IgraForm_Paint;

            // animacija brisanja in padanja mehurčkov
            timerAnimacijeBrisanja = new Timer();
            timerAnimacijeBrisanja.Interval = hitrostAnimacijeBrisanja;
            timerAnimacijeBrisanja.Tick += TimerAnimacijeBrisanja_Tick;

            timerAnimacijePadanja = new Timer();
            timerAnimacijePadanja.Interval = hitrostAnimacijePadanja;
            timerAnimacijePadanja.Tick += timerAnimacijePadanja_Tick;
            NepripetiMehurckiXYBarva = new (float, float, Color)[0];

            // nov mehurcek za streljanje
            streljanMehurcekSredisceXZacetek = (float)(this.ClientSize.Width / 2);
            streljanMehurcekSredisceYZacetek = (float)(this.ClientSize.Height - Mehurcek.velikostMehurcka);
            naslednjiMehurcekBarva = Mehurcek.mozneBarve[random.Next(Mehurcek.mozneBarve.Length)];
            ustvariNovStrelniMehurcek();

            timerStrela = new Timer();
            timerStrela.Interval = hitrostAnimacijeStrela;
            timerStrela.Tick += timerStrela_Tick;

            // dodajanje vrstic
            timerCakaj = new Timer();
            timerCakaj.Interval = 100;
            timerCakaj.Tick += timerCakaj_Tick;

            timerDodatneVrstice = new Timer();
            timerDodatneVrstice.Interval = hitrostDodajanjaVrstic;
            timerDodatneVrstice.Tick += timerDodatneVrstice_Tick;
            timerDodatneVrstice.Start();

            // gumbi in napisi
            fontVelikost = (int)(Mehurcek.velikostMehurcka / 3);

            gumbZamenjaj = new Button();
            gumbZamenjaj.AutoSize = true;
            gumbZamenjaj.Font = new Font(fontStil, fontVelikost);
            gumbZamenjaj.Text = "Zamenjaj";
            gumbZamenjaj.Click += gumbZamenjaj_Click;

            napisNaslednjiMehurcek = new Label();
            napisNaslednjiMehurcek.AutoSize = true;
            napisNaslednjiMehurcek.Font = new Font(fontStil, fontVelikost);
            napisNaslednjiMehurcek.Text = "Naslednji: ";

            napisSteviloTock = new Label();
            napisSteviloTock.AutoSize = true;
            napisSteviloTock.Font = new Font(fontStil, fontVelikost);
            napisSteviloTock.Text = $"Število točk: {steviloTock}";

            this.Controls.Add(gumbZamenjaj);
            this.Controls.Add(napisNaslednjiMehurcek);
            this.Controls.Add(napisSteviloTock);
        }

        private void NapolniMrezoMehurckov()
        {
            // napolnimo zacetne mehurcke
            for (int i = 0; i < stVrsticZacetek; i++)
            {
                for (int j = 0; j < stStolpcev; j++)
                {
                    // izmed moznih barvn eno nakljucno izberemo in ustvarimo nov mehurček
                    Color barva = Mehurcek.mozneBarve[random.Next(Mehurcek.mozneBarve.Length)];
                    mrezaMehurckov[i, j] = new Mehurcek(i, j, barva);
                }
            }
            // napolnimo tabelo do konca s praznimi (nevidnimi)
            for (int i = stVrsticZacetek; i < stVrstic; i++)
            {
                for (int j = 0; j < stStolpcev; j++)
                {
                    // izmed moznih barvn eno nakljucno izberemo in ustvarimo nov mehurček
                    Color barva = Mehurcek.mozneBarve[random.Next(Mehurcek.mozneBarve.Length)];
                    mrezaMehurckov[i, j] = new Mehurcek(i, j, barva);
                    mrezaMehurckov[i, j].JeVidn = false;
                }
            }
        }

        private void IgraForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush brush;
            Pen pen = new Pen(Color.Black);

            // risanje mehurckov v tabeli
            for (int i = 0; i < stVrstic; i++)
            {
                for (int j = 0; j < stStolpcev; j++)
                {
                    if (mrezaMehurckov[i, j].JeVidn)
                    {
                        // narišemo mehurček
                        brush = new SolidBrush(mrezaMehurckov[i, j].Barva);
                        g.FillEllipse(brush, mrezaMehurckov[i, j].X, mrezaMehurckov[i, j].Y, mrezaMehurckov[i, j].Velikost, mrezaMehurckov[i, j].Velikost);
                        // narišemo črno obrobo 
                        g.DrawEllipse(pen, mrezaMehurckov[i, j].X, mrezaMehurckov[i, j].Y, mrezaMehurckov[i, j].Velikost, mrezaMehurckov[i, j].Velikost);
                    }
                }
            }

            // risanje padajocih mehurckov
            for (int i = 0; i < NepripetiMehurckiXYBarva.Length; i++)
            {
                brush = new SolidBrush(NepripetiMehurckiXYBarva[i].Item3);
                g.FillEllipse(brush, NepripetiMehurckiXYBarva[i].Item1, NepripetiMehurckiXYBarva[i].Item2, Mehurcek.velikostMehurcka, Mehurcek.velikostMehurcka);
                g.DrawEllipse(pen, NepripetiMehurckiXYBarva[i].Item1, NepripetiMehurckiXYBarva[i].Item2, Mehurcek.velikostMehurcka, Mehurcek.velikostMehurcka);
            }

            // risanje puscice v smeri strela
            if (!seDogajaPremik())
            {
                double kot = Math.Atan2(polozajMiskeY - streljanMehurcekSredisceYZacetek, polozajMiskeX - streljanMehurcekSredisceXZacetek);
                
                Pen penPuscica = new Pen(Color.Black, velikostPuscice);
                penPuscica.CustomEndCap = new AdjustableArrowCap(velikostPuscice, velikostPuscice);
                g.DrawLine(penPuscica, streljanMehurcekSredisceXZacetek, streljanMehurcekSredisceYZacetek, streljanMehurcekSredisceXZacetek + (int)(dolzinaPuscice * Math.Cos(kot)), streljanMehurcekSredisceYZacetek + (int)(dolzinaPuscice * Math.Sin(kot)));
            }
            // risanje streljanjega mehurcka
            float strelX = streljanMehurcekSredisceX - Mehurcek.velikostMehurcka / 2;
            float strelY = streljanMehurcekSredisceY - Mehurcek.velikostMehurcka / 2;
            brush = new SolidBrush(streljanMehurcekBarva);
            g.FillEllipse(brush, strelX, strelY, Mehurcek.velikostMehurcka, Mehurcek.velikostMehurcka);
            g.DrawEllipse(pen, strelX, strelY, Mehurcek.velikostMehurcka, Mehurcek.velikostMehurcka);

            // naslednji
            brush = new SolidBrush(naslednjiMehurcekBarva);
            g.FillEllipse(brush, naslednjiMehurcekX, naslednjiMehurcekY, Mehurcek.velikostMehurcka * naslednjiMehurcekFaktorVelikosti, Mehurcek.velikostMehurcka * naslednjiMehurcekFaktorVelikosti);
            g.DrawEllipse(pen, naslednjiMehurcekX, naslednjiMehurcekY, Mehurcek.velikostMehurcka * naslednjiMehurcekFaktorVelikosti, Mehurcek.velikostMehurcka * naslednjiMehurcekFaktorVelikosti);

            // napis stevila tock
            napisSteviloTock.Location = new Point((int)(this.ClientSize.Width - napisSteviloTock.Width - 10), (int)(this.ClientSize.Height - Mehurcek.velikostMehurcka));
        }

        private List<Mehurcek> PoisciSosedeIsteBarve(Mehurcek mehurcek)
        {
            List<Mehurcek> sosedje = new List<Mehurcek>();
            HashSet<Mehurcek> obiskani = new HashSet<Mehurcek>();
            Queue<Mehurcek> vrsta = new Queue<Mehurcek>();

            // dodamo začetni mehurček
            vrsta.Enqueue(mehurcek);
            obiskani.Add(mehurcek);

            // izvedemo BFS algoritem (iskanje v širino)
            while (vrsta.Count > 0)
            {
                Mehurcek trenutni = vrsta.Dequeue();
                sosedje.Add(trenutni);

                int[] smeriVrstica = { 0, 0, -1, -1, 1, 1 };
                int[] smeriStolpec;

                // ker so vrstice mehurckov zamaknjene locimo smeri do sosedov, glede na vrstico v kateri se nahajamo
                if (trenutni.Vrstica % 2 == 0)
                {
                    smeriStolpec = new int[] { -1, 1, -1, 0, -1, 0 };
                }
                else
                {
                    smeriStolpec = new int[] { -1, 1, 0, 1, 0, 1 };
                }

                // imamo 6 smeri (sosednih položajev)
                for (int i = 0; i < smeriVrstica.Length; i++)
                {
                    int novaVrstica = trenutni.Vrstica + smeriVrstica[i];
                    int novStolpec = trenutni.Stolpec + smeriStolpec[i];

                    // pazimo da ne pademo iz tabele
                    if (LokacijaVeljavna(novaVrstica, novStolpec) && (mrezaMehurckov[novaVrstica, novStolpec].JeVidn) && (!obiskani.Contains(mrezaMehurckov[novaVrstica, novStolpec])) && (mrezaMehurckov[novaVrstica, novStolpec].Barva == mehurcek.Barva))
                    {
                        vrsta.Enqueue(mrezaMehurckov[novaVrstica, novStolpec]);
                        obiskani.Add(mrezaMehurckov[novaVrstica, novStolpec]);
                    }
                }
            }

            return sosedje;
        }

        private bool LokacijaVeljavna(int vrstica, int stolpec)
        {
            // preverimo ali je nova lokacija na veljavnem mestu v tabeli
            return (vrstica >= 0) && (vrstica < stVrstic) && (stolpec >= 0) && (stolpec < stStolpcev);
        }



        private void IzbrisiSosedeIsteBarve(Mehurcek mehurcek)
        {
            // poiščemo sosednje mehurčke iste barve
            SosediIsteBarve = PoisciSosedeIsteBarve(mehurcek);

            if (SosediIsteBarve.Count >= 3)
            {
                // poženemo animacijo brisanja mehurčkov
                stevecSlik = 0;
                timerAnimacijeBrisanja.Start();

                steviloTock += 10 * SosediIsteBarve.Count;
                napisSteviloTock.Text = $"Število točk: {steviloTock}";
            }
        }

        private HashSet<Mehurcek> PoisciNepripeteMehurcke()
        {
            HashSet<Mehurcek> obiskani = new HashSet<Mehurcek>();
            Queue<Mehurcek> vrsta = new Queue<Mehurcek>();

            // vsi v zgornji vrstici so pripeti
            for (int j = 0; j < stStolpcev; j++)
            {
                if (mrezaMehurckov[0, j].JeVidn)
                {
                    obiskani.Add(mrezaMehurckov[0, j]);
                    vrsta.Enqueue(mrezaMehurckov[0, j]);
                }
            }

            // vsi ki jih bomo obiskali bodo sosedi, in zato tudi pripeti
            while (vrsta.Count > 0)
            {
                Mehurcek trenutni = vrsta.Dequeue();

                int[] smeriVrstica = { 0, 0, -1, -1, 1, 1 };
                int[] smeriStolpec;

                // ker so vrstice mehurckov zamaknjene locimo smeri do sosedov, glede na vrstico v kateri se nahajamo
                if (trenutni.Vrstica % 2 == 0)
                {
                    smeriStolpec = new int[] { -1, 1, -1, 0, -1, 0 };
                }
                else
                {
                    smeriStolpec = new int[] { -1, 1, 0, 1, 0, 1 };
                }

                // imamo 6 smeri (sosednih položajev)
                for (int i = 0; i < smeriVrstica.Length; i++)
                {
                    int novaVrstica = trenutni.Vrstica + smeriVrstica[i];
                    int novStolpec = trenutni.Stolpec + smeriStolpec[i];

                    // pazimo da ne pademo iz tabele
                    if (LokacijaVeljavna(novaVrstica, novStolpec) && (mrezaMehurckov[novaVrstica, novStolpec].JeVidn) && (!obiskani.Contains(mrezaMehurckov[novaVrstica, novStolpec])))
                    {
                        vrsta.Enqueue(mrezaMehurckov[novaVrstica, novStolpec]);
                        obiskani.Add(mrezaMehurckov[novaVrstica, novStolpec]);
                    }
                }
            }


            // poznamo vse pripete, poiščemo nepripete
            HashSet<Mehurcek> nepripeti = new HashSet<Mehurcek>();
            for (int i = 0; i < stVrstic; i++)
            {
                for (int j = 0; j < stStolpcev; j++)
                {
                    if ((!obiskani.Contains(mrezaMehurckov[i, j])) && (mrezaMehurckov[i, j].JeVidn))
                    {
                        nepripeti.Add(mrezaMehurckov[i, j]);
                    }
                }
            }

            return nepripeti;
        }

        private void IzbrisiNepripete()
        {
            // v mrezi spremenimo nepripete na nevidne
            // zapomnimo si tocke, kjer so bili (za animacijo padanja)
            HashSet<Mehurcek> nepripeti = PoisciNepripeteMehurcke();
            if (nepripeti.Count == 0) return;

            NepripetiMehurckiXYBarva = new (float, float, Color)[nepripeti.Count];
            int stevec = 0;
            foreach (Mehurcek mehurcek in nepripeti)
            {
                mehurcek.JeVidn = false;
                NepripetiMehurckiXYBarva[stevec] = (mehurcek.X, mehurcek.Y, mehurcek.Barva);
                stevec++;
            }

            // poženemo animacijo padanja
            stevecNepripetih = NepripetiMehurckiXYBarva.Length;
            timerAnimacijePadanja.Start();
        }

        private void TimerAnimacijeBrisanja_Tick(object sender, EventArgs e)
        {
            stevecSlik++;

            if (stevecSlik <= maxSteviloSlik)
            {
                // izračunamo pomanjšano velikost mehurčka
                double faktorPomansanja = 1 - (double)stevecSlik / maxSteviloSlik;
                int novaVelikostMehurcka = (int)(Mehurcek.velikostMehurcka * faktorPomansanja);

                // vsem mehučkom nastavimo novo velikost
                foreach (Mehurcek trenutni in SosediIsteBarve)
                {
                    trenutni.Velikost = novaVelikostMehurcka;
                }
                this.Refresh();
            }
            else
            {
                // Končanje animacije
                timerAnimacijeBrisanja.Stop();
                foreach (Mehurcek trenutni in SosediIsteBarve)
                {
                    trenutni.JeVidn = false;
                    trenutni.Velikost = Mehurcek.velikostMehurcka;
                }
                this.Refresh();
                // izbrisemo nepripete mehurcke
                IzbrisiNepripete();
            }

            
        }

        private void timerAnimacijePadanja_Tick(object sender, EventArgs e)
        {
            // premaknemo vse nepripete en premik nizje
            for (int i = 0; i < NepripetiMehurckiXYBarva.Length; i++)
            {
                // vsi so padli pod rob okna
                if (stevecNepripetih == 0)
                {
                    timerAnimacijePadanja.Stop();
                    Array.Clear(NepripetiMehurckiXYBarva, 0, NepripetiMehurckiXYBarva.Length);
                    break;
                }

                NepripetiMehurckiXYBarva[i].Item2 += hitrostPadanja;
                if (this.ClientSize.Height < NepripetiMehurckiXYBarva[i].Item2) stevecNepripetih--;

                this.Refresh();
            }
        }

        private void IgraForm_ResizeEnd(object sender, EventArgs e)
        {
            // okno
            this.Width = (int) (this.Height * 0.7);

            // velikost mehurckov
            float velikostMehurcka = (float)(this.ClientSize.Width / (stStolpcev + 0.5));
            Mehurcek.velikostMehurcka = velikostMehurcka;
            for (int i = 0; i < stVrstic; i++)
            {
                for (int j = 0; j < stStolpcev; j++)
                {
                    mrezaMehurckov[i, j].Velikost = velikostMehurcka;
                }
            }

            // hitrosti premikanja
            hitrostStrela = this.ClientSize.Height / 100;
            hitrostPadanja = (int)(hitrostStrela * 1.5);

            // zacetni polozaj streljanja mehurcka
            streljanMehurcekSredisceXZacetek = (float)(this.ClientSize.Width / 2);
            streljanMehurcekSredisceYZacetek = (float)(this.ClientSize.Height - 2 * Mehurcek.velikostMehurcka);
            streljanMehurcekSredisceX = streljanMehurcekSredisceXZacetek;
            streljanMehurcekSredisceY = streljanMehurcekSredisceYZacetek;
            dolzinaPuscice = Mehurcek.velikostMehurcka * 1.2f;
            velikostPuscice = (float)Math.Ceiling(Mehurcek.velikostMehurcka / 10);

            // gumbi in napisi
            fontVelikost = (int)(Mehurcek.velikostMehurcka / 3);
            napisNaslednjiMehurcek.Font = new Font(fontStil, fontVelikost);
            napisSteviloTock.Font = new Font(fontStil, fontVelikost);
            gumbZamenjaj.Size = new Size(0, 0);
            gumbZamenjaj.Font = new Font(fontStil, fontVelikost);

            int vrsticaNapisovY = (int)(this.ClientSize.Height - Mehurcek.velikostMehurcka);
            napisNaslednjiMehurcek.Location = new Point(10, vrsticaNapisovY);
            napisSteviloTock.Location = new Point((int)(this.ClientSize.Width - napisSteviloTock.Width - 10), vrsticaNapisovY);
            gumbZamenjaj.Location = new Point((int)(this.ClientSize.Width / 2 - gumbZamenjaj.Width / 2), vrsticaNapisovY);
                        
            naslednjiMehurcekX = napisNaslednjiMehurcek.Location.X + napisNaslednjiMehurcek.Width;
            naslednjiMehurcekY = vrsticaNapisovY;

            this.Refresh();
        }

        private void IgraForm_Load(object sender, EventArgs e)
        {
            // začetna višina okna je 80% ekrana
            this.Height = (int)(0.8 * Screen.PrimaryScreen.Bounds.Height);
            IgraForm_ResizeEnd(sender, e);

            // postavimo na sredino ekrana
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);
        }

        private void IgraForm_Click(object sender, EventArgs e)
        {
            // če se še kak premik počakaj
            if (seDogajaPremik()) return;

            // poiščemo lokacijo klika
            polozajMiske(e);

            // izračunamo smer in hitrost premikanja ustreljenega mehurcka
            float razdaljaX = polozajMiskeX - streljanMehurcekSredisceXZacetek;
            float razdaljaY = polozajMiskeY - streljanMehurcekSredisceYZacetek;
            
            float dolzina = (float)Math.Sqrt(razdaljaX * razdaljaX + razdaljaY * razdaljaY);

            smerStrelaX = razdaljaX / dolzina * hitrostStrela;
            smerStrelaY = razdaljaY / dolzina * hitrostStrela;

            // nemoremo streljati navzdol
            if (0 <= smerStrelaY) return;

            timerStrela.Start();

            stevecStrelov++;
        }

        /// <summary>
        /// pridobi polozaj miske in posodobi spremenljivki polozajMiskeX in polozajMiskeY
        /// </summary>
        /// <param name="e"></param>
        private void polozajMiske(EventArgs e)
        {
            MouseEventArgs miska = e as MouseEventArgs;
            polozajMiskeX = miska.X;
            polozajMiskeY = miska.Y;
        }

        private void timerStrela_Tick(object sender, EventArgs e)
        {
            streljanMehurcekSredisceX += smerStrelaX;
            streljanMehurcekSredisceY += smerStrelaY;

            // preveri odboj od robu
            if (streljanMehurcekSredisceX - Mehurcek.velikostMehurcka / 2 < 0)
            {
                streljanMehurcekSredisceX = Mehurcek.velikostMehurcka / 2;
                smerStrelaX *= -1;
            }
            else if ((this.ClientSize.Width - Mehurcek.velikostMehurcka / 2) < streljanMehurcekSredisceX)
            {
                streljanMehurcekSredisceX = this.ClientSize.Width - Mehurcek.velikostMehurcka / 2;
                smerStrelaX *= -1;
            }

            // preveri trke
            Mehurcek zadetiMehurcek = mrezaMehurckov[0, 0];
            if (preveriTrk(streljanMehurcekSredisceX, streljanMehurcekSredisceY, ref zadetiMehurcek))
            {
                timerStrela.Stop();
                vstaviMehurcek(zadetiMehurcek);
                if (KonecIgre()) return; // če je konec takoj zakljuci
                pocakajInDodajVrstico();
            }

            this.Refresh();
        }

        private void vstaviMehurcek(Mehurcek zadetiMehurcek)
        {
            // poiscemo mesto kamor nov mehurcek vstavimo
            Mehurcek novoMesto = zadetiMehurcek;
            double minRazdalja = Mehurcek.velikostMehurcka + 1;

            int[] smeriVrstica = { 0, 0, -1, -1, 1, 1 };
            int[] smeriStolpec;

            if (zadetiMehurcek.Vrstica % 2 == 0)
            {
                smeriStolpec = new int[] { -1, 1, -1, 0, -1, 0 };
            }
            else
            {
                smeriStolpec = new int[] { -1, 1, 0, 1, 0, 1 };
            }

            for (int i = 0; i < smeriVrstica.Length; i++)
            {
                int novaVrstica = zadetiMehurcek.Vrstica + smeriVrstica[i];
                int novStolpec = zadetiMehurcek.Stolpec + smeriStolpec[i];

                if (LokacijaVeljavna(novaVrstica, novStolpec))
                {
                    double trenutnaRazdalja = razdalja(mrezaMehurckov[novaVrstica, novStolpec], streljanMehurcekSredisceX, streljanMehurcekSredisceY);
                    if ((!mrezaMehurckov[novaVrstica, novStolpec].JeVidn) && (trenutnaRazdalja < minRazdalja))
                    {
                        minRazdalja = trenutnaRazdalja;
                        novoMesto = mrezaMehurckov[novaVrstica, novStolpec];
                    }
                }
            }

            // imamo mesto za nov mehurcek
            novoMesto.Barva = streljanMehurcekBarva;
            novoMesto.JeVidn = true;

            // po potrebi izbrisemo sosede
            IzbrisiSosedeIsteBarve(novoMesto);

            // ustvari nov strelni mehurcek
            ustvariNovStrelniMehurcek();
        }

        /// <summary>
        /// streljan mehurcek nadomesti z naslednjim v vrsti in naredi novega naslednjega v vrsti
        /// </summary>
        private void ustvariNovStrelniMehurcek()
        {
            streljanMehurcekSredisceX = streljanMehurcekSredisceXZacetek;
            streljanMehurcekSredisceY = streljanMehurcekSredisceYZacetek;
            streljanMehurcekBarva = naslednjiMehurcekBarva;
            naslednjiMehurcekBarva = Mehurcek.mozneBarve[random.Next(Mehurcek.mozneBarve.Length)];
        }

        private bool preveriTrk(float x, float y, ref Mehurcek zadetiMehurcek)
        {
            // poiščemo lokacijo mehurcka v tabeli
            int vrstica = (int)(y / Mehurcek.velikostMehurcka);
            if (vrstica % 2 == 1) x -= Mehurcek.velikostMehurcka / 2;
            int stolpec = (int)(x / Mehurcek.velikostMehurcka);

            // zadane strop
            if (vrstica == 0)
            {
                zadetiMehurcek = mrezaMehurckov[1, stolpec];
                return true;
            }

            for (int i = vrstica - 2; i < vrstica + 2; i++)
            {
                for (int j = stolpec - 2; j < stolpec + 2; j++)
                {
                    if (LokacijaVeljavna(i, j))
                    {
                        if ((mrezaMehurckov[i, j].JeVidn) && (razdalja(mrezaMehurckov[i, j], x, y) < Mehurcek.velikostMehurcka))
                        {
                            // zgodi se trk
                            zadetiMehurcek = mrezaMehurckov[i, j];
                            return true;
                        }
                    }
                }
            }
            // ni trka
            return false;
        }

        /// <summary>
        /// Vrne razdaljo med tocko (x,y) in srediscem mehurcka
        /// </summary>
        /// <param name="mehurcek"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private double razdalja(Mehurcek mehurcek, float x, float y)
        {
            return Math.Sqrt(Math.Pow(x - mehurcek.SredisceX, 2) + Math.Pow(y - mehurcek.SredisceY, 2));
        }

        private void gumbZamenjaj_Click(object sender, EventArgs e)
        {
            if (seDogajaPremik()) return; // če kliknemo, ko se še izvajajo animacije, naj se ne zgodi nič
            
            // zamenjaj naslednja mehurcka
            Color temp = streljanMehurcekBarva;
            streljanMehurcekBarva = naslednjiMehurcekBarva;
            naslednjiMehurcekBarva = temp;

            this.Refresh();
        }

        private void timerDodatneVrstice_Tick(object sender, EventArgs e)
        {
            // na vsak tick (20 sekund) dodaj vrstico
            DodajVrstico();
            // ko dodamo vrstico ponastavimo strele
            stevecStrelov = 0;

            this.Refresh();
        }

        /// <summary>
        /// požene timer, ki počaka da se prenehajo izvajati vse animacije, in nato doda novo vrstico
        /// </summary>
        private void pocakajInDodajVrstico()
        {
            timerCakaj.Start();
        }

        private void timerCakaj_Tick(object sender, EventArgs e)
        {
            // če se ne izvaja nobena animacija, dodaj novo vrstico
            if (!seDogajaPremik())
            {
                timerCakaj.Stop();
                if (aliTrebaDodatiVrstico()) DodajVrstico();
            }
            this.Refresh();
        }

        private bool aliTrebaDodatiVrstico()
        {
            // ali je treba dodati vrstico
            if (stevecStrelov < maxSteviloStrelov)
            {
                return false;
            }
            else
            {
                stevecStrelov = 0;
                timerDodatneVrstice.Stop();
                timerDodatneVrstice.Start();
                return true;
            }
        }

        private void DodajVrstico()
        {        
            // vse prepisi eno vrstico nizje
            for (int i = stVrstic - 1; i > 1; i--)
            {
                for (int j = 0; j < stStolpcev; j++)
                {
                    mrezaMehurckov[i, j].JeVidn = mrezaMehurckov[i - 1, j].JeVidn;
                    mrezaMehurckov[i, j].Barva = mrezaMehurckov[i - 1, j].Barva;
                }
            }
            for (int j = 0; j < stStolpcev; j++)
            {
                mrezaMehurckov[1, j].JeVidn = mrezaMehurckov[0, j].JeVidn;
                mrezaMehurckov[1, j].Barva = mrezaMehurckov[0, j].Barva;
            }

            // nova prva vrstica
            for (int j = 0; j < stStolpcev; j++)
            {
                mrezaMehurckov[0, j].Barva = Mehurcek.mozneBarve[random.Next(Mehurcek.mozneBarve.Length)];
            }
                        
            // ponovno preveri nepripete
            IzbrisiNepripete();

            // preveri konec igre
            KonecIgre();
        }

        /// <summary>
        /// Vrne true, če je igre konec. Poleg tega igralcu sporoci, da je igre konec in odpre naslednje okno (lestvico)
        /// </summary>
        /// <returns></returns>
        private bool KonecIgre()
        {
            for (int j = 0; j < stStolpcev; j++)
            {
                if (mrezaMehurckov[stVrstic - 1, j].JeVidn)
                {
                    MessageBox.Show($"Konec Igre: Zbrali ste {steviloTock} točk.");
                    
                    Program.pozeniOknoLestvica();

                    return true;
                }
            }
            return false;
        }
       
        /// <summary>
        /// Vrne true, če se izvaja katera od animacij
        /// </summary>
        /// <returns></returns>
        private bool seDogajaPremik()
        {
            if ((timerStrela.Enabled || timerAnimacijeBrisanja.Enabled || timerAnimacijePadanja.Enabled)) return true;
            else return false;
        }

        private void OknoIgra_MouseMove(object sender, MouseEventArgs e)
        {
            // ce se doga
            if (seDogajaPremik()) return;

            // posodobimo polozaj miske in ponovno narisemo
            polozajMiske(e);
            this.Refresh();
        }

        private void OknoIgra_FormClosing(object sender, FormClosingEventArgs e)
        {
            // če zapremo trenutno okno, naj se zaprejo vsa okna in program ustavi
            Application.Exit();
        }
    }
}
