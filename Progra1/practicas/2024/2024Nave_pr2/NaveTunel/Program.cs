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
            int balaC = -1, balaF = -1;
            int enemigoC = -1, enemigoF = -1;
            int colC = -1, colF = -1; // Inicializamos la colisión
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



        static void RenderTunel(int[] suelo, int[] techo)
        {
            Console.Clear();
            for (int i = 0; i < ANCHO; i++)
            {
                for (int j = 0; j < ALTO; j++)
                {
                    if (j <= techo[i] || j >= suelo[i]) // escribimos por debajo de techo (número) y por encima de suelo (número)
                    {
                        Console.SetCursorPosition(2 * i, j);
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write("  ");
                    }
                }
            }
            Console.ResetColor();
        }


        static void IniciaTunel(int[] suelo, int[] techo) // inicializacón
        {
            techo[ANCHO - 1] = 0;
            suelo[ANCHO - 1] = ALTO - 1;

            for (int i = 0; i < ANCHO - 1; i++) // ejecutamos el método a lo largo de ANCHO
            {
                AvanzaTunel(suelo, techo);
            }
        }

        static void AvanzaTunel(int[] suelo, int[] techo)
        {
            for (int i = 0; i < ANCHO - 1; i++)
            { // desplazamiento de eltos a la izda
                techo[i] = techo[i + 1];
                suelo[i] = suelo[i + 1];
            }

            int s, t; // ultimas posiciones de suelo y techo
            s = suelo[ANCHO - 1];
            t = techo[ANCHO - 1];

            // generamos nuevos valores para esas ultimas posiciones
            int opt = rnd.Next(5); // 5 alternativas
            if (opt == 0 && s < ALTO - 1) { s++; t++; } // tunel baja           
            else if (opt == 1 && t > 0) { s--; t--; } // sube
            else if (opt == 2 && s - t > 7) { s--; t++; } // estrecha
            else if (opt == 3) // ensancha
            {
                if (s < ALTO - 1) s++;
                if (t > 0) t--;
            } // con 4 se deja igual, no se hace nada

            // asignamos ultimas posiciones en el array
            suelo[ANCHO - 1] = s;
            techo[ANCHO - 1] = t;
        }

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


        static void GeneraAvanzaEnemigo(ref int enemigoC, ref int enemigoF, int[] suelo, int[] techo)
        {
            if (enemigoC < 0) // cuando no haya enemigo enemigoC = -1
            {
                if (rnd.Next(0, 4) == 3) //25% prob de generar, por eso el random [0-4)
                {
                    enemigoC = ANCHO - 2; // -2 porque me apetece que sea ahí
                    enemigoF = rnd.Next(techo[enemigoC], suelo[enemigoC]); // una fila random entre el techo y el suelo
                }
            }
            else
                enemigoC--;
        }



        static void AvanzaNave(char ch, ref int naveC, ref int naveF, int enemigoC, int enemigoF, int[] suelo, int[] techo)
        {
            if (naveC < ANCHO && naveC > 0 && naveF > techo[naveC] && naveF < suelo[naveC] && !(naveC == enemigoC && naveF == enemigoF)) // comprobamos que solo se pueda introducir teclado cuando no esté en colisión
            {
                if (ch == 'l') { naveC--; }
                else if (ch == 'r') { naveC++; }
                else if (ch == 'u') { naveF--; }
                else if (ch == 'd') { naveF++; }
            }
        }


        static void GeneraAvanzaBala(char ch, ref int balaC, ref int balaF, int naveC, int naveF, int enemigoC, int enemigoF, int[] suelo, int[] techo)
        {
            if (balaC < 0 && ch == 'x') // si no hay bala (-1), al pulsar la tecla correspondiente se genera justo delante de la nave
            {
                balaC = naveC + 1;
                balaF = naveF;
            }
            if (balaC != -1 && balaF > techo[balaC] && balaF < suelo[balaC] && !(balaC == enemigoC && balaF == enemigoF)) // si no ha colsionado, avanza
                balaC++;
        }


        static bool ColisionNave(int naveC, int naveF, int[] suelo, int[] techo, int enemigoC, int enemigoF)
        {
            return ((enemigoF == naveF && naveC == enemigoC) || naveF <= techo[naveC] || naveF >= suelo[naveC]); // solo si ocurre algo de esto es true
        }


        static void ColisionBala(ref int balaC, ref int balaF, ref int enemigoC, ref int enemigoF, int[] suelo, int[] techo, ref int colC, ref int colF)
        {
            if (balaC >= ANCHO) // si se sale sin tocar ni pared ni enemigo por la derecha
            {
                balaC = -1;
            }
            else if (balaC == enemigoC && balaF == enemigoF) // colisión enemigo
            {
                colF = balaF;
                colC = balaC;
                enemigoC = -1;
                balaC = -1;
            }
            else if (balaC > -1 && balaF >= suelo[balaC]) // colisión suelo
            {
                colF = balaF; // le damos las coordenadas de la colisión
                colC = balaC;
                suelo[balaC] = colF + 1; // para poder eliminar el suelo/techo, este adquiriendo el valor de colF -1
                balaC = -1; // una vez eliminado el suelo/techo, reseteamos colisión
            }
            else if (balaC > -1 && balaF <= techo[balaC]) // colisión techo
            {
                colF = balaF;
                colC = balaC;
                techo[balaC] = colF - 1;
                balaC = -1;
            }
            else // si no ocurre nada de esto, es que no hay colisión, por lo que la dejamos desactivada
            {
                colF = -1;
                colC = -1;
            }
        }

        static void Render(int[] suelo, int[] techo, int naveC, int naveF, int balaC, int balaF, int enemigoC, int enemigoF, bool crashNave, int colC, int colF)
        {
            RenderTunel(suelo, techo); // render aquí

            // Render enemigo
            if (enemigoC > -1)
            {
                Console.SetCursorPosition(2 * enemigoC, enemigoF); // recordar que está *2 en x(ANCHO) todo el programa/juego
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("<>");
            }

            // Render nave
            Console.SetCursorPosition(2 * naveC, naveF);
            if (crashNave) // al chocarnos debido a la lógica del Main se sale del render y se deja de actualizar
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("*pum*");
                Thread.Sleep(500); // un poco de tiempo para poder apreciar el choque
                Console.Clear();
                Console.WriteLine("Has perdido!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("->");
            }

            // Render bala
            if (balaC > -1)
            {
                Console.SetCursorPosition(2 * balaC, balaF);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("--");
            }
            Console.ResetColor(); // por gusto
            Console.CursorVisible = false; // toque estético
        }
    }
}
