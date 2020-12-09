using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FatRajzol_M5APWK
{
    public partial class Form1 : Form
    {
        public static readonly Intervallum doles = new Intervallum() { min = 0.3, max = 0.5 };
        public static readonly Intervallum helyzet = new Intervallum() { min = 0.4, max = 0.6 };
        public static readonly Intervallum rovidules = new Intervallum() { min = 0.2, max = 0.5 };
        public static readonly Intervallum roviduleskozepso = new Intervallum() { min = 0.2, max = 0.3 };
        public static readonly Intervallum szinrandom = new Intervallum() { min = 0.7, max = 0.9 };
        public static readonly Intervallum szinfuzzy = new Intervallum() { min = -0.1, max = 0.1 };
        public static readonly Intervallum dolesrand = new Intervallum() { min = -0.1, max = 0.1 };
        public static readonly Intervallum meret = new Intervallum() { min = 60, max = 80 };
        public static readonly int melyseg = 8;
        public static Random random = new Random();
        public static Pen p = new Pen(Color.Black);
        public static Graphics graphics;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.BackColor = Color.White;
            pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.Controls.Add(pictureBox1);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            graphics = e.Graphics;
            randomfa(pictureBox1.Width/2, pictureBox1.Height/2, 3);
            graphics = e.Graphics;
        }
        public static double myrand(Intervallum i)
        {
            double rand1 = random.NextDouble(); 
            return i.min + (i.max - i.min) * rand1; 
        }
        public static uint ColorToUInt(Color color)
        {
            return (uint)((color.A << 24) | (color.R << 16) |
                          (color.G << 8) | (color.B << 0));
        }
        public static Color UIntToColor(uint color)
        {
            byte a = (byte)(color >> 24);
            byte r = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte b = (byte)(color >> 0);
            return Color.FromArgb(a, r, g, b);
        }
        public static void myline(double x1, double y1, double x2, double y2, uint color)
        {
            p.Color = UIntToColor(color);
            graphics.DrawLine(p,(float)x1, (float)y1, (float)x2, (float)y2);
        }
        public static void fat_rajzol(AgAdat[] agak, int agszam, int n, double x, double y, double hossz, double szog, double atlatszosag, double zold, double r)
        {
            if (n == 0)
            {
                return;
            }
            double atlb = (uint)atlatszosag * 255;
            double vorosb = atlb * 2 / 3;
            double zoldb = (int)(0xFF - atlb) * (zold + myrand(szinfuzzy));
            myline(x, y, x + hossz * Math.Cos(szog), y - hossz * Math.Sin(szog), (uint)(Convert.ToByte(vorosb) << 24 | Convert.ToByte(zoldb) << 16 | 0 << 8 | Convert.ToByte(atlb) << 0));
            int i;
            for (i = 0; i < agszam; ++i)
            {
                fat_rajzol(agak, agszam, n - 1, x + hossz * agak[i].helyzet * Math.Cos(szog), y - hossz * agak[i].helyzet * Math.Sin(szog), hossz * (1 - agak[i].rovidules), szog + agak[i].doles + myrand(dolesrand), atlatszosag * 0.8, zold, r * 0.88);
            }
        }
        public static void randomfa(double x, double y, int agakszama)
        {
            AgAdat[] agak = new AgAdat[agakszama];
            double d1 = -myrand(doles);
            double d2 = myrand(doles);
            for (int i = 0; i < agakszama; ++i)
            {
                agak[i] = new AgAdat();
                agak[i].doles = d1 + (d2 - d1) / (agakszama - 1) * i;
                agak[i].helyzet = myrand(helyzet);
                agak[i].rovidules = myrand(rovidules);
            }
            agak[ agakszama / 2].helyzet = 1.0; 
            agak[agakszama / 2].rovidules = myrand(rovidules);
            fat_rajzol(agak, agakszama, melyseg, x, y, myrand(meret), 90 * 3.1415 / 180, 1, myrand(szinrandom), 3);
        }
    }
}
    public class Intervallum
    {
        public double min;
        public double max;
    }
    public class AgAdat
    {
        public double doles;
        public double helyzet; 
        public double rovidules; 
    }


