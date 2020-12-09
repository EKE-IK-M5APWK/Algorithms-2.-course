using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egyiranyu_Lancolt_Lista
{
    class Program
    {
        class VeremCsomoPont
        {
            public int kulcs;
            public VeremCsomoPont kovetkezo;
            public VeremCsomoPont(int kulcs)
            {
                this.kulcs = kulcs;
                this.kovetkezo = null;
            }
        }
        class Verem
        {
            internal VeremCsomoPont eleje, vege;

            public Verem()
            {
                this.eleje = this.vege = null;
            }
            public void push(int kulcs)
            {
                VeremCsomoPont s = new VeremCsomoPont(kulcs);
                if (this.vege == null)
                {
                    this.eleje = this.vege = s;
                    return;
                }
                this.vege.kovetkezo = s;
                this.vege = s;
            }
            public void pop()
            {
                if (this.eleje == null)
                    return;
                VeremCsomoPont temp = this.eleje;
                this.eleje = this.eleje.kovetkezo;
                if (this.eleje == null)
                    this.vege = null;
            }
        }
        public static void Main(String[] args)
        {
            Verem pelda = new Verem();
            pelda.push(1);
            pelda.push(2);
            pelda.pop();
            pelda.pop();
            pelda.push(3);
            pelda.push(4);
            pelda.push(5);
            pelda.pop();
            Console.WriteLine("A Verem Eleje : " + pelda.eleje.kulcs);
            Console.WriteLine("A Verem Vége : " + pelda.vege.kulcs);
            Console.ReadLine();
        }
    }
}
