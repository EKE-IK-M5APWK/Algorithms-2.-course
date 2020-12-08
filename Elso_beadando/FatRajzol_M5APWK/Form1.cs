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
        /* a középső ág (tulajdonképpen a törzs) rövidülése. erősen befolyásolja a magasságot! */
        public static readonly Intervallum roviduleskozepso = new Intervallum() { min = 0.2, max = 0.3 };
        public static readonly Intervallum szinrandom = new Intervallum() { min = 0.7, max = 0.9 };
        public static readonly Intervallum szinfuzzy = new Intervallum() { min = -0.1, max = 0.1 };
        public static readonly Intervallum dolesrand = new Intervallum() { min = -0.1, max = 0.1 };
        public static readonly Intervallum meret = new Intervallum() { min = 60, max = 80 };
        public static readonly int melyseg = 8;
        public static Random random = new Random();
        public static Pen p = new Pen(Color.Black);
        public static Graphics g;
        public static List<Tuple<PointF,Color,double>> curvePoints = new List<Tuple<PointF, Color,double>>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Dock the PictureBox to the form and set its background to white.
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.BackColor = Color.White;
            // Connect the Paint event of the PictureBox to the event handler method.
            pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // Add the PictureBox control to the Form.
            this.Controls.Add(pictureBox1);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            randomfa(0, 0, 3);
            g = this.CreateGraphics();
            foreach (var item in curvePoints)
            {
                p.Color = item.Item2;
                g.DrawEllipse(p, item.Item1.X, item.Item1.Y, (float)item.Item3 * 2, (float)item.Item3 * 2);
            }
        }
        public static double myrand(Intervallum i)
        {
            double rand1 = random.NextDouble(); // ez 0 és 1 között lesz
            return i.min + (i.max - i.min) * rand1; // beskálázva min és max közé
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
        /* húz egy egymást átfedő körökből álló vonalat. az sdl_gfx thickline függvénye bugos.
        * ez pedig jobban kezeli a tört szám sugarat is. */
        public static void myline(double x1, double y1, double x2, double y2, uint color, double r)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;
            double lepes = Math.Sqrt(dx * dx + dy * dy) / r;
            if (lepes != 0)
            {
                dx /= lepes;
                dy /= lepes;
            }
            double i;
            for (i = 0; i <= lepes; ++i)
            {
                PointF point = new PointF((float)(x1 + dx * i), (float)(y1 + dy * i));
                curvePoints.Add(new Tuple<PointF, Color, double>(point,UIntToColor(color),r));
            }
        }
        /* ez rajzolja a fa ágait. agak[agszam] tömb adatai alapján, n lépés van vissza a rekurzióból. */
        public static void fat_rajzol(AgAdat[] agak, int agszam, int n, double x, double y, double hossz, double szog, double atlatszosag, double zold, double r)
        {
            if (n < 0)
            {
                return;
            }
            double atlb = (uint)atlatszosag * 255;
            double vorosb = atlb * 2 / 3;
            double zoldb = (int)(0xFF - atlb) * (zold + myrand(szinfuzzy));
            myline(x, y, x + hossz * Math.Cos(szog), y - hossz * Math.Sin(szog), (uint)(Convert.ToByte(vorosb) << 24 | Convert.ToByte(zoldb) << 16 | 0 << 8 | Convert.ToByte(atlb) << 0), r);
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
                agak[i].doles = d1 + (d2 - d1) / (agakszama - 1) * i; // nem random, hanem d1-tol d2-ig
                agak[i].helyzet = myrand(helyzet);
                agak[i].rovidules = myrand(rovidules);
            }
            agak[ agakszama / 2].helyzet = 1.0; // hogy a törzs (középső ág) mindig nőjön tovább
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
        public double doles; // merre dől az eredeti ághoz képest (radián)
        public double helyzet; // honnan indul ki (0 = legalul, 1 = legfelül)
        public double rovidules; // mennyit rövidül (0 = semennyit, 1 = lenullázódik)
    }


