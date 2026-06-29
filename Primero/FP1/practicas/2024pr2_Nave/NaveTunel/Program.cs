using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;

namespace naves
{
    class Program
    {
        const bool DEBUG = false;
        const int ANCHO = 30, ALTO = 15;
        static Random rnd = new Random(11);

        static void Main()
        {
            int[] suelo = new int[ANCHO], // Límites del túnel
                  techo = new int[ANCHO];

            int naveC, naveF;
            int balaC, balaF;
            int enemigoC, enemigoF;
            int colC, colF; // Inicializamos la colisión
            Console.SetWindowSize(ANCHO * 2, ALTO + 0); // El número que le sumamos al alto es para poder ver el debug (6)

            bool crashNave = false; // booleana declarada al inicio, indica si el jugador se ha dado

            IniciaTunel(suelo, techo);

            // Inicia nave
            naveC = ANCHO / 3;
            naveF = (techo[naveC] + suelo[naveC]) / 2; // para que se genere en el medio

            while (!crashNave)
            {
                char ch = LeeInput();
                if (ch == 'q')
                {
                    crashNave = true; // aunque no haya pasado la utilizamos para ahorrar variables
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Gracias por jugar!!");
                }
                else if (ch == 'p')
                {
                    Thread.Sleep(2000); // que dure
                    Console.SetCursorPosition(0, ALTO);
                }
                else
                {
                    AvanzaTunel(suelo, techo);
                    GeneraAvanzaEnemigo(ref enemigoC, ref enemigoF, suelo, techo); // solo pasamos por ref los parámetros que vamos a cambiar en el método
                    AvanzaNave(ch, ref naveC, ref naveF, enemigoC, enemigoF, suelo, techo);
                    crashNave = ColisionNave(naveC, naveF, suelo, techo, enemigoC, enemigoF);
                    GeneraAvanzaBala(ch, ref balaC, ref balaF, naveC, naveF, enemigoC, enemigoF, suelo, techo);
                    ColisionBala(ref balaC, ref balaF, ref enemigoC, ref enemigoF, suelo, techo, ref colC, ref colF);
                    Render(suelo, techo, naveC, naveF, balaC, balaF, enemigoC, enemigoF, crashNave, colC, colF);

                    if (DEBUG) // para activarlo ponemos true a la constante y cambiamos el windowsSize arriba
                    {
                        Console.SetCursorPosition(0, ALTO);
                        Console.WriteLine("Columna enemigo: " + enemigoC);
                        Console.WriteLine($"Columna nave: {naveC}, Fila nave: {naveF}");
                        Console.WriteLine("Columna bala: " + balaC + ". Fila bala: " + balaF);
                        Console.WriteLine($"Colisión bala: Columna: {colC}, Fila: {colF}");
                        Console.WriteLine("Crash nave: " + crashNave);
                    }
                    Thread.Sleep(120);
                }
            }
        } // corchete main

        static char LeeInput()
        {
            char ch = ' ';
            if (Console.KeyAvailable)
            {
                string dir = Console.ReadKey(true).Key.ToString();
                if (dir == "A" || dir == "LeftArrow") ch = 'l';
                else if (dir == "D" || dir == "RightArrow") ch = 'r';
                else if (dir == "W" || dir == "UpArrow") ch = 'u';
                else if (dir == "S" || dir == "DownArrow") ch = 'd';
                else if (dir == "X" || dir == "Spacebar") ch = 'x'; // bomba                   
                else if (dir == "P") ch = 'p'; // pausa					
                else if (dir == "Q" || dir == "Escape") ch = 'q'; // salir
                while (Console.KeyAvailable) Console.ReadKey().Key.ToString();
            }
            return ch;
        }
    }
}