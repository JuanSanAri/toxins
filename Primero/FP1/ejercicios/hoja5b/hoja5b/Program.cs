using System;
using System.Threading;

namespace hoja5b
{
    class Program
    {
        static void Main(string[] args)
        {
            const int ANCHO = 7; 
            int pistaIni = 5;  
            int posCoche = ANCHO / 2;
            Random rnd = new Random();

            int i = -2;
            int j = 2;

            Console.Write("Velocidad de refresco (ms): ");
            int delta = int.Parse(Console.ReadLine());

            bool jugando = true;

            while (jugando)
            {
                string dir = "";
                if (Console.KeyAvailable)
                {
                    dir = Console.ReadKey(true).KeyChar.ToString();
                }

                if (dir == "a" && posCoche > 0)
                {
                    posCoche--;
                }
                else if (dir == "d" && posCoche < ANCHO - 1)
                {
                    posCoche++;
                }

                int curva = rnd.Next(i, j);
                pistaIni += curva;
                if (curva > 0)
                {
                    posCoche = posCoche - 1;
                }
                else 
                {
                    posCoche = posCoche + 1;
                }


                if (pistaIni < 0) pistaIni = 0;
                if (pistaIni > Console.WindowWidth - ANCHO - 1) pistaIni = Console.WindowWidth - ANCHO - 1;

                if (posCoche < 0 || posCoche >= ANCHO)
                {
                    jugando = false;
                    DibujarLinea(pistaIni, ANCHO, posCoche, true);
                    Console.WriteLine("Crash!!");
                }

                DibujarLinea(pistaIni, ANCHO, posCoche);

                System.Threading.Thread.Sleep(delta);
            }
        }
        static void DibujarLinea(int pistaIni, int ANCHO, int posCoche, bool choque = false)
        {
            string pista = new string(' ', pistaIni) + "|";

            for (int i = 0; i < ANCHO; i++)
            {
                if (i == posCoche)
                {
                    pista += "#";
                }
                else
                {
                    pista += ".";
                }
            }

            if (choque)
            {
                pista += "*";
            }
            else
            {
                pista += "|";
            }

            Console.WriteLine(pista);
        }
    }
}
