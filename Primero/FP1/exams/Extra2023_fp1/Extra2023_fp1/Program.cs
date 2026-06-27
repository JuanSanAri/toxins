
class Hitori
{
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

        // Cosas mías ya
        Console.CursorVisible = false;
        bool terminado = false;
        char d = ' ';
        string file = "file.txt";
        string respuesta = " ";

        do
        {
            Console.Write("Recuperar partida [s/n]? ");
            respuesta = Console.ReadLine();
        } while (respuesta != "s" && respuesta != "n");

        if (respuesta == "s")
        {
            LeeArchivo(file, out tab, out tachadas, out fil, out col);

        }
        // Si no dice que si simplemente renderizamos con el tab ejemplo

        Render(tab, tachadas, fil, col);
        while (!terminado)
        {
            d = LeeInput();
            ProcesaInput(d, tachadas, ref fil, ref col);
            Render(tab, tachadas, fil, col);

            if (d == 'q')
            {
                terminado = true;
                Console.Clear();
                string r = "";
                do
                {
                    Console.Write("Has salido!! Te gustaría guardar la partida? [s/n]");
                    r = Console.ReadLine();
                } while (r != "s" && r != "n");

                if (r == "s")
                {
                    SalvaArchivo("file.txt", tab, tachadas);
                    Console.Clear();
                    Console.WriteLine("Partida guardada en el archivo file.txt");
                }
            }

            if (!RepetidosMatriz(tab, tachadas))
            {
                terminado = true;
                Console.Clear();
                Console.WriteLine("Enhorabuena, has ganado!");
            }
        }


    } // Main

    static void Render(int[,] tab, bool[,] tachadas, int fil, int col)
    {
        int N = tab.GetLength(0);
        Console.Clear();

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                // Color cursor
                if (i == fil && j == col)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                // Color tachadas
                if (tachadas[i, j]) Console.BackgroundColor = ConsoleColor.DarkRed;
                // Número
                Console.Write(tab[i, j]);
                Console.ResetColor();
                // Espacio entre medias
                Console.Write(" ");
            }
            Console.WriteLine();
        }
        Console.SetCursorPosition(col * 2, fil);
    }

    static void ClickCasilla(bool[,] tachadas, int fil, int col)
    {
        int N = tachadas.GetLength(0);
        bool puedeTachar = true;

        // condiciones tachado
        if (fil > 0 && tachadas[fil - 1, col]) { puedeTachar = false; }
        if (fil < N - 1 && tachadas[fil + 1, col]) { puedeTachar = false; }
        if (col > 0 && tachadas[fil, col - 1]) { puedeTachar = false; }
        if (col < N - 1 && tachadas[fil, col + 1]) { puedeTachar = false; }

        // tachar
        if (tachadas[fil, col]) { tachadas[fil, col] = false; }
        else if (puedeTachar) { tachadas[fil, col] = true; }
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
        int N = tachadas.GetLength(0);
        // Movimiento de cursor
        switch (c)
        {
            case 'u': if (fil > 0) fil--; break;
            case 'd': if (fil < N - 1) fil++; break;
            case 'r': if (col < N - 1) col++; break;
            case 'l': if (col > 0) col--; break;
        }
        // Des/Marca tachada
        if (c == 'c') ClickCasilla(tachadas, fil, col);
    }

    static int[] DameFil(int[,] tab, bool[,] tachadas, int i)
    {
        int[] fila = new int[tab.GetLength(0)];

        for (int j = 0; j < fila.Length; j++)
        {
            if (tachadas[i, j]) fila[j] = -1;
            else fila[j] = tab[i, j];
        }

        return fila;
    }

    static int[] DameCol(int[,] tab, bool[,] tachadas, int i)
    {
        int[] col = new int[tab.GetLength(0)];

        for (int j = 0; j < col.Length; j++)
        {
            if (tachadas[j, i]) col[j] = -1;
            else col[j] = tab[j, i];
        }

        return col;
    }

    static bool RepetidosVector(int[] v)
    {
        for (int i = 0; i < v.Length; i++)
        {
            for (int j = i + 1; j < v.Length; j++)
            {
                if (v[i] == v[j] && v[i] != -1) return true;
            }
        }
        return false;
    }

    static bool RepetidosMatriz(int[,] tab, bool[,] tachadas)
    {
        bool repetidos = false;
        int[] fila = new int[tab.GetLength(0)];
        int[] col = new int[tab.GetLength(0)];

        for (int i = 0; i < tab.GetLength(0); i++)
        {
            fila = DameFil(tab, tachadas, i);
            col = DameCol(tab, tachadas, i);

            if (RepetidosVector(fila) || RepetidosVector(col)) repetidos = true;
        }
        return repetidos;
    }

    static void LeeArchivo(string file, out int[,] tab, out bool[,] tachadas, out int fil, out int col)
    {
        StreamReader sr = new StreamReader("file.txt");

        // Tamaño
        int tam = int.Parse(sr.ReadLine());
        col = fil = tam;

        // Matriz tab
        string linea;
        tab = new int[fil, col];
        for (int i = 0; i < tam; i++)
        {
            linea = sr.ReadLine();
            string[] tabN = linea.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < tam; j++)
            {
                tab[i, j] = int.Parse(tabN[j]);
            }
        }
        // Matriz tachadas
        tachadas = new bool[fil, col];
        for (int i = 0; i < tam; i++)
        {
            linea = sr.ReadLine();
            string[] tachadasN = linea.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < tam; j++)
            {
                if (tachadasN[j] == "f") tachadas[i, j] = false;
                else tachadas[i, j] = true;
            }
        }
        sr.Close();
    }

    static void SalvaArchivo(string file, int[,] tab, bool[,] tachadas)
    {
        StreamWriter archivo = new StreamWriter(file);

        int N = tab.GetLength(0);

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
}