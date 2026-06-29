namespace Ordinaria2023_fp1
{
    class SolitarioParejas
    {
        static Random rnd = new Random(); // generador de aleatorios (se usará al final)
        const int DESCUBIERTAS = 8; // número de cartas descubiertas en la mesa
        static void Main()
        {
            int[] mazo = new int[40], // array de cartas del mazo
            mesa = new int[DESCUBIERTAS]; // array de cartas de la mesa
            int prim; // posición de la primera carta aún no extraída del mazo


            // Variables
            bool terminado = false;
            string res;
            string leeResp;

            // Bucle
            do
            {
                Console.Write("Quieres cargar partida de archivo? [s/n]: ");
                leeResp = Console.ReadLine();
            } while (leeResp != "s" && leeResp != "n");
            if (leeResp == "s") Lee(out mesa, out mazo, out prim);
            else
            {
                InicializaMazoAleatorio(mazo, out prim);
                InicializaMesa(mazo, ref prim, out mesa);
            }

            while (!terminado)
            {
                // Render
                Render(mesa, prim);
                // 1. Mesa vacía?
                if (MesaVacia(mesa))
                {
                    terminado = true;
                    Console.Clear();
                    Console.WriteLine("HAS GANADO");
                }
                // 2. Se puede seguir jugando?
                else if (!HayPar(mesa))
                {
                    Console.WriteLine("No hay pares en la mesa, has perdido");
                    terminado = true;
                }
                // Si no se cumple nada anterior seguimos
                else
                {
                    // Pide juego
                    PidePosiciones(out int p1, out int p2);
                    // Guardado
                    if (p1 == p2)
                    {
                        Console.WriteLine();
                        do
                        {
                            Console.Write("Quieres guardar la partida? [s/n]: ");
                            res = Console.ReadLine();
                        } while (res != "s" && res != "n");
                        if (res == "s")
                        {
                            Salva(mesa, mazo, prim);
                            Console.Write("Guardado en el archivo: solitario");
                        }
                        terminado = true;
                    }
                    // Juego
                    if (mesa[p1] % 10 == mesa[p2] % 10)
                    {
                        ExtraeCarta(ref mesa, p1, mazo, ref prim);
                        ExtraeCarta(ref mesa, p2, mazo, ref prim);
                    }
                }
            }
        }

        static void InicializaMazoAleatorio(int[] mazo, out int prim)
        {
            prim = 0;
            // Rellenamos mazo
            for (int i = 0; i < mazo.Length; i++)
            {
                mazo[i] = i;
            }
            // Intercambiar la carta de la pos actual con cualquier otra
            // con la pos = random[posActual, 39(mazoLenght)]
            for (int i = 0; i < mazo.Length; i++)
            {
                int j = rnd.Next(i, mazo.Length);
                int aux = mazo[i];
                mazo[i] = mazo[j];
                mazo[j] = aux;
            }
        }

        static void ExtraeCarta(ref int[] mesa, int pos, int[] mazo, ref int prim)
        {
            if (prim >= mazo.Length)
            {
                mesa[pos] = -1;
            }
            else
            {
                mesa[pos] = mazo[prim];
                prim++;
            }
        }

        static void InicializaMesa(int[] mazo, ref int prim, out int[] mesa)
        {
            mesa = new int[DESCUBIERTAS];
            for (int i = 0; i < DESCUBIERTAS; i++)
            {
                ExtraeCarta(ref mesa, i, mazo, ref prim);
            }
        }

        static void Render(int[] mesa, int prim)
        {
            Console.Clear();

            Console.Write(" Posiciones: ");
            for (int i = 0; i < DESCUBIERTAS; i++)
            {
                Console.Write(i + "  ");
            }
            Console.WriteLine();
            Console.Write(" Cartas:     ");
            for (int i = 0; i < DESCUBIERTAS; i++)
            {
                // Colores
                if (mesa[i] == -1) Console.ForegroundColor = ConsoleColor.Magenta;
                else if (mesa[i] < 10 && mesa[i] > -1) Console.ForegroundColor = ConsoleColor.Yellow;
                else if (mesa[i] < 20) Console.ForegroundColor = ConsoleColor.Red;
                else if (mesa[i] < 30) Console.ForegroundColor = ConsoleColor.Blue;
                else if (mesa[i] < 40) Console.ForegroundColor = ConsoleColor.Green;
                // Texto
                if (mesa[i] < 0) Console.Write(".  ");
                else Console.Write(mesa[i] % 10 + "  ");
            }
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($" Quedan {40 - prim} cartas en el mazo");
            Console.WriteLine();
        }

        static void PidePosiciones(out int p1, out int p2)
        {
            do
            {
                Console.Write(" Introduce posición 1 [0-7]: ");
                p1 = int.Parse(Console.ReadLine());
            } while (p1 < 0 || p1 >= DESCUBIERTAS);
            do
            {
                Console.Write(" Introduce posición 2 [0-7]: ");
                p2 = int.Parse(Console.ReadLine());
            } while (p2 < 0 || p2 >= DESCUBIERTAS);
        }

        static bool HayPar(int[] mesa)
        {
            bool hayp = false;
            int i = 0;
            while (!hayp && i < DESCUBIERTAS - 1)
            {
                for (int j = i + 1; j < DESCUBIERTAS; j++)
                {
                    if (mesa[i] % 10 == mesa[j] % 10 && mesa[i] > -1) hayp = true;
                }
                i++;
            }
            return hayp;
        }

        static bool MesaVacia(int[] mesa)
        {
            bool vacia = true;
            for (int i = 0; i < DESCUBIERTAS; i++)
            {
                if (mesa[i] != -1) vacia = false;
            }
            return vacia;
        }

        static void Salva(int[] mesa, int[] mazo, int prim)
        {
            StreamWriter sw = new StreamWriter("solitario");
            // Mesa
            for (int i = 0; i < DESCUBIERTAS; i++)
            {
                sw.Write(mesa[i] + " ");
            }
            sw.WriteLine();
            // Mazo
            for (int i = 0; i < mazo.Length; i++)
            {
                sw.Write(mazo[i] + " ");
            }
            sw.WriteLine();
            // Primero
            sw.Write(prim);
            sw.Close();
        }

        static void Lee(out int[] mesa, out int[] mazo, out int prim)
        {
            StreamReader sr = new StreamReader("solitario");
            // Mesa
            mesa = new int[DESCUBIERTAS];
            string[] nums = (sr.ReadLine()).Split(" ", StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < DESCUBIERTAS; i++)
            {
                mesa[i] = int.Parse(nums[i]);
            }
            // Mazo
            mazo = new int[40];
            nums = (sr.ReadLine()).Split(" ", StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < mazo.Length; i++)
            {
                mazo[i] = int.Parse(nums[i]);
            }
            // Primero
            prim = int.Parse(sr.ReadLine());
            sr.Close();
        }
    }
}