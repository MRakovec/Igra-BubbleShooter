using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BubbleShooter
{
    public static class Program
    {
        static OknoVpis vpis;
        static OknoIgra igra;
        static OknoLestvica lestvica;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            pozeniOknoVpis();
        }

        /// <summary>
        /// ustvari novo oknoVpis s katerim se začne program.
        /// </summary>
        public static void pozeniOknoVpis()
        {
            vpis = new OknoVpis();
            Application.Run(vpis);
        }

        /// <summary>
        /// ustvari novo okno igra in skrije okno za vpis
        /// </summary>
        public static void pozeniOknoIgra()
        {
            vpis.Hide();
            igra = new OknoIgra();
            igra.Show();

        }

        /// <summary>
        /// ustvari novo okno lestvice in skrije okno igre.
        /// </summary>
        public static void pozeniOknoLestvica()
        {
            igra.Hide();
            lestvica = new OknoLestvica();
            lestvica.Show();
        }

        /// <summary>
        /// skrije lestvico in ustvari novo okno igra
        /// </summary>
        public static void ponovnoPozeniOknoIgra()
        {
            lestvica.Hide();
            igra = new OknoIgra();
            igra.Show();
        }
    }
}
