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
        // Lista MemoJugadas; 


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
            if (d == 'c') ClickCasilla();
        }
    }
}