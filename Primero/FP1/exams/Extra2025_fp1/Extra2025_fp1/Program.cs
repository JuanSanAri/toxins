using System;
using System.Runtime.CompilerServices;
using static System.Net.WebRequestMethods;
//using System.Threading;

namespace Main
{
    class MainClass
    {
        static Random rnd = new Random(); // generador de números aleatorios

        struct Coor { public int fil, col; }; // estructura para representar posiciones en la matriz

        static bool[,] patron = {{false,false,false,false},  // patrón cuadrado 2x2 con borde vacío
				           		 {false, true,true ,false},
                                 {false, true,true ,false},
                                 {false,false,false,false}};

        public static void Main()
        {
            /* Variables usadas antes de crear Menu
            int tamf = 4;
            int tamc = 4;
            */
            Console.CursorVisible = false;


            // Variables
            bool[,] juego;
            int gen = 0;
            bool terminado = false;

            // Bucle
            Menu(out juego);
            while (!terminado)
            {
                Render(juego, gen);

                int filPat, colPat;
                BuscaPatron(juego, patron, out filPat, out colPat);
                if (filPat != -1)
                {
                    Console.WriteLine($" Patrón encontrado en posición ({filPat},{colPat})");
                    Console.ReadLine();
                }

                bool[,] juegoNew = SiguienteGen(juego, ref gen);

                if (Estable(juego, juegoNew))
                {
                    terminado = true;
                    Console.Clear();
                    Console.Write("La población es estable");
                }
                else juego = juegoNew;

                Thread.Sleep(500);
            }
        }

        static void Inicializa(int fils, int cols, out bool[,] mat)
        {
            mat = new bool[fils, cols];

            for (int i = 0; i < fils; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int a = rnd.Next(0, 2); // puede salir 0 ó 1 (50% cada)

                    if (a == 0) mat[i, j] = false; // si a es 0, vacía (false)
                    else mat[i, j] = true; // si es 1, célula (true)
                }
            }
        }

        static void Render(bool[,] mat, int gen)
        {
            Console.Clear();
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                Console.Write(" "); // Espacio para que se vea mejor la 1a columna
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    if (mat[i, j]) Console.Write("0 ");
                    else Console.Write(". ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine(" Gen: " + gen);
        }

        static int Next(int n, int tam) // Devuelve n siguiente
        {
            return ((n + 1) % tam);
        }

        static int Prev(int n, int tam) // Devuelve n anterior
        {
            if (n == 0) n = tam - 1;
            else n--;
            return n;
        }

        static int NumVecinas(bool[,] mat, int fil, int col)
        {
            int contador = 0;

            int[] filas = { Prev(fil, mat.GetLength(0)), fil, Next(fil, mat.GetLength(0)) };
            int[] cols = { Prev(col, mat.GetLength(1)), col, Next(col, mat.GetLength(1)) };

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Saltar la propia casilla
                    if (filas[i] == fil && cols[j] == col)
                        continue;

                    if (mat[filas[i], cols[j]]) contador++;
                }
            }
            return contador;
        }
        // A saco también funciona perfectamente, pero está feo
        /*
        static int NumVecinas(bool[,] mat, int fil, int col)
        {
            int contador = 0;
            int pf = Prev(fil, mat.GetLength(0)); // Fila anterior
            int pc = Prev(col, mat.GetLength(1));
            int nf = Next(fil, mat.GetLength(0)); // Fila next (siguiente)
            int nc = Next(col, mat.GetLength(1));

            // Previous fila
            if (mat[pf, pc]) contador++;
            if (mat[pf, col]) contador++;
            if (mat[pf, nc]) contador++;
            // Misma fila
            if (mat[fil, pc]) contador++;
            if (mat[fil, nc]) contador++;
            // Next fila
            if (mat[nf, pc]) contador++;
            if (mat[nf, col]) contador++;
            if (mat[nf, nc]) contador++;

            return contador;
        }*/

        static bool[,] SiguienteGen(bool[,] mat, ref int gen)
        {
            bool[,] mat2 = new bool[mat.GetLength(0), mat.GetLength(1)];

            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    int nVecinos = NumVecinas(mat, i, j);

                    if ((nVecinos == 2 || nVecinos == 3) && mat[i, j]) mat2[i, j] = true;
                    else if ((nVecinos > 3 || nVecinos < 2) && mat[i, j]) mat2[i, j] = false;
                    else if (nVecinos == 3 && !mat[i, j]) mat2[i, j] = true;
                    else mat2[i, j] = mat[i, j]; // en cualquier otro caso, sigue como está
                }
            }
            gen++;
            return mat2;
        }

        static bool Estable(bool[,] mat1, bool[,] mat2)
        {
            for (int i = 0; i < mat1.GetLength(0); i++)
            {
                for (int j = 0; j < mat1.GetLength(1); j++)
                {
                    if (mat1[i, j] != mat2[i, j]) return false;
                }
            }
            return true;
        }

        static void LeeArchivo(string file, out bool[,] mat)
        {
            StreamReader sr = new StreamReader(file);

            // Primer readline es número de filas
            string s = sr.ReadLine();
            int numf = int.Parse(s);
            // Segundo readline es número de columnas
            s = sr.ReadLine();
            int numc = int.Parse(s);

            // Valor de mat
            mat = new bool[numf, numc]; // Inicializar mat
            for (int i = 0; i < numf; i++)
            {
                s = sr.ReadLine();
                for (int j = 0; j < numc; j++)
                {
                    if (s[j] == 'O') mat[i, j] = false;
                    else mat[i, j] = true;
                }
            }
            sr.Close();
        }

        static void Menu(out bool[,] mat)
        {
            char a;
            do
            {
                Console.Write("Quieres jugar con una población de archivo(e) o aleatoria(a)? [e/a]: ");
                a = Console.ReadKey().KeyChar;
            } while (a != 'a' && a != 'e' && a != 'A' && a != 'E');

            if (a == 'e') // Archivo
            {
                LeeArchivo("file.txt", out mat);
            }
            else // Aleatorio
            {
                Console.Clear();
                Console.Write("Número de filas que quieres: ");
                int nfils = int.Parse(Console.ReadLine());
                Console.Write("Número de columnas que quieres: ");
                int ncols = int.Parse(Console.ReadLine());

                Inicializa(nfils, ncols, out mat);
            }
        }

        static bool BuscaPatronPos(bool[,] mat, bool[,] pat, int fil, int col)
        {
            int filsM = mat.GetLength(0);
            int colsM = mat.GetLength(1);

            for (int i = 0; i < pat.GetLength(0); i++)
            {
                for (int j = 0; j < pat.GetLength(1); j++)
                {
                    int mf = (fil + i) % filsM;
                    int mc = (col + j) % colsM;

                    if (mat[mf, mc] != pat[i, j]) return false;
                }
            }
            return true;
        }

        static void BuscaPatron(bool[,] mat, bool[,] pat, out int fil, out int col)
        {
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    if (BuscaPatronPos(mat, pat, i, j))
                    {
                        fil = i;
                        col = j;
                        return;
                    }
                }
            }
            fil = col = -1;
        }
    }
}