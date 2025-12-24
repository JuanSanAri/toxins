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
        bool salir = false;
        Render(tab, tachadas, fil, col);

        while (RepetidosMatriz(tab, tachadas) && !salir)
        {
            char c = LeeInput();

            if (c == 'q') { salir = true; }
            else ProcesaInput(c, tachadas, ref fil, ref col);

            Render(tab, tachadas, fil, col);
        }

        if (!RepetidosMatriz(tab, tachadas))
        {
            Console.Clear();
            Console.WriteLine("has ganao");
        }
        if (salir) { SalvaArchivo("file", tab, tachadas); }

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
        bool puedeTachar = true;

        // condiciones tachado
        if (fil > 0 && tachadas[fil - 1, col]) { puedeTachar = false; }
        if (fil < N - 1 && tachadas[fil + 1, col]) { puedeTachar = false; }
        if (col > 0 && tachadas[fil, col - 1]) { puedeTachar = false; }
        if (col < N - 1 && tachadas[fil, col + 1]) { puedeTachar = false; }

        // tachar
        if (tachadas[fil, col]) { tachadas[fil, col] = false; }
        else if (puedeTachar) { tachadas[fil, col] = true; }

        // NOTA: este método es importantísimo, ayuda un montón a la comprensión
        // de límites y te remodela la cabeza a mejor cuando estás empezando a programar

        // Si te fijas lo que buscamos son excepciones, en vez de decirle que trabaje solo
        // cuando se cumplen un montón de condiciones, le decimos: oye trabaja excepto cuando pasa esto.

        // barbaridad de script
    }

    static int[] DameFil(int[,] tab, bool[,] tachadas, int i)
    {
        int[] arrayFil = new int[N];

        for (int j = 0; j < N; j++)
        {
            if (tachadas[i, j]) arrayFil[j] = -1;
            else arrayFil[j] = tab[i, j];
        }
        return arrayFil;
    }

    static int[] DameCol(int[,] tab, bool[,] tachadas, int i)
    {
        int[] arrayCol = new int[N];

        for (int j = 0; j < N; j++)
        {
            if (tachadas[j, i]) arrayCol[j] = -1;
            else arrayCol[j] = tab[j, i];
        }
        return arrayCol;
    }

    static bool RepetidosVector(int[] v)
    {
        for (int i = 0; i < N; i++)
        {
            for (int j = i + 1; j < N; j++)
            {
                if (v[i] == v[j] && v[i] != -1) return true;
                // ^^en esta condición podría meter un "&& v[j] != -1" al final
                // pero para lo que nos pide con esto está perfecto
            }
        }
        return false;
    }

    static bool RepetidosMatriz(int[,] tab, bool[,] tachadas)
    {
        int[] columna = new int[N];
        int[] fila = new int[N];

        for (int i = 0; i < N; i++)
        {
            fila = DameFil(tab, tachadas, i);
            columna = DameCol(tab, tachadas, i);

            bool checkCol = RepetidosVector(columna);
            bool checkFil = RepetidosVector(fila);

            if (checkCol || checkFil) return true;
        }
        return false;
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

    static void SalvaArchivo(string file, int[,] tab, bool[,] tachadas)
    {
        StreamWriter archivo = new StreamWriter("file");
        archivo.WriteLine(N);
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                archivo.Write(tab[i, j] + " ");
            }
            archivo.WriteLine();
        }
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (tachadas[i, j]) archivo.Write("t ");
                else archivo.Write("f ");
            }
            archivo.WriteLine();
        }
        archivo.Close();
    }

    static void LeeArchivo(string file, int[,] tab, bool[,] tachadas, int fil, int col)
    {

    }

    //end
}