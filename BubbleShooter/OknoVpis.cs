using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BubbleShooter
{
    public partial class OknoVpis : Form
    {
        public static string Vzdevek = "";

        // napisi in gumbi
        private Label napisNaslov;
        private Label napisVzdevek;
        private TextBox besedilnoPoljeVzdevek;
        private Button gumbVpisiMe;

        private string fontStil = OknoIgra.fontStil;
        private int naslovFontVelikost;
        private int fontVelikost;

        public OknoVpis()
        {
            InitializeComponent();

            // velikost pisave
            fontVelikost = (int)(this.ClientSize.Height / 20);
            naslovFontVelikost = 3 * fontVelikost;

            // napisi in gumbi
            napisNaslov = new Label();
            napisNaslov.AutoSize = true;
            napisNaslov.Font = new Font(fontStil, naslovFontVelikost, FontStyle.Bold);
            napisNaslov.Text = "IGRA BUBBLE SHOOTER";

            napisVzdevek = new Label();
            napisVzdevek.AutoSize = true;
            napisVzdevek.Font = new Font(fontStil, fontVelikost);
            napisVzdevek.Text = "Vpiši vzdevek: ";

            besedilnoPoljeVzdevek = new TextBox();
            besedilnoPoljeVzdevek.AutoSize = false;
            besedilnoPoljeVzdevek.Font = new Font(fontStil, fontVelikost);
            besedilnoPoljeVzdevek.Size = new Size(napisVzdevek.Width, napisVzdevek.Height);

            gumbVpisiMe = new Button();
            gumbVpisiMe.AutoSize = true;
            gumbVpisiMe.Font = new Font(fontStil, fontVelikost);
            gumbVpisiMe.Text = "Vpiši me";
            gumbVpisiMe.Click += gumbVpisiMe_Click;

            this.Controls.Add(napisNaslov);
            this.Controls.Add(napisVzdevek);
            this.Controls.Add(besedilnoPoljeVzdevek);
            this.Controls.Add(gumbVpisiMe);
        }

        private void OknoVpis_Load(object sender, EventArgs e)
        {
            this.Height = (int)(0.3 * Screen.PrimaryScreen.Bounds.Height);
            OknoVpis_ResizeEnd(sender, e);

            // postavimo na sredino ekrana
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);
        }

        private void OknoVpis_ResizeEnd(object sender, EventArgs e)
        {
            this.Width = this.Height * 2;

            // velikost pisave
            fontVelikost = (int)(this.ClientSize.Height / 20);
            naslovFontVelikost = 2 * fontVelikost;
            gumbVpisiMe.Size = new Size(0, 0); // resetira velikost gumba

            // popravi napise in gumbe
            napisNaslov.Font = new Font(fontStil, naslovFontVelikost, FontStyle.Bold);
            napisVzdevek.Font = new Font(fontStil, fontVelikost);
            besedilnoPoljeVzdevek.Size = new Size(napisVzdevek.Width, (int)(napisVzdevek.Height * 1.2));
            besedilnoPoljeVzdevek.Font = new Font(fontStil, fontVelikost);
            gumbVpisiMe.Font = new Font(fontStil, fontVelikost);

            napisNaslov.Location = new Point(this.ClientSize.Width / 2 - napisNaslov.Size.Width / 2, this.ClientSize.Height / 4 - napisNaslov.Size.Height / 2);
            napisVzdevek.Location = new Point(this.ClientSize.Width / 2 - (napisVzdevek.Size.Width + besedilnoPoljeVzdevek.Size.Width) / 2, this.ClientSize.Height / 2 - napisVzdevek.Size.Height / 2);
            besedilnoPoljeVzdevek.Location = new Point(napisVzdevek.Location.X + napisVzdevek.Size.Width, this.ClientSize.Height / 2 - besedilnoPoljeVzdevek.Size.Height / 2);
            gumbVpisiMe.Location = new Point(this.ClientSize.Width / 2 - gumbVpisiMe.Size.Width / 2, this.ClientSize.Height * 3 / 4 - gumbVpisiMe.Size.Height);
        }

        private void gumbVpisiMe_Click(object sender, EventArgs e)
        {
            string vnos = besedilnoPoljeVzdevek.Text;

            // preveri, da vzdevek vsebuje le črke in številke
            if ((vnos.Any(char.IsLetter) && vnos.All(char.IsLetterOrDigit)))
            {
                Vzdevek = vnos;
                Program.pozeniOknoIgra();
            }
            else
            {               
                MessageBox.Show("Nepravilni vzdevek.\nVzdevek mora vsebovati samo črke in številke, ter vsaj eno črko.");               
            }
        }

        private void OknoVpis_FormClosing(object sender, FormClosingEventArgs e)
        {
            // če zapremo trenutno okno, naj se zaprejo vsa okna in program ustavi
            Application.Exit();
        }
    }
}
