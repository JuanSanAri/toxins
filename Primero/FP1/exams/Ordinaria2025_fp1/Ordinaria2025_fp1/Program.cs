class Takuzu
{
    static void Main()
    {
        const int N = 4;
        char[,] tab;
        int fil, col;
        fil = col = 0;

        // cosas mías
        Console.CursorVisible = false;
        bool terminado = false;
        char c = ' ';
        string respuesta;
        string file = "takuzu6";

        do
        {
            Console.Write("Quieres usar el ejemplo o cargar un archivo?[e/a]");
            respuesta = Console.ReadLine();
        } while (respuesta != "a" && respuesta != "e");

        if (respuesta == "a")
        {
            LeeArchivo(file, out tab);
        }
        else
        {
            tab = new char[N, N] {
            {'.','1','.','0'},
            {'.','.','0','.'},
            {'.','0','.','.'},
            {'1','1','.','0'} };
        }
        bool[,] fijas = new bool[tab.GetLength(0), tab.GetLength(1)];
        Inicializa(tab, fijas, fil, col);
        Renderiza(tab, fijas, fil, col);
        while (!terminado)
        {
            c = LeeInput();
            ProcesaInput(c, tab, fijas, ref fil, ref col);
            Renderiza(tab, fijas, fil, col);

            Console.SetCursorPosition(0, 10);
            MuestraResultado(tab);

            if (c == 'q')
            {
                terminado = true;
                Console.Clear();
                Console.WriteLine("Has salido");
            }

            if (TabLleno(tab))
            {
                terminado = true;
                Console.Clear();
                Console.WriteLine("ENHORABUENA");
            }
        }
    }

    static void Inicializa(char[,] tab, bool[,] fijas, int fil, int col)
    {
        int N = tab.GetLength(0);

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (tab[i, j] == '.') fijas[i, j] = false;
                else fijas[i, j] = true;
            }
        }
    }

    static void Renderiza(char[,] tab, bool[,] fijas, int fil, int col)
    {
        Console.Clear();
        int N = tab.GetLength(0);
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                // Color caracteres
                if (fijas[i, j]) Console.ForegroundColor = ConsoleColor.Blue;
                else Console.ForegroundColor = ConsoleColor.Yellow;
                // Color cursor
                if (i == fil && j == col)
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                // Caracter
                Console.Write(tab[i, j]);
                Console.ResetColor();
                // Espacio
                Console.Write(" ");
            }
            Console.WriteLine();
        }
        Console.SetCursorPosition(col * 2, fil);
    }

    static char LeeInput()
    {
        char d = ' ';
        string tecla = Console.ReadKey(true).Key.ToString();
        switch (tecla)
        {
            case "LeftArrow": d = 'l'; break;
            case "UpArrow": d = 'u'; break;
            case "RightArrow": d = 'r'; break;
            case "DownArrow": d = 'd'; break;
            case "D0": d = '0'; break;
            case "D1": d = '1'; break;
            case "Spacebar": d = 's'; break;
            case "Escape": d = 'q'; break;
            default: d = ' '; break;
        }
        return d;
    }

    static void ProcesaInput(char c, char[,] tab, bool[,] fijas, ref int fil, ref int col)
    {
        int N = tab.GetLength(0);
        // Movimiento
        switch (c)
        {
            case 'u': if (fil > 0) fil--; break;
            case 'd': if (fil < N - 1) fil++; break;
            case 'l': if (col > 0) col--; break;
            case 'r': if (col < N - 1) col++; break;
        }
        // Caracteres
        if (!fijas[fil, col])
        {
            if (c == '0') tab[fil, col] = '0';
            if (c == '1') tab[fil, col] = '1';
            if (c == 's') tab[fil, col] = '.';
        }
    }

    static bool TabLleno(char[,] tab)
    {
        int N = tab.GetLength(0);
        bool lleno = true;
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (tab[i, j] == '.') lleno = false;
            }
        }
        return lleno;
    }

    static void SacaFilCol(int k, char[,] tab, char[] filk, char[] colk)
    {
        int N = tab.GetLength(0);

        for (int i = 0; i < N; i++)
        {
            filk[i] = tab[k, i];
            colk[i] = tab[i, k];
        }
    }

    static bool TresSeguidos(char[] v)
    {
        bool tresSeg = false;
        for (int i = 0; i < v.Length - 2; i++)
        {
            if (v[i] == v[i + 1] && v[i] == v[i + 2]) tresSeg = true;
        }
        return tresSeg;
    }

    static bool IgCerosUnos(char[] v)
    {
        int cont0 = 0;
        int cont1 = 0;
        for (int i = 0; i < v.Length; i++)
        {
            if (v[i] == '0') cont0++;
            else if (v[i] == '1') cont1++;
        }
        return cont0 == cont1;
    }

    static void MuestraResultado(char[,] tab)
    {
        int N = tab.GetLength(0);
        char[] filk = new char[N];
        char[] colk = new char[N];

        for (int i = 0; i < N; i++)
        {
            SacaFilCol(i, tab, filk, colk);
            if (TresSeguidos(filk)) Console.WriteLine("Tres iguales seguidos en fila " + i);
            if (TresSeguidos(colk)) Console.WriteLine("Tres iguales seguidos en columna " + i);
            if (!IgCerosUnos(filk)) Console.WriteLine("No mismo num de 0s y 1s en fila " + i);
            if (!IgCerosUnos(colk)) Console.WriteLine("No mismo num de 0s y 1s en columna " + i);
        }
    }

    static void LeeArchivo(string file, out char[,] tab)
    {
        StreamReader sr = new StreamReader(file);

        string s = sr.ReadLine();
        int tam = int.Parse(s);

        tab = new char[tam, tam];
        for (int i = 0; i < tam; i++)
        {
            s = sr.ReadLine();
            for (int j = 0; j < tam; j++)
            {
                tab[i, j] = s[j];
            }
        }
        sr.Close();
    }
}