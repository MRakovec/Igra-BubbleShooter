using System;
using System.Drawing;
using System.Linq;

namespace BubbleShooter
{
    public class Mehurcek
    {
        public static float velikostMehurcka = 40;
        public static Color[] mozneBarve = { Color.Red, Color.Blue, Color.Yellow, Color.Green, Color.HotPink };

        private float _velikost;
        private Color _barva;
        private int _vrstica;
        private int _stolpec;
        private bool _JeVidn;

        public Mehurcek(int vrstica, int stolpec, Color barva)
        {
            this._barva = barva;
            this.Velikost = velikostMehurcka;
            this._vrstica = vrstica;
            this._stolpec = stolpec;
            this.JeVidn = true;
        }

        public Color Barva
        {
            get { return this._barva; }
            set
            {
                if (mozneBarve.Contains(value)) this._barva = value;
                else throw new ArgumentException($"{value} ni dovoljena barva.");
            }
        }

        public int Vrstica
        {
            get { return this._vrstica; }
        }

        public int Stolpec
        {
            get { return this._stolpec; }
        }

        public bool JeVidn
        {
            get { return this._JeVidn; }
            set { this._JeVidn = value; }
        }

        public float Velikost
        {
            get { return this._velikost; }
            set { this._velikost = value; }
        }

        public float X
        {
            get
            {
                if (this.Vrstica % 2 == 0) return this.Stolpec * velikostMehurcka;
                else return this.Stolpec * velikostMehurcka + velikostMehurcka / 2;
            }
        }

        public float Y
        {
            get { return this.Vrstica * velikostMehurcka; }
        }

        public float SredisceX
        {
            get { return this.X + this.Velikost / 2; }
        }
        public float SredisceY
        {
            get { return this.Y + this.Velikost / 2; }
        }
    }
}
