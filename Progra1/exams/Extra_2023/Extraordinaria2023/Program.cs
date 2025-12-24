using System;
using System.IO;

class Hitori
{

    const int N = 4; // para evitar usar getLenght() tol rato
    static void Main()
    {
        int[,] tab;      // números del tablero
        bool[,] tachadas; // casillas tachadas
        int fil, col;     // posición del cursor

        fil = col = 0;

        tab = new int[,]{
            {4, 4, 1, 2},
            {3, 2, 3, 1},
            {1, 3, 2, 4},
            {2, 1, 4, 3}};

        tachadas = new bool[,]{
            {false, false, false, false},
            {false, false, false, false},
            {false, false, false, false},
            {false, false, false, false}};


        //---
        bool acabado = false;

        Render(tab, tachadas, fil, col);

        while (!acabado)
        {
            char c = LeeInput();
            ProcesaInput(c, tachadas, ref fil, ref col);

            Render(tab, tachadas, fil, col);


            // DEBUG
            Console.SetCursorPosition(0, 10);
            Console.WriteLine();
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Console.Write(tachadas[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.Write($"fil: {fil}, col: {col}");
        }

    } // Main

    static void Render(int[,] tab, bool[,] tachadas, int fil, int col)
    {
        Console.Clear();

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                // Cursor
                if (i == fil && j == col)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                // Texto
                if (tachadas[i, j] == false)
                {
                    Console.Write(tab[i, j] + " ");
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.Write(tab[i, j]);
                    Console.ResetColor();
                    Console.Write(" ");
                }
                Console.ResetColor(); // resetColor del cursor
            }
            Console.WriteLine();
        }

        Console.SetCursorPosition(col, fil);
    }

    static void ClickCasilla(ref bool[,] tachadas, int fil, int col)
    {
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                // lados
                if ((i - 1 < 0) && (tachadas[i + 1, j] == false) && (tachadas[i, j + 1] == false) && (tachadas[i, j - 1] == false)) { tachadas[i, j] = true; } // arriba
                else if ((i + 1 > N - 1) && (tachadas[i - 1, j] == false) && (tachadas[i, j + 1] == false) && (tachadas[i, j - 1] == false)) { tachadas[i, j] = true; } // abajo
                else if ((j - 1 < 0) && (tachadas[i, j + 1] == false) && (tachadas[i + 1, j] == false) && (tachadas[i - 1, j] == false)) { tachadas[i, j] = true; } // izquierda 
                else if ((j + 1 > N - 1) && (tachadas[i, j - 1] == false) && (tachadas[i + 1, j] == false) && (tachadas[i - 1, j] == false)) { tachadas[i, j] = true; } // derecha
                // esquinas
                else if ((j + 1 > N - 1) && (tachadas[i, j - 1] == false) && (i + 1 > N - 1) && (tachadas[i - 1, j] == false)) { tachadas[i, j] = true; } // abajo-derecha
                else if ((j - 1 < 0) && (tachadas[i, j + 1] == false) && (i - 1 < 0) && (tachadas[i - 1, j] == false)) { tachadas[i, j] = true; } // abajo-izquierda
                else if ((j - 1 < 0) && (tachadas[i, j + 1] == false) && (i - 1 < 0) && (tachadas[i + 1, j] == false)) { tachadas[i, j] = true; } // arriba-izquierda
                else if ((j + 1 > N - 1) && (tachadas[i, j - 1] == false) && (i - 1 < 0) && (tachadas[i + 1, j] == false)) { tachadas[i, j] = true; } // arriba-derecha
                // centro
                else if ((tachadas[i + 1, j] == false) && (tachadas[i - 1, j] == false) && (tachadas[i, j + 1] == false) && (tachadas[i, j - 1] == false)) { tachadas[i, j] = true; }

                // destachar
                if (tachadas[i, j] == true) { tachadas[i, j] = false; }

            }
        }
        //tachadas[fil, col] = !tachadas[fil, col];
    }

    static char LeeInput()
    {
        char d = ' ';

        string tecla = Console.ReadKey(true).Key.ToString();
        switch (tecla)
        {
            /* INPUTS ELEMENTALES PARA EL JUEGO BÁSICO */

            // movimiento del cursor 	
            case "LeftArrow": d = 'l'; break;
            case "UpArrow": d = 'u'; break;
            case "RightArrow": d = 'r'; break;
            case "DownArrow": d = 'd'; break;

            // pulsacion de casilla (click)
            case "Spacebar": d = 'c'; break;

            // terminar juego
            case "Escape": case "q": d = 'q'; break;
            default: d = ' '; break;
        }
        return d;
    }

    static void ProcesaInput(char c, bool[,] tachadas, ref int fil, ref int col)
    {
        // eje x
        if (c == 'l' && col > 0) { col--; }
        if (c == 'r' && col < N - 1) { col++; }
        // eje y
        if (c == 'u' && fil > 0) { fil--; }
        if (c == 'd' && fil < N - 1) { fil++; }

        if (c == 'c') { ClickCasilla(ref tachadas, fil, col); }
    }

    //end
}