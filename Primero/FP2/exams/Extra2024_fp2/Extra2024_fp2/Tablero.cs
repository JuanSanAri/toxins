using System;
using System.IO;
using Listas;

namespace LightsOut
{
    class Tablero
    {
        int FILS, COLS;     // numero de filas y columnas del tablero       
        bool[,] mat;       // matriz de casillas del tablero
        struct Posicion { public int fil, col; }    // tipo para coordenadas 2D    
        Posicion cursor; // coordenadas de la posicion del cursor

        // lista de posiciones para guardar jugadas y poder deshacerlas
        Lista MemoJugadas;


        // Constructora principal
        public Tablero(string[] lineas)
        {
            // FILS, COLS
            FILS = lineas.Length;
            COLS = lineas[0].Length;
            // mat
            mat = new bool[FILS, COLS];
            for (int i = 0; i < FILS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    if (lineas[i][j] == '*') mat[i, j] = true;
                    else mat[i, j] = false;
                }
            }
            // Cursor
            cursor.fil = cursor.col = 0;
            // Memoria de jugadas
            MemoJugadas = new Lista();
        }

        // Segunda constructora
        public Tablero(int fils, int cols, int n)
        {
            // FILS, COLS
            FILS = fils;
            COLS = cols;
            // Los arrays de bools se inicializan a false por defecto
            mat = new bool[FILS, COLS];
            cursor.fil = cursor.col = 0;
            Random rnd = new Random();
            for (int i = 0; i < n; i++)
            {
                // rnd.Next(FILS) == rnd.Next (0,FILS)
                cursor.fil = rnd.Next(FILS);
                cursor.col = rnd.Next(COLS);
                ClickCasilla();
            }
            // Memoria de jugadas
            MemoJugadas = new Lista();
        }

        private void ClickCasillaMemo()
        {
            ClickCasilla();
            int k = 10 * cursor.fil + cursor.col;
            MemoJugadas.InsertaPri(k);
        }

        private void DeshacerJugada()
        {
            int aux = MemoJugadas.DamePri();
            cursor.fil = aux / 10;
            cursor.col = aux % 10;
            ClickCasilla();

            MemoJugadas.EliminaPri();
        }

        public void Dibuja()
        {
            Console.Clear();
            // Tablero
            for (int i = 0; i < FILS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    if (mat[i, j]) Console.BackgroundColor = ConsoleColor.Yellow;
                    else Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write("  ");
                }
                Console.WriteLine();
            }
            // Cursor
            Console.SetCursorPosition(2 * cursor.col, cursor.fil);
            Console.ForegroundColor = ConsoleColor.Black;
            if (mat[cursor.fil, cursor.col]) Console.BackgroundColor = ConsoleColor.Yellow;
            else Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write("<>");
            Console.ResetColor();
        }

        private void ClickCasilla()
        {
            mat[cursor.fil, cursor.col] = !mat[cursor.fil, cursor.col];
            if (cursor.fil - 1 >= 0) mat[cursor.fil - 1, cursor.col] = !mat[cursor.fil - 1, cursor.col];
            if (cursor.fil + 1 <= FILS - 1) mat[cursor.fil + 1, cursor.col] = !mat[cursor.fil + 1, cursor.col];
            if (cursor.col - 1 >= 0) mat[cursor.fil, cursor.col - 1] = !mat[cursor.fil, cursor.col - 1];
            if (cursor.col + 1 <= COLS - 1) mat[cursor.fil, cursor.col + 1] = !mat[cursor.fil, cursor.col + 1];
        }

        public void FinJuego(out bool fin)
        {
            fin = true;
            int i = 0;
            int j = 0;
            while (i < FILS && fin)
            {
                while (j < COLS && fin)
                {
                    if (mat[i, j]) fin = false;
                    j++;
                }
                j = 0;
                i++;
            }
        }

        public void ProcesaInput(char d)
        {
            switch (d)
            {
                case 'u': if (cursor.fil > 0) cursor.fil--; break;
                case 'd': if (cursor.fil < FILS - 1) cursor.fil++; break;
                case 'l': if (cursor.col > 0) cursor.col--; break;
                case 'r': if (cursor.col < COLS - 1) cursor.col++; break;
            }
            if (d == 'c') ClickCasillaMemo();
            if (d == 'z' && !MemoJugadas.EsVacia()) DeshacerJugada();
            if (d == 's') Guardar();
            if (d == 'o')
            {
                try
                {
                    Restaurar();
                }
                catch (Exception e)
                {
                    Console.SetCursorPosition(0, FILS + 1);
                    Console.WriteLine("Error: " + e.Message);
                }
            }
        }

        public void Guardar()
        {
            StreamWriter sw = new StreamWriter("partida.txt");
            for (int i = 0; i < FILS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    if (mat[i, j]) sw.Write('*');
                    else sw.Write('.');
                }
                sw.Write('|');
            }
            sw.Close();
        }

        void Restaurar()
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader("partida.txt");
                string linea = sr.ReadLine();
                string[] filas = linea.Split('|', StringSplitOptions.RemoveEmptyEntries);

                // Validar dimensiones
                if (filas.Length != FILS) throw new Exception("Número de filas incorrecto");

                for (int i = 0; i < FILS; i++)
                {
                    if (filas[i].Length != COLS) throw new Exception("Número de columnas incorrecto");
                    int j = 0;
                    bool valido = true;
                    while (j < COLS && valido)
                    {
                        if (filas[i][j] != '*' && filas[i][j] != '.') valido = false;
                        j++;
                    }
                    if (!valido) throw new Exception("Carácter no válido");
                }
                // Construir con la primera constructora
                Tablero nuevo = new Tablero(filas);
                mat = nuevo.mat;
                FILS = nuevo.FILS;
                COLS = nuevo.COLS;
                cursor = nuevo.cursor;
                MemoJugadas = new Lista();
            }
            catch (FileNotFoundException)
            { throw new FileNotFoundException("No se encuentra el archivo partida.txt"); }
            catch (Exception)
            { throw new Exception("Formato del archivo incorrecto"); }
            finally { if (sr != null) sr.Close(); }
        }
    }
}